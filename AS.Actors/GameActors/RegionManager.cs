using System.Diagnostics;
using UnityEngine;
using Akka.Actor;
using AS.Common;
using AS.Messages.Entities;

namespace AS.Actors
{
    public class RegionManager : ReceiveActor
    {
        private float _size = 100.0f;
        private int _entitiesPerRegion = 3;
        private IActorRef _rootRegion;
        public RegionManager(float size, int entitiesPerRegion)
        {
            _size = size;
            _entitiesPerRegion = entitiesPerRegion;

            System.Diagnostics.Debug.WriteLine($"RegionManager: {Self.Path.ToString()}");
            Props rootRegionProps = Props.Create<Region>(new object[] { new Common.Bounds(Common.Vector3.zero, Common.Vector3.one * _size), _entitiesPerRegion });
            _rootRegion = Context.ActorOf(rootRegionProps, "RootRegion");
            System.Diagnostics.Debug.WriteLine($"RootRegion: {_rootRegion.Path.ToSerializationFormat()}");

            Receive<SpawnEntity>(message =>
            {
                System.Diagnostics.Debug.WriteLine($"Failed to add entity to a region.  EntityId: {message.EntityId}");
            });

            ReceiveAny(message => _rootRegion.Tell(message));
        }
    }
}
