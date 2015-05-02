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
        private Action<RoomList> OnUserListReceived { get; set; }
        public ClientLobby (Action<RoomList> onUserListReceived)
        {
            this.OnUserListReceived = onUserListReceived;

            Receive<RoomList>(msg =>
                {
                    if (this.OnUserListReceived != null)
                        this.OnUserListReceived(msg);
                });
        }
    }
}
