using Akka.Actor;
using Akka.Configuration;
using AS.MockActors;
using System;
using AS.Actors.GameActors;
using AS.Actors.UserActors;
using AS.Actors.StatsActors;

namespace AS.TestHost
{
    class Program
    {
        private static ActorSystem Sys;
        private static IActorRef _users;
        private static IActorRef _lobby;
        private static IActorRef _mockClientConnectionManager;
        private static IActorRef _gamesRoot;
        private static IActorRef _statsGatherer;

        static void Main(string[] args)
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

            if (_mockClientConnectionManager == null)
                _mockClientConnectionManager = Sys.ActorOf<MockClientConnectionManager>("ConnectionManager");

            if (_lobby == null)
                _lobby = Sys.ActorOf<AS.Actors.Lobby.Lobby>("lobby");

            _gamesRoot = Sys.ActorOf<GamesRoot>("GamesRoot");

            _statsGatherer = Sys.ActorOf<StatsGatherer>("StatsGatherer");
        }

        
    }
}
