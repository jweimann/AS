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
        private Common.Vector3 _position;
        private IActorRef _region;

        public Entity(int entityId, Common.Vector3 position)
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
