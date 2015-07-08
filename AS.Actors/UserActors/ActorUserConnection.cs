using Akka.Actor;
using System;
using System.Diagnostics;

namespace AS.Actors.UserActors
{
    /// <summary>
    /// Handles the connection between a user and a client that connects via remote actor.
    /// Ex. Admin client connects to the actor system and uses this to link between it's Client actors and the user.
    /// </summary>
    public class ActorUserConnection : UntypedActor, IWithUnboundedStash
    {
        private IActorRef _user;
        private IActorRef _remoteActor;

        public IStash Stash { get; set; }

        public ActorUserConnection(IActorRef testActor)
        {
            _user = Context.Parent;
            _remoteActor = testActor;// Context.System.ActorSelection("/user/testActor1");
            Debug.WriteLine("RemoteActor: " + _remoteActor.ToString());
        }

        protected override void OnReceive(object message)
        {
            var senderSelection = Context.ActorSelection(Sender.Path);
            if (Sender.Path != _user.Path)
                ForwardMessageToUser(message);
            else
                ForwardMessageToClient(message);
        }

        private void ForwardMessageToClient(object message)
        {
            if (_remoteActor == null)
            {
                Debug.WriteLine("Stashing Message: " + message.ToString());
                Stash.Stash();
            }
            else
            {
                Debug.WriteLine("Forwarding Message To Client: " + message.ToString());
                Console.WriteLine("Forwarding Message To Client: " + message.ToString());
                _remoteActor.Tell(message);
            }
        }

        private void ForwardMessageToUser(object message)
        {
            Debug.WriteLine("Forwarding Message To User: " + message.ToString());
            Console.WriteLine("Forwarding Message To User: " + message.ToString());
            _user.Tell(message);
        }
    }
}