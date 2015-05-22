namespace AS.Admin.ChatClient
{
    public class MessageLoggerViewModel : ViewModelBase
    {
        private string _logText;
        public string LogText {  get { return _logText; } set { _logText = value; OnPropertyChanged(() => LogText); } }
    }
}
