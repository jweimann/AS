using Akka.Actor;
using AS.Messages;
using System.Diagnostics;
using AS.Messages.Game;
using AS.Messages.Entities;
using Helios.Net;
using AS.Client.Messages;
using AS.Client.Messages.Errors;
using AS.Client.Messages.ClientRequests;
using AS.Client.Messages.Game;
using AS.Client.Messages.Entities;
using AS.Common;
using System.Collections.Generic;
using System;
using SG.Client.Messages;

namespace AS.Actors.UserActors
{
    public class User : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef _lobbyUser;
        private IActorRef _room;
        private IActorRef _userConnection;
        private ActorSelection _entityManager;
        private ActorSelection _gamesRoot;
        private string _username;
        private IActorRef _game;
        private Dictionary<long, Tuple<Common.Vector3, DateTime>> _entityVelocitys = new Dictionary<long, Tuple<Common.Vector3, DateTime>>();

        public IStash Stash { get; set; }

        public User(IActorRef testActor)
        {
            Debug.WriteLine("User Constructor " + Self.Path.ToString());

            Props props = Props.Create<ActorUserConnection>(new object[] { testActor }); //NOTE this should not be null :)
            Initialize(props);

            testActor.Tell(new UserCreated(Self, _userConnection));
        }

        public User(ConnectionEstablished connectionEstablishedMessage)
        {
            Debug.WriteLine("User Constructor " + Self.Path.ToString());
            //connection.Send(NetworkData.Empty);
            Props props = Props.Create<TcpUserConnection>(new object[] { connectionEstablishedMessage }); //NOTE this should not be null :)
            Initialize(props);

            _userConnection.Tell(new UserCreated(Self, _userConnection));
        }

        private void Initialize(Props props)
        {
            _userConnection = Context.ActorOf(props, "UserConnection");

            Debug.WriteLine("UserConnection Created " + _userConnection.Path.ToString());

            /// Old message from actors, not sure how to handle this yet need the client version.
            Receive<Authenticate>(msg =>
            {
                _username = msg.Name;
                Become(Authenticated);
                _userConnection.Tell(new AuthenticateResult(true));
            });

            Receive<ClientAuthenticateRequest>(msg =>
            {
                _username = msg.Username;
                Become(Authenticated);
                _userConnection.Tell(new AuthenticateResult(true));
            });

            Receive<string>(words => Sender.Tell("Not Authenticated"));
            ReceiveAny(msg =>
            {
                Stash.Stash();
                Sender.Tell(new NotAuthenticated());
            });

            Debug.WriteLine("Left Constructor " + Self.Path.ToString());
        }

        private void Authenticated()
        {
            Debug.WriteLine("User Authenticated " + Self.Path.ToString());
            var lobby = Context.ActorSelection("/user/lobby");
            _gamesRoot = Context.ActorSelection("/user/GamesRoot");

            ReceiveChatMessages(lobby);

            ReceivePreGameMessages();

            Receive<ClientAuthenticateRequest>(message => { });

            ReceiveAny(message =>
            {
                if (message is IClientMessage == false)
                    ForwardToConnections(message);
                else
                {
                    System.Console.WriteLine($"Unhandled message while in authenticated state.  Type: {message.GetType().ToString()}");
                    Stash.Stash();
                    //Self.Tell(message, Sender);
                }
            });

            Stash.UnstashAll();
            //TODO: Handle CreateGame (message not created), startgame, joingame, etc.
            // Create new high level actor to handle game creation/search/etc.
            // ex. /user/GameManager/GameSpawner/Game1234
            // GameName match room name??
        }

        private void RecieveInGameMessages()
        {
            Receive<SpawnEntity>(message =>
            {
                _entityManager.Tell(message);
            });

            Receive<ClientSpawnEntityRequest>(message =>
            {
                _entityManager.Tell(new SpawnEntity(0, message.EntityType, Vector3.zero, message.Count));
            });

            Receive<SetPosition>(message =>
            {
                _entityManager.Tell(message);
            });

            Receive<UpdatePosition>(message =>
            {
                if (ChangedVelocity(message))
                    ForwardToConnections(message);
            });

            Receive<ClientRequestEntityDetails>(message =>
            {
                _entityManager.Tell(message, Sender);
            });

            Receive<SetTargetObject>(message =>
            {
                _entityManager.Tell(message);
            });
        }

        private bool ChangedVelocity(UpdatePosition message)
        {
            if (_entityVelocitys.ContainsKey(message.EntityId) == false)
            {
                _entityVelocitys.Add(message.EntityId, new Tuple<Vector3, DateTime>(message.Velocity, DateTime.Now));
                return true;
            }

            if (_entityVelocitys[message.EntityId].Item1 != message.Velocity ||
               (DateTime.Now - _entityVelocitys[message.EntityId].Item2).TotalSeconds >= 10)
            {
                _entityVelocitys[message.EntityId] = new Tuple<Vector3, DateTime>(message.Velocity, DateTime.Now);
                return true;
            }
            return false;
        }

        private void ReceivePreGameMessages()
        {
            // Some of these messages are for in-game and some are out of game
            // Should split this into 2 behaviors
            Receive<CreateGame>(message => 
                _gamesRoot.Tell(message)
            );

            Receive<JoinGameSuccess>(message =>
            {
                _game = message.Game;
                var entityManagerPath = _game.Path + "/" + "EntityManager";
                Debug.WriteLine($"Searching for EntityManager: {entityManagerPath.ToString()}");
                _entityManager = Context.System.ActorSelection(entityManagerPath);
                ForwardToConnections(message);
            });

            Receive<StartGame>(message =>
            {
                if (_game == null)
                    Sender.Tell("Not in a game, nothing to start...");
                else
                    _game.Tell(message);
            });

            Receive<GameStarted>(message => {
                Become(RecieveInGameMessages);
                ForwardToConnections(message);
                _entityManager.Tell(new SpawnEntity(0, EntityType.Ship, Vector3.zero, 1));
            });

            // Can't do this because we use receiveAny outside this call to foward everything else to the client...
            //ReceiveAny(message => Sender.Tell($"{message.GetType().ToString()} not handled.  In PreGame State."));
        }

        private void ReceiveChatMessages(ActorSelection lobby)
        {
            Receive<string>(words => Listen(words));
            Receive<JoinLobby>(message =>
            {
                Context.ActorSelection("/user/lobby").Tell(message);
            });

            // Refactor these message types that need to put themselves in..
            Receive<LeaveRoom>(message =>
            {
                var updatedMessage = new LeaveRoom(Self, message.RoomName);
                lobby.Tell(updatedMessage);
            });
            Receive<JoinRoom>(message =>
            {
                var updatedMessage = new JoinRoom(Self, message.RoomName);
                lobby.Tell(updatedMessage);
            });
            // Refactor these message types that need to put themselves in..


            Receive<GetRooms>(message =>
            {
                lobby.Tell(message);
            });

            Receive<JoinRoomSuccess>(message =>
            {
                _room = message.RoomActor;
                Stash.UnstashAll();
                _userConnection.Tell(message);
            });

            Receive<Chat>(message =>
            {
                if (_room == null)
                    Stash.Stash();
                else if (Sender == _room)
                    ForwardToConnections(message);
                else
                    _room.Tell(new Chat(message.ChatActor, message.Text, _username));
            });
        }

        private void ForwardToConnections(object message)
        {
            _userConnection.Tell(message);
        }

        private void Listen(string words)
        {
            Debug.WriteLine("Got Text: " + words);
        }
    }
}
