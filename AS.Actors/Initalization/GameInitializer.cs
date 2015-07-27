using Akka.Actor;
using AS.Messages.Game;

namespace AS.Actors.Initalization
{
    public class GameInitializer : ReceiveActor
    {
        private IActorRef _gameActor;
        public GameInitializer(IActorRef gameActor)
        {
            _gameActor = gameActor;
            Initialize();
            SendInitalizationMessage(GameInitializationComplete.Instance);
        }

        protected virtual void Initialize()
        {

        }

        protected void SendInitalizationMessage(object message)
        {
            _gameActor.Tell(message);
        }
    }
}
