using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class JoinRoom
    {
        public JoinRoom(IActorRef actorRef, string roomName)
        {
            this.ActorRef = actorRef;
            this.RoomName = roomName;
        }
        public IActorRef ActorRef { get; private set; }

        public string RoomName { get; private set; }
    }
}
