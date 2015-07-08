//using UnityEngine;

using UnityEngine;

namespace AS.Client.Core
{
    /// <summary>
    /// Base Implementation.
    /// Each client will have it's own type to deal with any binding/etc it needs.
    /// </summary>
    public abstract class ClientEntity
    {
        protected long _entityId;
        protected Vector3 _position;
        public virtual long EntityId { get { return _entityId; } protected set { _entityId = value; } }
        public virtual Vector3 Position { get { return _position; } set { _position = value; } }

        public ClientEntity(long entityId)
        {
            EntityId = entityId;
        }
    }
}
