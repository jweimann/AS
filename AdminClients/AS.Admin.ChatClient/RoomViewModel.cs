using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient
{
    public class RoomViewModel : ViewModelBase
    {
        public RoomViewModel()
        {
            Users = new ObservableCollection<string>();
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
    }
}
