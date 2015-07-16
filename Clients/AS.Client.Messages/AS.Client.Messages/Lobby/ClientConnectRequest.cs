using System;

namespace AS.Client.Messages.Lobby
{
    /// <summary>
    /// Not sure this is needed, think i'm just using authenticate request instead.
    /// </summary>
    [Serializable]
    public class ClientConnectRequest
    {
        public string AccountName { get; set; }
        public string AccountPassword { get; set; }

        public ClientConnectRequest(string accountName, string accountPassword)
        {
            AccountName = accountName;
            AccountPassword = accountPassword;
        }
    }
}
