﻿using Akka.Actor;
using AS.Interfaces;
namespace AS.Actors.ClientConnection
{
    public class MockActorConnection : IConnection
    {
        public IActorRef TestActor { get; set; }
        public MockActorConnection(IActorRef testActor)
        {
            this.TestActor = testActor;
        }
    }
}