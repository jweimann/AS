using AS.Common;

namespace AS.Client.Messages.Entities
{
    [System.Serializable]
    public class EntityDetails : ClientMessage
    {
        public override Destination Destination { get { return Destination.Client; } }
        public EntityType EntityType { get; private set; }
        public EntityDetails(int actorId, EntityType entityType) : base(actorId, UnityClientActorType.Entity)
        {
            EntityType = entityType;
        }
    }
}
