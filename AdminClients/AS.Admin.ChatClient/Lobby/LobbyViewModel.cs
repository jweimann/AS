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
        private string _newRoomName;
        private string _selectedRoomName;

        public ObservableCollection<string> RoomNames { get { return _roomNames; } set { _roomNames = value; OnPropertyChanged(() => RoomNames); } }
        public string NewRoomName {  get { return _newRoomName; } set { _newRoomName = value; OnPropertyChanged(() => NewRoomName); } }

        public string SelectedRoomName {  get { return _selectedRoomName; } set { _selectedRoomName = value; OnPropertyChanged(() => SelectedRoomName); } }

        public Action OnRefreshRoomsClicked { get; set; }
        public Action<string> OnCreateRoomClicked { get; set; }
        public Action<string> OnJoinRoomClicked { get; set; }
        public ICommand RefreshRoomsCommand { get; set; }
        public ICommand CreateRoomCommand { get; set; }
        public ICommand JoinRoomCommand { get; set; }
        public ICommand CreateGameCommand { get; set; }

        public LobbyViewModel()
        {
            RefreshRoomsCommand = new RelayCommand(RefreshRooms);
            CreateRoomCommand = new RelayCommand(CreateRoom);
            JoinRoomCommand = new RelayCommand(JoinRoom);
            
            NewRoomName = "NewRoom";
        }

        private void CreateGame(object obj)
        {
            throw new NotImplementedException();
        }

        private void JoinRoom(object obj)
        {
            if (OnJoinRoomClicked != null)
                OnJoinRoomClicked(SelectedRoomName);
        }

        private void CreateRoom(object obj)
        {
            if (OnCreateRoomClicked != null)
                OnCreateRoomClicked(NewRoomName);
        }

        private void RefreshRooms(object obj)
        {
            if (OnRefreshRoomsClicked != null)
                OnRefreshRoomsClicked();
        }

    }
}
