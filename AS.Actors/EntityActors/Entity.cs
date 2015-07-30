using System;
using Akka.Actor;
using AS.Common;
using AS.Messages.Region;
using AS.Client.Messages.Entities;
using AS.Client.Messages.ClientRequests;

namespace AS.Messages.Entities
{
    public class Entity : ReceiveActor
    {
        private int _entityId;
        private Vector3 _position;
        private Vector3 _velocity;
        private IActorRef _region;
        private static Random _random = new Random();
        private EntityType _entityType;

        private int _lastTickCount = Environment.TickCount;
        private Vector3? _targetPosition;
        private int? _targetEntityId;

        public Entity(int entityId, Common.Vector3 position, EntityType entityType)
        {
            Console.WriteLine($"Entity {entityId} Spawned at {position.ToString()}");
            _entityType = entityType;

            _entityId = entityId;
            _position = position;
            _velocity = Vector3.zero;

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

            Receive<ClientRequestEntityDetails>(message =>
            {
                Sender.Tell(new EntityDetails(_entityId, _entityType));
            });

            Receive<SG.Client.Messages.SetTargetObject>(message =>
            {
                Console.WriteLine("Set Target");
                _targetPosition = null;
                _targetEntityId = message.ActorId;
            });

            Receive<EntityPositionResponse>(message =>
            {
                if (_targetEntityId == message.EntityId)
                {
                    _targetPosition = message.Position;
                }
            });

            Receive<TickMessage>(_ => Tick());

            //RandomizeVelocity();

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(0.1),
                TimeSpan.FromSeconds(0.1),
                Self,
                TickMessage.Instance,
                Self);
            //Context.System.Scheduler.Advanced.ScheduleRepeatedly(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1), () => Tick());
        }
        
        private void Tick()
        {
            if (_region == null)
                return;

            int deltaMs = Environment.TickCount - _lastTickCount;
            double deltaTime = (float)deltaMs / 1000f;
            _lastTickCount = Environment.TickCount;

            //if (_velocity == Vector3.zero)
            //    RandomizeVelocity();
            FaceTarget();

            _position += (_velocity * deltaTime);

            _region.Tell(new UpdatePosition(_entityId, _position, _velocity));
        }

        private void FaceTarget()
        {
            if (_targetEntityId != null)
            {
                _region.Tell(new RequestEntityPosition(_targetEntityId.Value));
                if (_targetPosition != null)
                {
                    _velocity = (_targetPosition.Value - _position).Normalized();
                }
            }
            
        }

        private void RandomizeVelocity()
        {
            //_velocity = new Vector3(1, 1, 1);
            _velocity = new Vector3(_random.Next(-1, 1), _random.Next(-1, 1), _random.Next(-1, 1));
        }
    }
}
