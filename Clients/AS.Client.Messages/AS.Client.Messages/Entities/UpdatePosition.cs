namespace AS.Client.Messages.Entities
{
    [System.Serializable]
    public class UpdatePosition : ClientMessage
    {
        public UpdatePosition(int entityId, Common.Vector3 location, Common.Vector3 velocity) : base(entityId, UnityClientActorType.Entity)
        {
            Position = location;
            EntityId = entityId;
            Velocity = velocity;
        }

        public int EntityId { get; private set; }
        public Common.Vector3 Position { get; private set; }
        public Common.Vector3 Velocity { get; private set; }
    }
}
