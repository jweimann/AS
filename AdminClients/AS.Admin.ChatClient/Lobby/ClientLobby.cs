using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient
{
    public class ClientLobby : ReceiveActor
    {
        private Action<RoomList> OnRoomListReceived { get; set; }
        public ClientLobby(Action<RoomList> onUserListReceived)
        {
            this.OnRoomListReceived = onUserListReceived;

            Receive<RoomList>(msg =>
                {
                    if (this.OnRoomListReceived != null)
                        this.OnRoomListReceived(msg);
                });
        }
    }
}
