using Akka.Actor;
using Akka.TestKit.Xunit;
using AS.Actors.ClientConnection;
using AS.Messages;
using AS.MockActors;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AS.Actors.UserActors;
using AS.Actors.GameActors;
using Akka.TestKit;
using AS.Messages.Game;
using AS.Client.Messages;
using AS.Client.Messages.Game;
using AS.Actors.Initalization;

namespace AS.Actors.Tests
{
    public class ASTestBase : TestKit
    {
        protected IActorRef _lobby;
        protected IActorRef _users;
        protected IActorRef _mockClientConnectionManager;
        protected IActorRef _gamesRoot;

        public ASTestBase()
        {
            CreateRootActors();
        }

        protected IActorRef InitializeSingleUserThroughConnectionManager()
        {
            CreateRootActors();

            Debug.WriteLine("Sending Connection");
            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(base.TestActor)));
            this.TestActor.Tell("asdf");
            Debug.WriteLine("SENT Connection");
            Task.Delay(100);
            Debug.WriteLine("Waiting For Msg");
            var msgs = ReceiveN(2);
            Debug.WriteLine(msgs.Count);
            Debug.WriteLine(msgs.Last().ToString());
            Assert.IsType<UserCreated>(msgs.Last());
            var userMessage = ((UserCreated)msgs.Last());
            //UserCreated userMessage = ExpectMsg<UserCreated>(TimeSpan.FromSeconds(1), "User Creation Failed");
            Debug.WriteLine("Got Msg");

            return userMessage.UserConnectionActor;
        }

        protected void CreateRootActors()
        {
            if (_users == null)
                _users = Sys.ActorOf<Users>("users");
            
            if (_mockClientConnectionManager == null)
                _mockClientConnectionManager = Sys.ActorOf<MockClientConnectionManager>("ConnectionManager");

            if (_lobby == null)
                _lobby = Sys.ActorOf<AS.Actors.Lobby.Lobby>("lobby");

            if (_gamesRoot == null)
                _gamesRoot = Sys.ActorOf(Props.Create<GamesRoot>(new object[] { typeof(GameInitializer) }, "GamesRoot"));
        }

        protected IActorRef CreateUserAndJoinChat1(string name)
        {
            var userConnection = InitializeSingleUserThroughConnectionManager();

            userConnection.Tell(new Authenticate(name));
            ExpectMsg<AuthenticateResult>(TimeSpan.FromSeconds(10), "AuthenticateResult " + name);
            userConnection.Tell(new JoinRoom(userConnection, "Chat1"));
            ExpectMsg<JoinRoomSuccess>(TimeSpan.FromSeconds(10), "JoinRoomSuccess " + name);
            ExpectMsg<UserList>(TimeSpan.FromSeconds(20), "UserList " + name);
            //ExpectMsg<UserJoinedRoom>(TimeSpan.FromSeconds(10), "UserJoinedRoom " + name);
            return userConnection;
        }

        protected static Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject GetPrivate<TActorType>(Akka.TestKit.TestActorRef<TActorType> region) where TActorType : ActorBase
        {
            return new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(region.UnderlyingActor, new
            Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(TActorType)));
        }

        protected TestActorRef<User> CreateUserAndGame(out IActorRef game)
        {
            Props props = Props.Create<User>(new object[] { TestActor });
            var user = ActorOfAsTestActorRef<User>(props, "TestUser");
            ExpectMsg<UserCreated>();
            user.Tell(new Authenticate("TestUser"));
            ExpectMsg<AuthenticateResult>();
            user.Tell(new CreateGame("TestGame"));
            JoinGameSuccess response = ExpectMsg<JoinGameSuccess>();
            Assert.False(response.Game.IsNobody());
            game = response.Game;
            return user;
        }
    }
}
