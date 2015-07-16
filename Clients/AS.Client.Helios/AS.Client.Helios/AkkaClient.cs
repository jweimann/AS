using AS.Client.Messages.ClientRequests;
using AS.Client.Messages.Lobby;
using AS.Serialization;
using Helios.Exceptions;
using Helios.Net;
using Helios.Net.Bootstrap;
using Helios.Topology;

using System;
using System.Net;
using System.Text;
using System.Threading;

namespace AS.Client.Helios
{
    public class AkkaClient : IDisposable
    {
        private const int DEFAULT_PORT = 9991;
        private const TransportType TRANSPORT_TYPE = TransportType.Udp;

        private static int Port;

        public INode RemoteHost;
        public IConnection Connection;

        public bool TempConnected { get; set; }

        public Action<object> MessageReceived { get; set; }
        public Action<string> Disconnected { get; set; }

        public void SendMessage(object message)
        {
            Connection.Send(GetNetworkData(message));
        }

        private NetworkData GetNetworkData(object message)
        {
            BinarySerializer serializer = new BinarySerializer();

            var bytes = serializer.Serialize(message);

            //var bytes = Encoding.Default.GetBytes(v);
            return new NetworkData()
            {
                Buffer = bytes,
                Length = bytes.Length,
                RemoteHost = this.RemoteHost
            };
        }

        private static void ServerPrint(INode node, string message)
        {
            Console.WriteLine("[{0}] {1}:{2}: {3}", DateTime.UtcNow, node.Host, node.Port, message);
        }

        public void Initialize()
        {
            Thread.Sleep(1000);
            while (TryConnect() == false)
                Thread.Sleep(100);
        }

        public bool TryUdpConnect()
        {
            Connection =
                  new ClientBootstrap()
                      .SetTransport(TransportType.Udp)
                      .RemoteAddress(Node.Loopback())
                      .OnConnect(ConnectionEstablishedCallback)
                      .OnReceive(ReceivedDataCallback)
                      .OnDisconnect(ConnectionTerminatedCallback)
                      .Build().NewConnection(NodeBuilder.BuildNode().Host(IPAddress.Any).WithPort(10002), RemoteHost);
            Connection.OnError += ConnectionOnOnError;
            Connection.Open();

            RemoteHost = NodeBuilder.BuildNode().Host(IPAddress.Loopback).WithPort(DEFAULT_PORT).WithTransportType(TransportType.Udp);

            SendMessage(new ClientConnectRequest("jasons", "jasonspass"));

            return true;
        }

        public bool TryConnect()
        {
            if (TRANSPORT_TYPE == TransportType.Udp)
                return TryUdpConnect();

            Connection =
                  new ClientBootstrap()
                      .SetTransport(TransportType.Tcp)
                      .RemoteAddress(Node.Loopback())
                      .OnConnect(ConnectionEstablishedCallback)
                      .OnReceive(ReceivedDataCallback)
                      .OnDisconnect(ConnectionTerminatedCallback)
                      .Build().NewConnection(NodeBuilder.BuildNode().Host(IPAddress.Loopback).WithPort(DEFAULT_PORT));
            Connection.OnError += ConnectionOnOnError;

            try
            {
                Connection.Open();
            }
            catch (HeliosConnectionException ex)
            {
                switch (ex.Type)
                {
                    case ExceptionType.NotOpen:
                        return false;
                    default:
                        AppendStatusText(ex.ToString());
                        return false;
                }
            }

            return true;
        }

        private void AppendStatusText(string text)
        {
            Console.WriteLine(text);
        }

        public bool AttemptConnect(string host, string portStr)
        {
            try
            {
                var port = Int32.Parse(portStr);
                RemoteHost = NodeBuilder.BuildNode().Host(host).WithPort(port).WithTransportType(TransportType.Udp);

                return true;
            }
            catch (Exception ex)
            {
                AppendStatusText(ex.Message);
                AppendStatusText(ex.StackTrace);
                AppendStatusText(ex.Source);
                return false;
            }
        }

        private void ConnectionTerminatedCallback(HeliosConnectionException reason, IConnection closedChannel)
        {
            AppendStatusText(string.Format("Disconnected from {0}", closedChannel.RemoteHost));
            AppendStatusText(string.Format("Reason: {0}", reason.Message));
            Disconnected(reason.Message);
        }

        private void ReceivedDataCallback(NetworkData incomingData, IConnection responseChannel)
        {
            //AppendStatusText(string.Format("Received {0} bytes from {1}", incomingData.Length,
            //    incomingData.RemoteHost));
            //AppendStatusText(Encoding.UTF8.GetString(incomingData.Buffer));

            BinarySerializer serializer = new BinarySerializer();
            var message = serializer.Deserialize<object>(incomingData.Buffer);
            MessageReceived(message);
        }

        private void ConnectionOnOnError(Exception exception, IConnection connection)
        {
            AppendStatusText(string.Format("Exception {0} sending data to {1}", exception.Message, connection.RemoteHost));
            AppendStatusText(exception.StackTrace);
        }

        private void ConnectionEstablishedCallback(INode remoteAddress, IConnection responseChannel)
        {
            AppendStatusText(string.Format("Connected to {0}", remoteAddress));
            responseChannel.BeginReceive();
            TempConnected = true;

            SendMessage(new ClientConnectRequest("jason", "jasonspass"));
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
