using AS.Common;

namespace AS.Messages.Entities
{
    /// <summary>
    /// Sent to User.  Forwarded to EntityManager
    /// Handled by EntityManager
    /// </summary>
    public class SpawnEntity : EntityMessage
    {
        public EntityType EntityType { get; private set; }
        public Vector3 Position { get; private set; }
        public int Count { get; private set; }
        public SpawnEntity(long entityId, EntityType entityType, Vector3 position, int count) : base(entityId)
        {
            EntityType = entityType;
            Position = position;
            Count = count;
        }
    }
}
