using Akka.Actor;
using Akka.Configuration;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AS.Admin.ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RoomViewModel _vm { get; set; }
        private ActorSystem Sys;
        private ActorSelection _connectionManager;

        public MainWindow()
        {
            InitializeComponent();
            CreateSystem();
        }


        public List<string> Users { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Users = new List<string>() { "one", "two" };
            //this.DataContext = this;
            var vm = new RoomViewModel();
            _roomController = new RoomController(this.Sys, ref vm);

            var lobbyvm = new LobbyViewModel();
            _lobbyController = new LobbyController(this.Sys, ref lobbyvm);
            lobbyList.DataContext = _lobbyController;

            this.DataContext = vm;
            vm.Users.Add("jason1234");

            //return;
            
            
        }

        private void HandleUserListReceived(UserList obj)
        {
            MessageBox.Show(obj.ToString());
        }

        private void CreateSystem()
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
		    applied-adapters = []
		    transport-protocol = tcp
		    port = 0
		    hostname = localhost
        }
    }
}
");

            this.Sys = ActorSystem.Create("MyClient", config);

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _roomController.DoStuff();
        }
        private RoomController _roomController;
        private LobbyController _lobbyController;

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _roomController.SendChat((sender as TextBox).Text);
                (sender as TextBox).Text = String.Empty;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
