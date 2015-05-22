using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors.Lobby
{
    public class Room : ReceiveActor
    {
        // HashSet automatically eliminates duplicates
        private HashSet<IActorRef> _subscribers;

        public Room()
        {
            _subscribers = new HashSet<IActorRef>();

            Receive<JoinRoom>(sub =>
            {
                if (_subscribers.Contains(sub.ActorRef) == false)
                    MessageSubscribers(new UserJoinedRoom(sub.ActorRef.ToString()));

                sub.ActorRef.Tell(new JoinRoomSuccess(Self));
                _subscribers.Add(sub.ActorRef);

                sub.ActorRef.Tell(new UserList(GetUserList()));
            });

            Receive<Chat>(message => {
                // notify each subscriber
               MessageSubscribers(message);
            });

            Receive<LeaveRoom>(unsub =>
            {
                _subscribers.Remove(unsub.ActorRef);
                MessageSubscribers(new UserLeftRoom(unsub.ActorRef.ToString()));
            });
        }

        private List<string> GetUserList()
        {
            return _subscribers.Select(t => t.Path.ToString()).ToList();
        }

        private void MessageSubscribers(object message)
        {
            foreach (var sub in _subscribers)
            {
                Debug.WriteLine(String.Format("Messaging Subscriber {0} - {1}", sub.Path.ToString(), message.ToString()));
                sub.Tell(message);
            }
        }
    }
}
