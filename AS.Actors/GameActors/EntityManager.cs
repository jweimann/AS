using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using AS.Messages.Entities;
using AS.Messages;
using AS.Messages.Game;
using System.Diagnostics;

namespace AS.Actors
{
    public class EntityManager : ReceiveActor
    {
        private ActorSelection _userActors;
        private int _nextEntityId;
        private ActorSelection _regionManager;

        private List<IActorRef> Entities { get; set; } = new List<IActorRef>();
        public Dictionary<long, IActorRef> EntitiesById { get; set; } = new Dictionary<long, IActorRef>();

        public EntityManager()
        {
            _regionManager = Context.ActorSelection("../RegionManager");
            Receive<SpawnEntity>(message => HandleSpawnEntityMessage(message));
            Receive<EntityMessage>(message => HandleEntityMessage(message));
        }

        private void HandleEntityMessage(EntityMessage message)
        {
            
        }

        private void HandleSpawnEntityMessage(SpawnEntity message)
        {
            IActorRef entity = SpawnEntity(message);

            //SpawnEntity innerMessage = new SpawnEntity(message.EntityId, message.Name, message.Position);
            //Sender.Tell(new ForwardToPlayers(innerMessage));
        }

        private IActorRef SpawnEntity(SpawnEntity message)
        {
            Debug.WriteLine($"{nameof(EntityManager)} Handling SpawnEntity");
            long entityId = NextEntityId();
            IActorRef newEntity = Context.ActorOf(Props.Create<Entity>(entityId, message.Position), message.Name);
            Entities.Add(newEntity);
            EntitiesById.Add(entityId, newEntity);

            IActorRef regionManager = _regionManager.ResolveOne(TimeSpan.FromSeconds(1)).Result;
            Debug.WriteLine($"{Self.Path.ToString()} messaging regionManager ActorRef {regionManager.Path.ToString()}");
            Debug.WriteLine($"{Self.Path.ToString()} messaging regionManager {_regionManager.PathString}");
            _regionManager.Tell(new AddEntityToRegion(entityId, newEntity, message.Position));
            return newEntity;
        }
        
        private long NextEntityId()
        {
            return _nextEntityId++;
        }
    }
}
