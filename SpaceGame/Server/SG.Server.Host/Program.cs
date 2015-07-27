using Akka.Actor;
using Akka.Configuration;
using AS.Actors.ClientConnection;
using AS.Actors.GameActors;
using AS.Actors.Initalization;
using AS.Actors.StatsActors;
using AS.Actors.UserActors;
using SG.Server.Actors;
using System;

namespace SG.Server.Host
{
    public class Program
    {
        private static ActorSystem Sys;
        private static IActorRef _users;
        private static IActorRef _lobby;
        private static IActorRef _gamesRoot;
        private static IActorRef _statsGatherer;

        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
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
		    port = 8081
		    hostname = localhost
        }
    }
}
");

            Sys = ActorSystem.Create("as", config);
            CreateRootActors();
            Console.ReadLine();
        }

        protected static void CreateRootActors()
        {
            if (_users == null)
                _users = Sys.ActorOf<Users>("users");

            //if (_mockClientConnectionManager == null)
            //    _mockClientConnectionManager = Sys.ActorOf<MockClientConnectionManager>("ConnectionManager");

            if (_lobby == null)
                _lobby = Sys.ActorOf<AS.Actors.Lobby.Lobby>("lobby");

            var _clientConnectionManager = Sys.ActorOf<ClientConnectionManager>("ClientConnectionManager");

            Props props = Props.Create<GamesRoot>(new object[] { typeof(SpaceGameInitializer) });
            _gamesRoot = Sys.ActorOf(props, "GamesRoot");

            _statsGatherer = Sys.ActorOf<StatsGatherer>("StatsGatherer");
        }


    }
}
