using Akka.Actor;
using Akka.Configuration;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AS.Admin.ChatClient.Authentication;

namespace AS.Admin.ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RoomViewModel _vm { get; set; }
        private ActorSystem Sys;

        private RoomController _roomController;
        private LobbyController _lobbyController;
        private IActorRef _clientUserConnection;

        public MainWindow()
        {
            InitializeComponent();
            CreateSystem();
        }


        public List<string> Users { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var authController = new AuthenticationController(Sys);
            return;
            InitializeClientUserConnection();
        }

        private void InitializeClientUserConnection()
        {
            var roomsViewModel = new RoomViewModel();
            var lobbyViewModel = new LobbyViewModel();

            _roomController = new RoomController(Sys, ref roomsViewModel);
            DataContext = roomsViewModel;

            _lobbyController = new LobbyController(Sys, ref lobbyViewModel);
            LobbyPanel.DataContext = lobbyViewModel;

            _clientUserConnection = Sys.ActorOf(Props.Create<ClientUserConnection>(_lobbyController, _roomController.ClientRoom));
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
        

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _roomController.SendChat((sender as TextBox).Text);
                (sender as TextBox).Text = String.Empty;
            }
        }

        private void RefreshRooms_Click(object sender, RoutedEventArgs e)
        {
            _clientUserConnection.Tell(new GetRooms());
        }
    }
}
