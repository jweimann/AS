using Akka.Actor;
using AS.Messages;
using AS.Actors.ClientConnection;

namespace AS.Admin.ChatClient
{
    public class ClientUserConnection : UntypedActor
    {
        private IActorRef _clientLobby;
        private IActorRef _clientRoom;
        private ActorSelection _mockClientConnectionManager;

        public ClientUserConnection(LobbyController clientLobbyController, IActorRef clientRoom)
        {
            _clientLobby = clientLobbyController.ClientLobby;
            _clientRoom = clientRoom;

            _mockClientConnectionManager = Context.System.ActorSelection("akka.tcp://as@localhost:8081/user/ConnectionManager");
            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(Self)));
        }

        protected override void OnReceive(object message)
        {
            _clientLobby.Tell(message);
            _clientRoom.Tell(message);
        }
    }
}
