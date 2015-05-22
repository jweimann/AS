using Akka.Actor;
using AS.Messages;

namespace AS.Admin.ChatClient.Authentication
{
    public class AuthenticationController : ClientControllerBase
    {
        public IActorRef AuthenticationActor { get; private set; }

        public AuthenticationController(IUntypedActorContext context) : base(context.System)
        {
            AuthenticationActor = context.ActorOf(
               Props.Create(() => new AuthenticationActor(OnUserCreated))
                .WithDispatcher("akka.actor.synchronized-dispatcher")
               , "AuthenticationController");
        }

        private void OnUserCreated(UserCreated obj)
        {

        }

        
    }
}
