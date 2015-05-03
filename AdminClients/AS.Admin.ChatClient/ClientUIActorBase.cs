using Akka.Actor;
using AS.Messages;

namespace AS.Admin.ChatClient
{
    public class ClientUIActorBase : ReceiveActor
    {
        protected IActorRef _myUserConnection;

        public ClientUIActorBase()
        {
            Receive<UserCreated>(message =>
            {
                _myUserConnection = message.UserConnectionActor;
            });
        }
    }
}
