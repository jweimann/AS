using Akka.Actor;
using Akka.Configuration;
using AS.Admin.ServerStatsClient.Controllers;
using AS.Admin.ServerStatsClient.ViewModels;
using System.Threading;
using System.Windows;

namespace AS.Admin.ServerStatsClient
{
    public partial class MainWindow : Window
    {
        private ServerStatsViewModel _viewModel;
        private IActorRef _serverStatsController;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ServerStatsViewModel();
            DataContext = _viewModel;
            CreateSystem();

            _serverStatsController = Sys.ActorOf(Props.Create<ServerStatsController>(new object[] { _viewModel })
                    //.WithDispatcher("akka.actor.synchronized-dispatcher"),
                    ,"ServerStatsController");
        }

        public ActorSystem Sys { get; private set; }

        private void CreateSystem()
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    loglevel = ""DEBUG""
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
            //Thread.Sleep(3000);
        }
    }
}
