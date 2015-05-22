using Akka.Actor;
using AS.Messages;
using System.Diagnostics;
using AS.Messages.Game;
using AS.Messages.User;
using AS.Messages.Entities;

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

        public IStash Stash { get; set; }

        //public User(IActorRef clientConnectionManager)
        public User(IActorRef testActor)
        {
            Debug.WriteLine("User Constructor " + Self.Path.ToString());
            
            Props props = Props.Create<UserConnection>(new object[] { testActor }); //NOTE this should not be null :)
            _userConnection = Context.ActorOf(props, "UserConnection");
            
            Debug.WriteLine("UserConnection Created " + _userConnection.Path.ToString());

            Receive<Authenticate>(msg =>
                {
                    _username = msg.Name;
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

            

            testActor.Tell(new UserCreated(Self, _userConnection));
            //Debug.WriteLine("Sent user to " + clientConnectionManager.Path.ToString());
        }

        private void Authenticated()
        {
            Debug.WriteLine("User Authenticated " + Self.Path.ToString());
            var lobby = Context.ActorSelection("/user/lobby");
            _gamesRoot = Context.ActorSelection("/user/GamesRoot");

            ReceiveChatMessages(lobby);

            ReceiveGameMessages();

            ReceiveAny(message =>
            {
                ForwardToConnections(message);
            });

            //TODO: Handle CreateGame (message not created), startgame, joingame, etc.
            // Create new high level actor to handle game creation/search/etc.
            // ex. /user/GameManager/GameSpawner/Game1234
            // GameName match room name??
        }

        private void ReceiveGameMessages()
        {
            Receive<CreateGame>(message => _gamesRoot.Tell(message));
            Receive<SpawnEntity>(message =>
            {
                _entityManager.Tell(message);
            });
            Receive<JoinGameSuccess>(message =>
            {
                _game = message.Game;
                var entityManagerPath = _game.Path + "/" + "EntityManager";
                Debug.WriteLine($"Searching for EntityManager: {entityManagerPath.ToString()}");
                _entityManager = Context.System.ActorSelection(entityManagerPath);
                ForwardToConnections(message);
            });
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
