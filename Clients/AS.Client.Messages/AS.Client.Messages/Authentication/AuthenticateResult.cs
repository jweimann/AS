using System;

namespace AS.Client.Messages
{
    [Serializable]
    public class AuthenticateResult : ClientMessage
    {
        public bool Result { get; private set; }
        public AuthenticateResult(bool result) : base(0, UnityClientActorType.ClientUser)
        {
            Result = result;
        }
    }
}
