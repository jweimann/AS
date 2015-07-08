using AS.Client.Core.WPF;

namespace AS.Admin.ChatClient.Authentication
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private string _username;
        public string Username {  get { return _username; } set { _username = value; OnPropertyChanged(() => Username); } }
    }
}
