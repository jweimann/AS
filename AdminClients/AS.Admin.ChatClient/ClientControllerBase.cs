using Akka.Actor;

namespace AS.Admin.ChatClient
{
    public abstract class ClientControllerBase
    {
        protected IActorRef _myUserConnection;
        protected ActorSystem Sys { get; private set; }

        public ClientControllerBase(ActorSystem sys)
        {
            Sys = sys;
        }
    }
}
