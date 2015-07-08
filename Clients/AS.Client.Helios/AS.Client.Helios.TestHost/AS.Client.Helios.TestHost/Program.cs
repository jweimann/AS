using AS.Client.Messages.Lobby;
using AS.Client.Unity3D;
using System;

namespace AS.Client.Helios.TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            UnityClientActorSystem system = new UnityClientActorSystem();
            while (true)
                System.Threading.Thread.Sleep(20);

            AkkaClient client = new AkkaClient();
            client.MessageReceived += HandleMessageRecieved;
            client.Initialize();

            while (!client.TempConnected)
                System.Threading.Thread.Sleep(20);

            client.SendMessage(new ClientConnectRequest("jweimann", "nopass"));

            while (true)
                System.Threading.Thread.Sleep(20);
        }

        private static void HandleMessageRecieved(object obj)
        {
            
        }
    }
}
