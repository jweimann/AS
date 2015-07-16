using Akka.Actor;
using AS.Client.Messages.Game;
using AS.Interfaces;

namespace AS.Messages.Game
{
    public class JoinGameSuccess : IMapToClientCommand
    {
        public IActorRef Game { get; private set; }
        public string GameName { get; private set; }

        public JoinGameSuccess(IActorRef game, string gameName)
        {
            Game = game;
            GameName = gameName;
        }
        public object GetClientCommand(int entityId)
        {
            return new ClientJoinGameSuccessResponse(GameName);
        }
    }
}
