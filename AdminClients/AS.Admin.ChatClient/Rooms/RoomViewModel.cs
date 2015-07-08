using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AS.Common;
using AS.Client.Core.WPF;

namespace AS.Admin.ChatClient
{
    public class RoomViewModel : ViewModelBase
    {
        public RoomViewModel()
        {
            Users = new ObservableCollection<string>();
            ChatCommand = new RelayCommand(SendChat);
        }

        private void SendChat(object text)
        {
            OutgoingChatText = String.Empty;
            if (OnSendChat != null)
                OnSendChat(text.ToString());
        }

        private ObservableCollection<string> _users;
        public ObservableCollection<string> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(() => Users);
            }
        }

        private string _chatText;
        public string ChatText
        {
            get { return _chatText; }
            set
            {
                _chatText = value;
                OnPropertyChanged(() => ChatText);
            }
        }

        private string _outgoingChatText;
        public string OutgoingChatText { get { return _outgoingChatText; } set { _outgoingChatText = value; OnPropertyChanged(() => OutgoingChatText); } }

        public ICommand ChatCommand { get; set; }
        public Action<string> OnSendChat { get; set; }
    }
}
