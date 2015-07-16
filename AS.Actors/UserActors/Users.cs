using Akka.Actor;
using AS.Actors.ClientConnection;
using AS.Messages;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AS.Actors.UserActors
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
            
            MockActorConnection mockActorConnection = message.MockConnection as MockActorConnection;

            if (mockActorConnection != null)
            {
                Debug.WriteLine("Sender/MockConnection: " + mockActorConnection.TestActor.Path.ToString());

                Props props = Props.Create<User>(mockActorConnection.TestActor);
                var user = Context.System.ActorOf(props);
                Debug.WriteLine("User Created " + user.Path.ToString());
            }
            else
            {
                //Debug.WriteLine("Sender/MockConnection: " + mockActorConnection.TestActor.Path.ToString());
                Props props = Props.Create<User>(message);
                var user = Context.System.ActorOf(props);
                Debug.WriteLine("User Created " + user.Path.ToString());
            }

            Task.Delay(1000);
            
            

            // move this out of here or to an ask?

        }
    }
}