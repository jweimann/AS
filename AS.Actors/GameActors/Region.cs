using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using Akka.Actor;
using AS.Common;
using AS.Messages.Game;
using AS.Messages.Entities;
using System;
using AS.Messages.Region;

namespace AS.Actors
{


    public class Region : ReceiveActor
    {
        private Bounds _bounds;
        private Dictionary<long, IActorRef> _entities = new Dictionary<long, IActorRef>();
        private Dictionary<long, Vector3> _entityLocations = new Dictionary<long, Vector3>();
        private int _maxEntities;
        private Dictionary<Bounds, IActorRef> _children = new Dictionary<Bounds, IActorRef>();
        private bool HasChildren {  get { return _children.Count > 0; } }

        public Region(Bounds bounds, int maxEntities)
        {
            Debug.WriteLine($"Region Spawned {Self.Path.ToString()}");
            _maxEntities = maxEntities;
            _bounds = bounds;
            Receive<AddEntityToRegion>(message => AddEntity(message));
            Receive<UpdatePosition>(message =>
            {
                _entityLocations[message.EntityId] = message.Position;
                MessageEntities(message, _entities[message.EntityId]);
            });
            Receive<RequestEntityList>(message =>
            {
                Sender.Tell(new EntitiesInRegionList(_entities.Values.ToList(), Self.Path.ToString()));
                foreach (var child in _children.Values)
                    child.Tell(message, Sender);
            });
        }

        private void MessageEntities(UpdatePosition message, IActorRef excludeActorRef = null)
        {
            return;
            foreach (var entity in _entities.Values)
            {
                if (excludeActorRef != null && entity.Path != excludeActorRef.Path)
                    entity.Tell(message);
            }
        }

        private void AddEntity(AddEntityToRegion message)
        {
            if (EntityPositionIsOutOfBounds(message))
            {
                Debug.WriteLine($"Unable to spawn entity at position {message.Position.ToString()} which is out of bounds {_bounds.ToString()}");
                Context.Parent.Tell(message);
                return;
            }

            if (HasChildren)
            {
                var childRegion = GetRegion(message.Position);
                childRegion.Tell(message);
                return;
            }

            Debug.WriteLine($"{Self.Path.ToString()} Recieved Message: {message.ToString()} FROM {Sender.Path.ToString()}");
            if (_entities.ContainsKey(message.EntityId))
                throw new Exception($"Entity ID already exists in database.  ID: {message.EntityId}");
            _entities.Add(message.EntityId, message.EntityActor);


            if (_entities.Count > _maxEntities)
                SplitRegion();
            else
                message.EntityActor.Tell(new JoinRegionSuccess(Self));

            Debug.WriteLine($"AddEntity SUCCESS.  Total Entities: {_entities.Count}");
            //else
            //    message.EntityActor.Tell(new JoinRegion(Self));
        }

        private bool EntityPositionIsOutOfBounds(AddEntityToRegion message)
        {
            return _bounds.Contains(message.Position) == false;
        }

        private void SplitRegion()
        {
            Bounds[] splitBounds = _bounds.Split();
            for (int i = 0; i < splitBounds.Length; i++)
            {
                Props props = Props.Create<Region>(new object[] { splitBounds[i], _maxEntities });
                IActorRef child = Context.ActorOf(props, splitBounds[i].ToString());
                _children.Add(splitBounds[i], child);
                Debug.WriteLine($"ChildRegion: {child.Path.ToString()}");
            }

            foreach (var entityKvp in _entities)
            {
                var entity = entityKvp.Value;
                var entityId = entityKvp.Key;
                object pos = entity.Ask<Vector3>(new GetPosition()).Result;
                Debug.WriteLine($"Pos={pos.GetType().ToString()}");
                Vector3 position = (Vector3)pos;
                var childRegion = GetRegion(position);
                childRegion.Tell(new AddEntityToRegion(entityId, entity, position));
                entity.Tell(new JoinRegionSuccess(childRegion));
            }
            _entities.Clear();
            Debug.WriteLine($"{Self.Path.ToString()} Entities Cleared");
        }

        private IActorRef GetRegion(Vector3 position)
        {
            foreach (var key in _children.Keys)
            {
                if (key.Contains(position))
                    return _children[key];
            }
            Debug.WriteLine($"Unable to find Child Region for Position {position.ToString()}");
            foreach (var key in _children.Keys)
                Debug.WriteLine($"Child Bounds {key.ToString()}");

            return null;
            //return _children.First().Value;
            // iterate over _children Key and find match, then return and send msg to join it to the entity.
        }


    }
}
