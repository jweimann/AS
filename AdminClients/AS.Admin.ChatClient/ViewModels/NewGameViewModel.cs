using System.Windows.Input;

namespace AS.Admin.ChatClient.ViewModels
{
    public class NewGameViewModel : ViewModelBase
    {
        private string _name;

        public string Name {  get { return _name; }  set { _name = value; OnPropertyChanged(() => Name); }  }

        public ICommand CreateGameCommand { get; set; }
    }
}
