using Akka.Actor;

namespace AS.Messages.Entities
{
    public class JoinRegion
    {
        public IActorRef Region { get; private set; }

        public JoinRegion(IActorRef region)
        {
            Region = region;
        }
    }
}
