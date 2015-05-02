//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AS.Actors.Tests
//{
//    public class debugTests : ASTestBase
//    {
//        [Fact]
//        public void test1()
//        {
//            var userConnection = InitializeSingleUserThroughConnectionManager();
//            userConnection.Tell("Ping");
//            userConnection.Tell("Ping");
//            var messages2 = ReceiveN(2);

//            userConnection.Tell("Hello");
//            var messages = ReceiveN(1);
//        }
//        [Fact]
//        public void test2()
//        {
//            var userConnection = InitializeSingleUserThroughConnectionManager();
//            userConnection.Tell("Ping");
//            userConnection.Tell("Ping");
//            var messages2 = ReceiveN(2);

//            //userConnection.Tell("Hello");
//            //var messages = ReceiveN(1);
//        }
//        [Fact]
//        public void test3()
//        {
//            var userConnection = InitializeSingleUserThroughConnectionManager();
//            userConnection.Tell("Ping");
//            userConnection.Tell("Ping");
//            var messages2 = ReceiveN(2);

//            //userConnection.Tell("Hello");
//            //var messages = ReceiveN(1);
//        }
//        [Fact]
//        public void test4()
//        {
//            var userConnection = InitializeSingleUserThroughConnectionManager();
//            userConnection.Tell("Ping");
//            userConnection.Tell("Ping");
//            var messages2 = ReceiveN(2);

//            //userConnection.Tell("Hello");
//            //var messages = ReceiveN(1);
//        }
//        [Fact]
//        public void test5()
//        {
//            var userConnection = InitializeSingleUserThroughConnectionManager();
//            userConnection.Tell("Ping");
//            userConnection.Tell("Ping");
//            var messages2 = ReceiveN(2);

//            //userConnection.Tell("Hello");
//            //var messages = ReceiveN(1);
//        }
//    }

//    public class autoReplyDebugTests : ASTestBase
//    {
//        private IActorRef _autoReply;
//        private IActorRef GetAutoReply()
//        {
//            if (_autoReply == null)
//                _autoReply = ActorOf<AutoReplyActor>();

//            CreateRootActors();

//            Debug.WriteLine("Sending Connection");
//            _mockClientConnectionManager.Tell(new ConnectionEstablished(new MockActorConnection(base.TestActor)));
//            this.TestActor.Tell("asdf");
//            Debug.WriteLine("SENT Connection");
//            Task.Delay(100);
//            Debug.WriteLine("Waiting For Msg");
//            var msgs = ReceiveN(2);
//            Debug.WriteLine(msgs.Count);
//            Debug.WriteLine(msgs.Last().ToString());
//            Assert.IsType<UserCreated>(msgs.Last());
//            var userMessage = ((UserCreated)msgs.Last());
//            //UserCreated userMessage = ExpectMsg<UserCreated>(TimeSpan.FromSeconds(1), "User Creation Failed");
//            Debug.WriteLine("Got AutoReply");

//            return _autoReply;
//        }
//        private void RunTheStuff()
//        {
//            var userConnection = GetAutoReply();
//            userConnection.Tell("Hello");
//            Task.Delay(100);
//            var messages = ReceiveN(1);
//        }
//        [Fact]
//        public void test1()
//        {
//            RunTheStuff();
//        }
//        [Fact]
//        public void test2()
//        {
//            RunTheStuff();
//        }
//        [Fact]
//        public void test3()
//        {
//            RunTheStuff();
//        }
//        [Fact]
//        public void test4()
//        {
//            RunTheStuff();
//        }
//        [Fact]
//        public void test5()
//        {
//            RunTheStuff();
//        }
//    }


//}
