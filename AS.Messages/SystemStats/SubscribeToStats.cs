using Akka.Actor;

namespace AS.Messages.SystemStats
{
    public class SubscribeToStats
    {
        public SubscribeToStats(IActorRef subscriber)
        {
            Subscriber = subscriber;
        }

        public IActorRef Subscriber { get; private set; }
    }
}
