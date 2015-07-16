using AS.Common;

namespace AS.Messages.Entities
{
    /// <summary>
    /// Sent to User.  Forwarded to EntityManager
    /// Handled by EntityManager
    /// </summary>
    public class SpawnEntity : EntityMessage
    {
        public string Name { get; private set; }
        public Vector3 Position { get; private set; }
        public int Count { get; private set; }
        public SpawnEntity(long entityId, string name, Vector3 position, int count) : base(entityId)
        {
            Name = name;
            Position = position;
            Count = count;
        }
    }
}
