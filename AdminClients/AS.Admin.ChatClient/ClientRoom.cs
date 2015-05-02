using Akka.Actor;
using AS.Actors.ClientConnection;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient
{
    public class ClientRoom : ReceiveActor // actually a user in chat.. should rename
    {
        private ActorSelection _mockClientConnectionManager;
        private IActorRef _myUserConnection;
        private Action<UserList> _onUserListReceived;
        private Action<UserJoinedRoom> _onUserJoined;
        private Action<Chat> _onChat;

        public ClientRoom(Action<UserList> onUserListReceived, Action<UserJoinedRoom> onUserJoined, Action<Chat> onChat)
        {
            _onChat = onChat;
            _onUserListReceived = onUserListReceived;
            _onUserJoined = onUserJoined;
            _mockClientConnectionManager = Context.System.ActorSelection("akka.tcp://as@localhost:8081/user/ConnectionManager");
            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(Self)));

            Receive<UserCreated>(message =>
                {
                    _myUserConnection = message.UserConnectionActor;
                    Console.WriteLine(message);
                    Become(Connected);
                });
        }

        private void Connected()
        {
            _myUserConnection.Tell(new Authenticate("jason"));
            Receive<AuthenticateResult>(message =>
                {
                    Become(Authenticated);
                    _myUserConnection.Tell(new GetRooms());
                });
        }

        private void Authenticated()
        {
            Receive<string>(message =>
            {
                if (_myUserConnection != null)
                    _myUserConnection.Tell(new JoinRoom(Self, "Chat1"));
                Console.Write(message);
            });

            Receive<UserList>(message =>
            {
                if (_onUserListReceived != null)
                    _onUserListReceived(message);
                Console.WriteLine(message);
            });

            Receive<UserJoinedRoom>(message =>
            {
                if (_onUserJoined != null)
                    _onUserJoined(message);
                Console.WriteLine(message);
            });

            Receive<Chat>(message =>
                {
                    if (Sender.Path.Equals(_myUserConnection.Path))
                    {
                        if (_onChat != null)
                            _onChat(message);
                    }
                    else
                    {
                        _myUserConnection.Tell(message);
                    }
                });
        }
        public ClientRoom(IActorRef connectionManager)
        {
            connectionManager.Tell(new ConnectionEstablished(new MockActorConnection(Self)));


            _mockClientConnectionManager = Context.System.ActorSelection("/user/lobby");

            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(Self)));

            Receive<string>(message =>
                Console.Write(message)
                );
        }
    }
}
