using System;
using UnityEngine;
using Akka.Actor;
using AS.Messages.Game;
using AS.Messages.Region;

namespace AS.Messages.Entities
{
    public class Entity : ReceiveActor
    {
        private long _entityId;
        private Vector3 _position;
        private IActorRef _region;

        public Entity(long entityId, Vector3 position)
        {
            _entityId = entityId;
            _position = position;

            Receive<GetPosition>(message => 
                Sender.Tell(_position)
            );

            Receive<SetPosition>(message =>
            {
                _position = message.Position;
                _region.Tell(new UpdatePosition(_entityId, _position));
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

            _position += Vector3.one;
            _region.Tell(new UpdatePosition(_entityId, _position));
        }
    }
}
