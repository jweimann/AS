using UnityEngine;

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
        public SpawnEntity(long entityId, string name, Vector3 position) : base(entityId)
        {
            Name = name;
            Position = position;
        }
    }
}
