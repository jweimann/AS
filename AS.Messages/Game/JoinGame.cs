using Akka.Actor;

namespace AS.Messages.Game
{
    public class JoinGame
    {
        public IActorRef ActorRef { get; private set; }
        public JoinGame(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }
    }
}
