using System;

namespace AS.Client.Messages.ClientRequests
{
    [Serializable]
    public class ClientAuthenticateRequest
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public ClientAuthenticateRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
