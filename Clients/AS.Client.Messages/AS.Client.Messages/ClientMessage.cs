using System;
using AS.Common;

namespace AS.Client.Messages
{
    public enum Destination
    {
        Unknown,
        Server,
        Client,
    }

    [Serializable]
    public class ClientMessage
    {
        public ClientMessage(int actorId, UnityClientActorType clientActorType)
        {
            ActorId = actorId;
            Path = actorId.ToString();
            ClientActorType = clientActorType;
        }

        public virtual Destination Destination { get; }

        public int ActorId { get; private set; }
        /// <summary>
        /// May be able to do this with just the ActorId and not use a path.
        /// </summary>
        public string Path { get; private set; }

        public UnityClientActorType ClientActorType { get; private set; }
    }
}
