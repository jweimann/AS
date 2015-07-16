using System;

namespace AS.Client.Messages.Errors
{
    [Serializable]
    public class NotAuthenticated : ClientMessage
    {
        public NotAuthenticated() : base(0, UnityClientActorType.ClientUser)
        {
        }
    }
}
