using Akka.Actor;
using AS.Client.Core;
using AS.Client.Core.WPF;
using AS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AS.Admin.ChatClient.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private string _gameState;
        private ObservableCollection<WPFClientEntity> _entities;
        private ObservableCollection<IActorRef> _regions;

        public string GameName { get; set; }
        public string GameState { get { return _gameState; } set { _gameState = value; OnPropertyChanged(() => GameState); } }
        public ObservableCollection<WPFClientEntity> Entities { get { return _entities; } set { _entities = value; OnPropertyChanged(() => Entities); } }
        public ObservableCollection<IActorRef> Regions { get { return _regions; } set { _regions = value; OnPropertyChanged(() => Regions); } }

        public ICommand StartGameCommand { get; set; }
        public ICommand SpawnTestEntityCommand { get; set; }
        public Action OnStartGame { get; set; }
        public Action OnSpawnTestEntity { get; set; }


        public GameViewModel()
        {
            StartGameCommand = new RelayCommand(SendStartGame);
            SpawnTestEntityCommand = new RelayCommand(SendSpawnTestEntity);
            Regions = new ObservableCollection<IActorRef>();
            Entities = new ObservableCollection<WPFClientEntity>();
        }

        private void SendSpawnTestEntity(object obj)
        {
            if (OnSpawnTestEntity != null)
                OnSpawnTestEntity();
        }

        private void SendStartGame(object obj)
        {
            if (OnStartGame != null)
                OnStartGame();
        }
    }
}
