using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using AS.Messages;

namespace AS.Admin.ChatClient.Authentication
{
    public class AuthenticationController : ClientControllerBase
    {
        public AuthenticationController(ActorSystem sys) : base(sys)
        {
            this.AuthenticationActor = Sys.ActorOf(
               Props.Create(() => new AuthenticationActor(OnUserCreated))
                .WithDispatcher("akka.actor.synchronized-dispatcher")
               , "lobbydispatcher");
        }

        private void OnUserCreated(UserCreated obj)
        {
            
        }

        public IActorRef AuthenticationActor { get; private set; }
    }
}
