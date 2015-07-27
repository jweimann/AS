using System;
using Akka.Actor;
using AS.Common;
using AS.Messages.Region;
using AS.Client.Messages.Entities;

namespace AS.Messages.Entities
{
    public class Entity : ReceiveActor
    {
        private int _entityId;
        private Vector3 _position;
        private Vector3 _velocity;
        private IActorRef _region;
        private static Random _random = new Random();

        public Entity(int entityId, Common.Vector3 position)
        {
            Console.WriteLine($"Entity {entityId} Spawned at {position.ToString()}");

            _entityId = entityId;
            _position = position;

            Receive<GetPosition>(message => 
                Sender.Tell(_position)
            );

            Receive<SetPosition>(message =>
            {
                _position = message.Position;
                _region.Tell(new UpdatePosition(_entityId, _position, _velocity));
            });

            //Receive<JoinRegion>(message =>
            //{
            //    _region = message.Region;
            //    _region.Tell(new AddEntityToRegion(_entityId, Self));
            //    Context.System.Scheduler.Advanced.ScheduleRepeatedly(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), () => Tick());
            //});

            Receive<JoinRegionSuccess>(message =>
            {
                _region = message.RegionActor;
            });

            Context.System.Scheduler.Advanced.ScheduleRepeatedly(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), () => Tick());
        }

        private void Tick()
        {
            if (_region == null)
                return;

            _position += _velocity;

            if (_velocity == Vector3.zero)
                RandomizeVelocity();

            _region.Tell(new UpdatePosition(_entityId, _position, _velocity));
        }

        private void RandomizeVelocity()
        {
            //_velocity = new Vector3(1, 1, 1);
            _velocity = new Vector3(_random.Next(-1, 1), _random.Next(-1, 1), _random.Next(-1, 1));
        }
    }
}
