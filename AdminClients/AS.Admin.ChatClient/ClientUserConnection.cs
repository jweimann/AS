﻿using Akka.Actor;
using AS.Messages;
using AS.Actors.ClientConnection;
using System;
using System.Windows;
using AS.Admin.ChatClient.Authentication;
using AS.Messages.SystemStats;
using AS.Admin.ChatClient.Lobby;

namespace AS.Admin.ChatClient
{
    public class ClientUserConnection : UntypedActor
    {
        private IActorRef _clientLobby;
        private IActorRef _clientRoom;
        private ActorSelection _mockClientConnectionManager;
        private RoomController _roomController;
        private LobbyController _lobbyController;
        private ClientGameController _gameController;
        private AuthenticationController _authenticationController;
        private MessageLoggerViewModel _messageLoggerViewModel;
        private ActorSelection _statsGatherer;

        public Action<RoomList> OnRoomListReceived { get; private set; }

        public ClientUserConnection(LobbyViewModel lobbyViewModel, RoomViewModel roomViewModel, MessageLoggerViewModel messageLoggerViewModel)
        {
            _messageLoggerViewModel = messageLoggerViewModel;

            _authenticationController = new AuthenticationController(Context);
            _lobbyController = new LobbyController(Context, ref lobbyViewModel);
            _roomController = new RoomController(Context, ref roomViewModel);
            _gameController = new ClientGameController(Context);

            OnRoomListReceived += roomList => MessageBox.Show("RoomList");
            //_clientLobby = clientLobbyController.ClientLobby;
            //_clientRoom = clientRoom;

            ConnectToServer();
        }

        private void ConnectToServer()
        {
            _mockClientConnectionManager = Context.System.ActorSelection("akka.tcp://as@localhost:8081/user/ConnectionManager");
            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(Self)));

            _statsGatherer = Context.System.ActorSelection("akka.tcp://as@localhost:8081/user/StatsGatherer");
            _statsGatherer.Tell(new SubscribeToStats(Self));
        }


        protected override void OnReceive(object message)
        {
            LogMessage(message);
            if (message is UserCreated)
            {
                _lobbyController.SetConnection(((UserCreated)message).UserConnectionActor);
                _roomController.SetConnection(((UserCreated)message).UserConnectionActor);
                _gameController.SetConnection(((UserCreated)message).UserConnectionActor);
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
