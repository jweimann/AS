using Akka.Actor;

namespace AS.Messages.Game
{
    public class JoinGameSuccess
    {
        public IActorRef Game { get; private set; }

        public JoinGameSuccess(IActorRef game)
        {
            Game = game;
        }
    }
}
