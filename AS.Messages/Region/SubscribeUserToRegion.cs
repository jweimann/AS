using Akka.Actor;

namespace AS.Messages.Region
{
    public class SubscribeUserToRegion
    {
        public SubscribeUserToRegion(IActorRef userActor)
        {
            UserActor = userActor;
        }

        public IActorRef UserActor { get; private set; }
    }
}
