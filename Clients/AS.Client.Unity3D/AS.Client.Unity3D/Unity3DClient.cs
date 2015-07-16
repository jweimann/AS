using System;
using AS.Client.Helios;
using AS.Client.Messages.Lobby;

namespace AS.Client.Unity3D
{
    public class Unity3DClient : AkkaClient
    {
        public Unity3DClient()
        {
            

            //while (!TempConnected)
            //    System.Threading.Thread.Sleep(20);

            //SendMessage(new ClientConnectRequest("jweimann", "nopass")); // Authenticate message is used instead..

        }
    }
}
