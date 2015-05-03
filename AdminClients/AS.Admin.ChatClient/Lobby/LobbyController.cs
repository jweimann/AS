using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AS.Admin.ChatClient
{
    public class LobbyController : ClientControllerBase
    {
        private LobbyViewModel _lobbyViewModel;
        private Action<RoomList> OnRoomListReceived;

        public IActorRef ClientLobby { get; private set; }

        public LobbyController(ActorSystem system, ref LobbyViewModel lobbyViewModel) : base(system)
        {
             _lobbyViewModel = lobbyViewModel;
            _lobbyViewModel.OnRefreshRoomsClicked += () => _myUserConnection.Tell(new GetRooms());

            OnRoomListReceived += obj => _lobbyViewModel.RoomNames = new ObservableCollection<string>(obj.Rooms);

            this.ClientLobby = Sys.ActorOf(
                Props.Create(() => new ClientLobby(OnRoomListReceived))
                 .WithDispatcher("akka.actor.synchronized-dispatcher")
                ,"lobbydispatcher");
        }
    }
}
