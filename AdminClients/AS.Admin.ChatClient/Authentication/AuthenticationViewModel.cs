using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient.Authentication
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private string _username;
        public string Username {  get { return _username; } set { _username = value; OnPropertyChanged(() => Username); } }
    }
}
