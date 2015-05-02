using Akka.Actor;
using AS.Actors;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.MockActors
{
    public class MockClientConnectionManager : ReceiveActor
    {
        private ActorSelection _testKitActor;
        public MockClientConnectionManager()
        {
            _testKitActor = Context.System.ActorSelection("/user/testActor1");
            ActorSelection users = Context.System.ActorSelection("/user/users");
            Receive<ConnectionEstablished>(message =>
                {
                    System.Diagnostics.Debug.WriteLine("ConnectionEstablished");
                    users.Tell(message);
                });

            Receive<UserCreated>(message => ForwardMessageToTestKit(message));
        }

        private void ForwardMessageToTestKit(UserCreated message)
        {
            Debug.WriteLine("Forwarding Message to TestKit");
            _testKitActor.Tell(message);
        }
    }
}
