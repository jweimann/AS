using Akka.Actor;
using Akka.Configuration;
using AS.Actors;
using AS.MockActors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.TestHost
{
    class Program
    {
        private static ActorSystem Sys;
        private static IActorRef _users;
        private static IActorRef _lobby;
        private static IActorRef _mockClientConnectionManager;
        static void Main(string[] args)
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
        }

        
    }
}
