using Akka.Actor;
using AS.Interfaces;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors
{
    public class User : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef _lobbyUser;
        private IActorRef _room;
        private IActorRef _userConnection;
        private string _username;
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
            Receive<object>(msg => 
                {
                    Stash.Stash();
                });
            Debug.WriteLine("Left Constructor " + Self.Path.ToString());

            
            testActor.Tell(new UserCreated(Self, _userConnection));
            //Debug.WriteLine("Sent user to " + clientConnectionManager.Path.ToString());
        }

        private void Authenticated()
        {
            Debug.WriteLine("User Authenticated " + Self.Path.ToString());
            var lobby = Context.ActorSelection("/user/lobby");

            Receive<string>(words => Listen(words));
            Receive<JoinLobby>(message =>
                {
                    Context.ActorSelection("/user/lobby").Tell(message);
                });
            Receive<JoinRoom>(message =>
                {
                    var updatedMessage = new JoinRoom(Self, message.RoomName);
                    lobby.Tell(updatedMessage);
                });
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
            
            ReceiveAny(message =>
                {
                    ForwardToConnections(message);
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
