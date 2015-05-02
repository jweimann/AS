using Akka.Actor;
using AS.Actors.ClientConnection;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors
{
    public class Users : ReceiveActor
    {
        public Users()
        {
            Receive<ConnectionEstablished>(message => HandleNewConnection(message));
        }

        private void HandleNewConnection(ConnectionEstablished message)
        {
            Debug.WriteLine("HandleNewConnection");
            
            MockActorConnection mockActorConnection = message.Connection as MockActorConnection;

            Debug.WriteLine("Sender/MockConnection: " + mockActorConnection.TestActor.Path.ToString());

            Props props = Props.Create<User>(mockActorConnection.TestActor);
            //Props props = Props.Create<User>(Sender);
            var user = Context.System.ActorOf(props);

            Task.Delay(1000);
            
            Debug.WriteLine("User Created " + user.Path.ToString());

            // move this out of here or to an ask?

        }
    }
}