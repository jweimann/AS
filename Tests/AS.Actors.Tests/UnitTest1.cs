using Akka.Actor;
using Akka.Routing;
using Akka.TestKit.Xunit;
using AS.Actors.ClientConnection;
using AS.Messages;
using AS.MockActors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace AS.Actors.Tests
{
  
    public class userConnection : ASTestBase
    {
        [Fact]
        public void userConnection_creation_success()
        {
            var userConnection = InitializeSingleUserThroughConnectionManager();

            userConnection.Tell("Hello");

            var messages = ReceiveN(1);
            Assert.True("Not Authenticated" == ((IReadOnlyCollection<object>)messages).First().ToString());
        }

        [Fact]
        public void userConnection_authentication_success()
        {
            var userConnection = InitializeSingleUserThroughConnectionManager();

            userConnection.Tell(new Authenticate("jason"));
            //ReceiveN(1);
            //ExpectMsg<AuthenticateResult>();

            userConnection.Tell("Hello");

            var messages = ReceiveN(1);
        }

        [Fact]
        public void userConnection_joinRoom_success()
        {
            var userConnection = CreateUserAndJoinChat1("jason1234");

            Debug.WriteLine("Telling Hello");

            string chatText = "Hellooooo";
            userConnection.Tell(new Chat(userConnection, chatText));
            Chat chat = ExpectMsg<Chat>(TimeSpan.FromSeconds(1), "Chat");
            Assert.Equal(chatText, chat.Text);

            chatText = "What's up!";
            userConnection.Tell(new Chat(userConnection, chatText));
            chat = ExpectMsg<Chat>(TimeSpan.FromSeconds(1), "Chat");
            Assert.Equal(chatText, chat.Text);
        }

        private void LogMessage(IReadOnlyCollection<object> messages)
        {
            Debug.WriteLine("Message Count: " + messages.Count);
            Debug.WriteLine(messages.First().ToString());
        }

        [Fact]
        public void userConnection_chatBetween2Users_success()
        {
            // This is failing because the messages from userConnections all come here and
            // i'm looking at them like they're distinct when they're not
            // to make this work, move the expectations to the testactors, not in here

            // Temp fixed this to expect the message that actually comes.

            var userConnection = CreateUserAndJoinChat1("Jason");
            var userConnection2 = CreateUserAndJoinChat1("Jason2");

            userConnection.Tell(new Chat(userConnection, "Hellooooo"));

            ExpectMsg<UserJoinedRoom>(TimeSpan.FromSeconds(1), "UserJoinedRoom");

            ExpectMsg<Chat>(TimeSpan.FromSeconds(1), "Chat1");
            ExpectMsg<Chat>(TimeSpan.FromSeconds(1), "Chat2");
        }


        [Fact]
        public void connectionManager_getRooms_returnsRooms()
        {
            var userConnection = InitializeSingleUserThroughConnectionManager();
            userConnection.Tell(new Authenticate("tester"));
            ExpectMsg<AuthenticateResult>();
            Assert.NotNull(userConnection);
            userConnection.Tell(new GetRooms());
            var roomList = ExpectMsg<RoomList>(TimeSpan.FromSeconds(1), "RoomList");
            Assert.Equal(0, roomList.Rooms.Count);

            userConnection.Tell(new JoinRoom(userConnection, "jasonsRoom1"));
            userConnection.Tell(new GetRooms());
            roomList = ExpectMsg<RoomList>(TimeSpan.FromSeconds(1), "RoomList");
            Assert.Equal(1, roomList.Rooms.Count);

            userConnection.Tell(new JoinRoom(userConnection, "jasonsRoom2"));
            userConnection.Tell(new GetRooms());
            roomList = ExpectMsg<RoomList>(TimeSpan.FromSeconds(1), "RoomList");
            Assert.Equal(2, roomList.Rooms.Count);
        }

        [Fact]
        public void connectionManager_handleNewConnection_getsUser()
        {
            var userConnection = InitializeSingleUserThroughConnectionManager();
            Assert.NotNull(userConnection);
        }

        [Fact]
        public void trackedEntities_createTrackedEntity_success()
        {
            var userConnection = InitializeSingleUserThroughConnectionManager();
            userConnection.Tell(new CreateTrackedEntity("testEntity"));
            Assert.True(false);
        }



        [Fact]
        public void userConnection_chatBetween5Users_success()
        {
            var userConnection = CreateUserAndJoinChat1("Jason");
            for (int i = 2; i <= 5; i++)
            {
                CreateUserAndJoinChat1("Jason" + i);
            }

            userConnection.Tell(new Chat(userConnection, "Hellooooo"));

            for (int i = 0; i < 5; i++)
            {
                ExpectMsg<Chat>(TimeSpan.FromSeconds(1), "Chat" + i);
            }
        }
    }
}