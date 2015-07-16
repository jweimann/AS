using UnityEngine;

namespace AS.Messages.Entities
{
    public class SetPosition
    {
        public long EntityId { get; private set; }
        public AS.Common.Vector3 Position { get; private set; }

        public SetPosition(AS.Common.Vector3 position, long entityId)
        {
            Position = position;
            EntityId = entityId;
        }
    }
}