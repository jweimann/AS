using System;

namespace AS.Client.Messages.User
{
    [Serializable]
    public class ClientUserCreated : ClientMessage
    {
        public ClientUserCreated(int actorId) : base(actorId, UnityClientActorType.ClientUser)
        {
            
        }
    }
}
