using AS.Common;

namespace AS.Messages.Entities
{
    public class RequestEntityPosition
    {
        public int EntityId { get; private set; }
        public RequestEntityPosition(int entityId)
        {
            EntityId = entityId;
        }
    }

    public class EntityPositionResponse
    {
        public int EntityId { get; private set; }
        public Vector3 Position { get; private set; }

        public EntityPositionResponse(int entityId, Vector3 position)
        {
            EntityId = entityId;
            Position = position;
        }
    }

    public class EntityNotFoundResponse
    {
        public int EntityId { get; private set; }
        public EntityNotFoundResponse(int entityId)
        {
            EntityId = entityId;
        }
    }
}
