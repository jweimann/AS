using AS.Client.Core;
using AS.Client.Logging;
using AS.Client.Messages;
using AS.Client.Messages.Lobby;
using AS.Client.Unity3D;
using AS.Common;
using System;

namespace AS.Client.Helios.TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetLogger(LogLevel.Debug, (text) => { Console.WriteLine(text); });
            Logger.SetLogger(LogLevel.Warning, (text) => { Console.WriteLine(text); });
            Logger.SetLogger(LogLevel.Error, (text) => { Console.WriteLine(text); });
            UnityClientActorSystem system = new UnityClientActorSystem(new ConsoleClientActorEntityFactory() );
            //system.SendMessage(new ClientConnectRequest("jweimann", "nopass"));

            for (int i = 0; i < 100; i++)
            {
                system.Tick();
                System.Threading.Thread.Sleep(20);

            }

            system.SendMessage(new ClientSpawnEntityRequest(EntityType.Asteroid, 100));

            while (true)
            {
                system.Tick();
                System.Threading.Thread.Sleep(20);
            }

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
