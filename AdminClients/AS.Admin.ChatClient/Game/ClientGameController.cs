using Akka.Actor;
using AS.Admin.ChatClient.Game;
using AS.Admin.ChatClient.ViewModels;
using AS.Common;
using AS.Messages.Entities;
using AS.Client.Messages.Game;

namespace AS.Admin.ChatClient.Lobby
{
    public class ClientGameController : ClientControllerBase
    {
        private GameViewModel _viewModel;
        public IActorRef ClientGame { get; private set; }

        public ClientGameController(IActorContext context) : base(context.System)
        {
            _viewModel = new GameViewModel();
            _viewModel.OnStartGame += HandleOnStartGame;
            _viewModel.OnSpawnTestEntity += HandleOnSpawnTestEntity;

            ClientGame = context.ActorOf(
               Props.Create<ClientGame>(new object[] { _viewModel })
                .WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
                ,"ClientGame");
        }

        private void HandleOnSpawnTestEntity()
        {
            _myUserConnection.Tell(new SpawnEntity(1, EntityType.Asteroid, Vector3.one, 1));
            //_myUserConnection.Tell(new SetPosition(Vector3.one, 1));
        }

        private void HandleOnStartGame()
        {
            _myUserConnection.Tell(new StartGame());
        }
    }
}
