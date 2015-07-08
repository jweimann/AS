using Akka.Actor;
using AS.Messages.SystemStats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors.StatsActors
{
    public class StatsGatherer : ReceiveActor
    {
        HashSet<IActorRef> _rooms = new HashSet<IActorRef>();
        HashSet<IActorRef> _games = new HashSet<IActorRef>();
        
        private HashSet<IActorRef> _subscribers = new HashSet<IActorRef>();

        public StatsGatherer()
        {
            var path = Self.Path.ToStringWithAddress();
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10), Self, new RefreshSystemStats(), Self);
            Receive<RefreshSystemStats>(message => RefreshSystemStats());
            Receive<GetSystemStats>(message => {
                var exists = Sender.IsNobody();
                Sender.Tell(new SystemStats(_rooms.Count, _games.Count));
            });

            Receive<RoomStats>(message =>
            {
                _rooms.Add(Sender);
                Debug.WriteLine($"Room {Sender.Path} Responded");
                SendStatsToSubscribers();
            });

            Receive<GameStats>(message =>
            {
                _games.Add(Sender);
                Debug.WriteLine($"Game {Sender.Path} Responded");
                SendStatsToSubscribers();
            });

            Receive<SubscribeToStats>(message =>
            {
                Sender.Tell(new SystemStats(-1, -1));
                _subscribers.Add(message.Subscriber);
            });
        }

        private void SendStatsToSubscribers()
        {
            foreach (var subscriber in _subscribers)
            {
                if (!subscriber.IsNobody())
                    subscriber.Tell(new SystemStats(_rooms.Count, _games.Count));
            }
        }

        private void RefreshSystemStats()
        {
            _rooms.Clear();
            ActorSelection rooms = Context.System.ActorSelection("/user/lobby/*");
            rooms.Tell(new GetSystemStats());

            _games.Clear();
            ActorSelection games = Context.System.ActorSelection("/user/GamesRoot/*");
            games.Tell(new GetSystemStats());
        }
    }
}
