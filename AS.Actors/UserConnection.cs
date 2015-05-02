using Akka.Actor;
using AS.Interfaces;
using System.Diagnostics;
namespace AS.Actors
{
    public class UserConnection : UntypedActor, IWithUnboundedStash
    {
        IActorRef _user;
        IActorRef _testActor;
        private IConnection _connection;
        public UserConnection(IActorRef testActor)
        {
            //_connection = connection;
            _user = Context.Parent;
            _testActor = testActor;// Context.System.ActorSelection("/user/testActor1");
            Debug.WriteLine("TestActor: " + _testActor.ToString());
        }

        protected override void OnReceive(object message)
        {
            var senderSelection = Context.ActorSelection(Sender.Path);
            if (Sender != _user)
                ForwardMessageToUser(message);
            else
                ForwardMessageToClient(message);
        }

        private void ForwardMessageToClient(object message)
        {
            if (_testActor == null)
            {
                Debug.WriteLine("Stashing Message: " + message.ToString());
                Stash.Stash();
            }
            else
            {
                Debug.WriteLine("Forwarding Message To Client: " + message.ToString());
                _testActor.Tell(message);
            }
        }

        private void ForwardMessageToUser(object message)
        {
            Debug.WriteLine("Forwarding Message To User: " + message.ToString());
            _user.Tell(message);
        }

        public IStash Stash { get; set; }
    }
}