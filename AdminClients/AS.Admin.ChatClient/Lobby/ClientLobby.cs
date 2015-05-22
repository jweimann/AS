using Akka.Actor;
using AS.Messages;
using System;

namespace AS.Admin.ChatClient
{
    public class ClientLobby : ReceiveActor
    {
        private Action<RoomList> OnRoomListReceived { get; set; }
        public ClientLobby(Action<RoomList> onUserListReceived)
        {
            OnRoomListReceived = onUserListReceived;

            Receive<RoomList>(msg =>
            {
                if (OnRoomListReceived != null)
                    OnRoomListReceived(msg);
            });
        }
    }
}
