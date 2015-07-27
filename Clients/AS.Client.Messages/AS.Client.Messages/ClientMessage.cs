using System;
using AS.Common;

namespace AS.Client.Messages
{
    [Serializable]
    public class ClientMessage
    {
        public ClientMessage(int actorId, UnityClientActorType clientActorType)
        {
            ActorId = actorId;
            Path = actorId.ToString();
            ClientActorType = clientActorType;
        }

        public ClientMessage(int actorId, UnityClientActorType clientActorType, EntityType entityType)
        {
            ActorId = actorId;
            Path = actorId.ToString();
            ClientActorType = clientActorType;
            EntityType = entityType;
        }

        public int ActorId { get; private set; }
        /// <summary>
        /// May be able to do this with just the ActorId and not use a path.
        /// </summary>
        public string Path { get; private set; }

        public UnityClientActorType ClientActorType { get; private set; }
        public EntityType EntityType { get; private set; }
    }
}
