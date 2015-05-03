using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AS.Common;

namespace AS.Admin.ChatClient
{
    public class LobbyViewModel : ViewModelBase
    {
        private ObservableCollection<string> _roomNames;
        public ObservableCollection<string> RoomNames { get { return _roomNames; } set { _roomNames = value; OnPropertyChanged(() => RoomNames); } }

        public Action OnRefreshRoomsClicked { get; set; }
        public ICommand RefreshRoomsCommand { get; set; }

        public LobbyViewModel()
        {
            RefreshRoomsCommand = new RelayCommand(RefreshRooms);
        }

        private void RefreshRooms(object obj)
        {
            if (OnRefreshRoomsClicked != null)
                OnRefreshRoomsClicked();
        }

    }
}
