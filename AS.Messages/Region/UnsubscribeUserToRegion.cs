using Akka.Actor;

namespace AS.Messages.Region
{
    public class UnsubscribeUserToRegion
    {
        public UnsubscribeUserToRegion(IActorRef userActor)
        {
            UserActor = userActor;
        }

        public IActorRef UserActor { get; private set; }
    }
}
