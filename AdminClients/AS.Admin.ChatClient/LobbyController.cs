using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient
{
    public class LobbyController
    {
        private LobbyViewModel _vm;
        private ActorSelection _lobby; 
        private Action<RoomList> OnRoomListReceived;
        public LobbyController(ActorSystem system, ref LobbyViewModel vm)
        {
             _vm = vm;
            this.Sys = system;
            //_lobby = Context.ActorSelection("/user/lobby");

            _clientRoom = Sys.ActorOf(
                Props.Create(() => new ClientLobby(OnRoomListReceived))
                 .WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
                ,"lobbydispatcher");
        }

    


        private ActorSelection _connectionManager;
        private IActorRef _clientRoom;
        private RoomViewModel _roomViewModel;
        public ActorSystem Sys { get; set; }
    
    }
}
