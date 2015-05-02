using Akka.Actor;
using AS.Messages;
using AS.MockActors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AS.Actors.Tests
{
    public class MockRemoteUser : ReceiveActor
    {
        private IActorRef _userConnection;
        private IActorRef _testActor;
        public MockRemoteUser(IActorRef testActor)
        {
            Receive<UserCreated>(userConnection =>
                {
                    _userConnection = userConnection.UserConnectionActor;
                    _testActor.Tell(userConnection);
                });
        }

    
    }
}
