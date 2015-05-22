namespace AS.Messages.Entities
{
    public class EntityMessage
    {
        public long EntityId { get; private set; }

        public EntityMessage(long entityId)
        {
            EntityId = entityId;
        }
    }
}
