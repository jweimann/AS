using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace AS.Client.Helios.Tests
{
    [TestClass]
    public class HeliosAkkaClientIntegrationTests
    {
        [TestMethod]
        public void akkaClient_initialize_attemptsConnection()
        {
            AkkaClient client = new AkkaClient();
            client.Initialize();
            while (!client.TempConnected)
                Thread.Sleep(10);

            Assert.IsNotNull(client);
        }
    }
}
