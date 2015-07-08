using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.Client.Messages.Lobby
{
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
