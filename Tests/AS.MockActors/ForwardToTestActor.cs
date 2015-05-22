using Akka.Actor;

namespace AS.MockActors
{
    public class ForwardToTestActor : UntypedActor
    {
        private IActorRef _testActor;

        public ForwardToTestActor(IActorRef testActor)
        {
            _testActor = testActor;
        }
        protected override void OnReceive(object message)
        {
            _testActor.Tell(message);
        }
    }
}
