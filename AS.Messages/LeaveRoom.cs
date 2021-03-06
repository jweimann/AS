﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class LeaveRoom
    {
        public IActorRef ActorRef { get; private set; }
        public string RoomName { get; private set; }
        public LeaveRoom(IActorRef actorRef, string roomName)
        {
            ActorRef = actorRef;
            RoomName = roomName;
        }
    }
}
