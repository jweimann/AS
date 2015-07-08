using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AS.Common;
using AS.Messages.Game;
using AS.Admin.ChatClient.Views;
using AS.Admin.ChatClient.ViewModels;
using System.Windows;

namespace AS.Admin.ChatClient
{
    public class LobbyController : ClientControllerBase
    {
        private LobbyViewModel _lobbyViewModel;
        private Action<RoomList> OnRoomListReceived;
        private string _currentRoomName;

        public IActorRef ClientLobby { get; private set; }

        public LobbyController(IUntypedActorContext context, ref LobbyViewModel lobbyViewModel) : base(context.System)
        {
            _lobbyViewModel = lobbyViewModel;
            _lobbyViewModel.OnRefreshRoomsClicked += () =>
                _myUserConnection.Tell(new GetRooms());

            _lobbyViewModel.OnCreateRoomClicked += JoinNewRoomAndLeaveCurrent;
            _lobbyViewModel.OnJoinRoomClicked += JoinNewRoomAndLeaveCurrent;
            _lobbyViewModel.CreateGameCommand = new RelayCommand(CreateGame);

            OnRoomListReceived += obj => _lobbyViewModel.RoomNames = new ObservableCollection<string>(obj.Rooms);

            ClientLobby = context.ActorOf(
              Props.Create(() => new ClientLobby(OnRoomListReceived))
               .WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
              , "ClientLobby");
        }

        private void CreateGame(object obj)
        {
            NewGameViewModel vm = new NewGameViewModel();
            vm.CreateGameCommand = new RelayCommand(SendCreateGameCommand);
            NewGameNameDialog dialog = new NewGameNameDialog();
            dialog.DataContext = vm;
            
            dialog.ShowDialog();

            //_myUserConnection.Tell(new JoinGame(_myUserConnection));
            //throw new NotImplementedException();
        }

        private void SendCreateGameCommand(object obj)
        {
            ((Window)obj).Close();
            NewGameViewModel vm = ((Window)obj).DataContext as NewGameViewModel;
            _myUserConnection.Tell(new CreateGame(vm.Name));
            
            //_myUserConnection.Tell(new CreateGame("ignoredTheUI"));
        }

        private void JoinNewRoomAndLeaveCurrent(string roomName)
        {
            _myUserConnection.Tell(new LeaveRoom(ClientLobby, _currentRoomName));
            _myUserConnection.Tell(new JoinRoom(ClientLobby, roomName));
            _currentRoomName = roomName;
        }

    }
}
