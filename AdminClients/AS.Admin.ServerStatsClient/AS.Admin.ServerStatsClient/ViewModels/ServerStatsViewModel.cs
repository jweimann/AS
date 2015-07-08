using AS.Client.Core.WPF;

namespace AS.Admin.ServerStatsClient.ViewModels
{
    public class ServerStatsViewModel : ViewModelBase
    {
        private int _roomCount;
        private int _gameCount;

        public int GameCount { get { return _gameCount; } set { _gameCount = value; OnPropertyChanged(() => GameCount); } }

        public int RoomCount { get { return _roomCount; } set { _roomCount = value; OnPropertyChanged(() => RoomCount); } }
    }
}
