using Akka.Actor;

namespace AS.Messages.Game
{
    public class JoinGameSuccess
    {
        public IActorRef Game { get; private set; }
        public string GameName { get; private set; }

        public JoinGameSuccess(IActorRef game, string gameName)
        {
            Game = game;
            GameName = gameName;
        }
    }
}
