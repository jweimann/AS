using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors.Lobby
{
    public class Lobby : ReceiveActor
    {
        private const string DEFAULT_CHAT_ROOM = "OpenChat";

        private List<string> _rooms;

        public Lobby()
        {
            _rooms = new List<string>();
            Receive<JoinRoom>(msg => AddPlayerToRoom(msg));
            Receive<LeaveRoom>(msg => RemovePlayerFromRoom(msg));
            Receive<GetRooms>(msg => 
                Sender.Tell(new RoomList(_rooms))
                );


            var room = Context.ActorOf<Room>(DEFAULT_CHAT_ROOM);
            _rooms.Add(DEFAULT_CHAT_ROOM);
        }

        private void RemovePlayerFromRoom(LeaveRoom msg)
        {
            if (msg.RoomName != null)
            {
                IActorRef room = Context.Child(msg.RoomName);
                room.Tell(msg);
            }
        }

        private void AddPlayerToRoom(JoinRoom msg)
        {
            IActorRef room = Context.Child(msg.RoomName);

            if (room.IsNobody())
            {
                room = Context.ActorOf<Room>(msg.RoomName);
                _rooms.Add(msg.RoomName);
                Task.Delay(100);
            }
            room.Tell(msg);
        }
    }
}
 