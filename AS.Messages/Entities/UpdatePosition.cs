using System.Numerics;

namespace AS.Messages.Entities
{
    public class UpdatePosition
    {
        public UpdatePosition(long entityId, Vector3 location)
        {
            Position = location;
            EntityId = entityId;
        }

        public long EntityId { get; private set; }
        public Vector3 Position { get; private set; }
    }
}
