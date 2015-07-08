using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
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
        private UnityEngine.Bounds _bounds;
        private HashSet<IActorRef> _subscribedUsers = new HashSet<IActorRef>();
        private Dictionary<long, IActorRef> _entities = new Dictionary<long, IActorRef>();
        private Dictionary<long, Vector3> _entityLocations = new Dictionary<long, Vector3>();
        private int _maxEntities;
        private Dictionary<UnityEngine.Bounds, IActorRef> _children = new Dictionary<UnityEngine.Bounds, IActorRef>();
        private bool HasChildren {  get { return _children.Count > 0; } }

        public Region(UnityEngine.Bounds bounds, int maxEntities)
        {
            System.Diagnostics.Debug.WriteLine($"Region Spawned {Self.Path.ToString()}");
            _maxEntities = maxEntities;
            _bounds = bounds;
            Receive<AddEntityToRegion>(message => AddEntity(message));
            Receive<UpdatePosition>(message =>
            {
                _entityLocations[message.EntityId] = message.Position;
                MessagePlayers(message);
            });
            Receive<RequestEntityList>(message =>
            {
                Sender.Tell(new EntitiesInRegionList(_entities.Values.ToList(), Self.Path.ToString()));
                foreach (var child in _children.Values)
                    child.Tell(message, Sender);
            });

            Receive<SetPosition>(message =>
            {
                _entities[message.EntityId].Tell(message);
            });

            Receive<SubscribeUserToRegion>(message => _subscribedUsers.Add(message.UserActor));
            Receive<UnsubscribeUserToRegion>(message => _subscribedUsers.Remove(message.UserActor));
        }

        private void MessagePlayers(UpdatePosition message)
        {
            foreach (var user in _subscribedUsers)
                user.Tell(message);
        }

        private void AddEntity(AddEntityToRegion message)
        {
            if (EntityPositionIsOutOfBounds(message))
            {
                System.Diagnostics.Debug.WriteLine($"Unable to spawn entity at position {message.Position.ToString()} which is out of bounds {_bounds.ToString()}");
                Context.Parent.Tell(message);
                return;
            }

            if (HasChildren)
            {
                var childRegion = GetRegion(message.Position);
                childRegion.Tell(message);
                return;
            }

            System.Diagnostics.Debug.WriteLine($"{Self.Path.ToString()} Recieved Message: {message.ToString()} FROM {Sender.Path.ToString()}");
            if (_entities.ContainsKey(message.EntityId))
                throw new Exception($"Entity ID already exists in database.  ID: {message.EntityId}");
            _entities.Add(message.EntityId, message.EntityActor);

            

            if (_entities.Count > _maxEntities)
                SplitRegion();
            else
                message.EntityActor.Tell(new JoinRegionSuccess(Self));

            System.Diagnostics.Debug.WriteLine($"AddEntity SUCCESS.  Total Entities: {_entities.Count}");

            Self.Tell(new SubscribeUserToRegion(message.UserActor));
            //else
            //    message.EntityActor.Tell(new JoinRegion(Self));
        }

        private bool EntityPositionIsOutOfBounds(AddEntityToRegion message)
        {
            return _bounds.Contains(message.Position) == false;
        }

        private void SplitRegion()
        {
            throw new NotImplementedException("Disabled this when switching to UnityEngine Bounds and Vector3D because the UnityEngine bounds doesn't have .Split() - Add or fix this.");

            /*
            UnityEngine.Bounds[] splitBounds = _bounds.Split();
            for (int i = 0; i < splitBounds.Length; i++)
            {
                Props props = Props.Create<Region>(new object[] { splitBounds[i], _maxEntities });
                IActorRef child = Context.ActorOf(props, splitBounds[i].ToString());
                _children.Add(splitBounds[i], child);
                System.Diagnostics.Debug.WriteLine($"ChildRegion: {child.Path.ToString()}");
            }

            foreach (var entityKvp in _entities)
            {
                var entity = entityKvp.Value;
                var entityId = entityKvp.Key;
                object pos = entity.Ask<Vector3>(new GetPosition()).Result;
                System.Diagnostics.Debug.WriteLine($"Pos={pos.GetType().ToString()}");
                Vector3 position = (Vector3)pos;
                var childRegion = GetRegion(position);
                childRegion.Tell(new AddEntityToRegion(entityId, entity, position, Sender));
                entity.Tell(new JoinRegionSuccess(childRegion));
            }
            _entities.Clear();
            System.Diagnostics.Debug.WriteLine($"{Self.Path.ToString()} Entities Cleared");
            */
        }

        private IActorRef GetRegion(Vector3 position)
        {
            foreach (var key in _children.Keys)
            {
                if (key.Contains(position))
                    return _children[key];
            }
            System.Diagnostics.Debug.WriteLine($"Unable to find Child Region for Position {position.ToString()}");
            foreach (var key in _children.Keys)
                System.Diagnostics.Debug.WriteLine($"Child Bounds {key.ToString()}");

            return null;
            //return _children.First().Value;
            // iterate over _children Key and find match, then return and send msg to join it to the entity.
        }


    }
}
