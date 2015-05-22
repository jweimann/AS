using Akka.Actor;
using AS.Messages;
using AS.Actors.ClientConnection;
using System;
using System.Windows;
using AS.Admin.ChatClient.Authentication;

namespace AS.Admin.ChatClient
{
    public class ClientUserConnection : UntypedActor
    {
        private IActorRef _clientLobby;
        private IActorRef _clientRoom;
        private ActorSelection _mockClientConnectionManager;
        private RoomController _roomController;
        private LobbyController _lobbyController;
        private AuthenticationController _authenticationController;
        private MessageLoggerViewModel _messageLoggerViewModel;

        public Action<RoomList> OnRoomListReceived { get; private set; }

        public ClientUserConnection(LobbyViewModel lobbyViewModel, RoomViewModel roomViewModel, MessageLoggerViewModel messageLoggerViewModel)
        {
            _messageLoggerViewModel = messageLoggerViewModel;

            _authenticationController = new AuthenticationController(Context);
            _lobbyController = new LobbyController(Context, ref lobbyViewModel);
            _roomController = new RoomController(Context, ref roomViewModel);

            OnRoomListReceived += roomList => MessageBox.Show("RoomList");
            //_clientLobby = clientLobbyController.ClientLobby;
            //_clientRoom = clientRoom;

            ConnectToServer();
        }

        private void ConnectToServer()
        {
            _mockClientConnectionManager = Context.System.ActorSelection("akka.tcp://as@localhost:8081/user/ConnectionManager");
            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(Self)));
        }


        protected override void OnReceive(object message)
        {
            LogMessage(message);
            if (message is UserCreated)
            {
                _lobbyController.SetConnection(((UserCreated)message).UserConnectionActor);
                _roomController.SetConnection(((UserCreated)message).UserConnectionActor);
            }
            foreach (var child in Context.GetChildren())
                child.Tell(message);
            //_clientLobby.Tell(message);
            //_clientRoom.Tell(message);
        }

        private void LogMessage(object message)
        {
            _messageLoggerViewModel.LogText += $"Recieved Message {message.GetType()} from {Sender.Path}\n";
        }
    }
}
