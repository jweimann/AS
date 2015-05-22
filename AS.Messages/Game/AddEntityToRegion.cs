using System.Numerics;
using Akka.Actor;

namespace AS.Messages.Game
{
    public class AddEntityToRegion
    {
        public long EntityId { get; private set; }
        public IActorRef EntityActor { get; private set; }
        public Vector3 Position { get; private set; }
        public AddEntityToRegion(long entityId, IActorRef entityActor, Vector3 position)
        {
            EntityId = entityId;
            EntityActor = entityActor;
            Position = position;
        }

        public override string ToString()
        {
            return $"AddEntityToRegion EntityId:{EntityId}";
        }
    }
}
