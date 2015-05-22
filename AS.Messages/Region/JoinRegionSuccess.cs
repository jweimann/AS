using Akka.Actor;

namespace AS.Messages.Region
{
    public class JoinRegionSuccess
    {
        public IActorRef RegionActor { get; private set; }
        public JoinRegionSuccess(IActorRef regionActor)
        {
            RegionActor = regionActor;
        }
    }
}
