using Akka.Actor;
using AS.Admin.ChatClient.ViewModels;
using AS.Admin.ChatClient.Views;
using AS.Messages.Game;
using AS.Messages.Region;
using AS.Messages.Entities;
using AS.Client.Core;
using System.Linq;
using AS.Client.Core.WPF;
using AS.Client.Messages.Game;
using AS.Client.Messages.Entities;

namespace AS.Admin.ChatClient.Game
{
    public class ClientGame : ReceiveActor
    {
        private GameViewModel _gameViewModel;
        public ClientGame(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;

            Receive<JoinGameSuccess>(message =>
            {
                GameWindow gameWindow = new GameWindow();
                gameWindow.DataContext = _gameViewModel;
                _gameViewModel.GameName = message.GameName;
                gameWindow.Show();
            });

            Receive<GameStarted>(message =>
            {
                _gameViewModel.GameState = "Started";
            });

            //TODO: Not sure what to handle here, need to deal with subscribing to regions and updating the client properly.
            Receive<EntitiesInRegionList>(message =>
            {
                foreach (var entity in message.Entities)
                {
                    //if (_gameViewModel.Entities.Contains(entity) == false)
                    //    _gameViewModel.Entities.Add(entity);
                }
            });

            Receive<UpdatePosition>(message =>
            {
                return;
                var entity = GetOrAddEntity(message.EntityId);
                entity.Position = message.Position;
            });
        }


        private WPFClientEntity GetOrAddEntity(long entityId)
        {
            var existing = _gameViewModel.Entities.FirstOrDefault(t => t.EntityId == entityId);
            if (existing != null)
                return existing;

            WPFClientEntity newEntity = new WPFClientEntity(entityId);
            _gameViewModel.Entities.Add(newEntity);
            return newEntity;
        }
    }
}
