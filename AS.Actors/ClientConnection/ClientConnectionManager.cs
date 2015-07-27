using Akka.Actor;
using AS.Actors.UserActors;
using AS.Client.Messages.Lobby;
using AS.Messages;
using AS.Serialization;
using Helios.Net;
using Helios.Ops.Executors;
using Helios.Reactor.Bootstrap;
using Helios.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors.ClientConnection
{
    public class ClientConnectionManager : ReceiveActor
    {
        private ActorSelection _users;
        private const TransportType TRANSPORT_TYPE = TransportType.Tcp;

        // This class is not implemented yet, just copied from TimeServiceServer
        public ClientConnectionManager()
        {
            _users = Context.System.ActorSelection("/user/users");

            var host = IPAddress.Any;
            var port = 9991;
            Console.Title = "Server";
            Console.WriteLine("Starting server on {0}:{1}", host, port);
            var executor = new TryCatchExecutor(exception => Console.WriteLine("Unhandled exception: {0}", exception));

            var bootstrapper =
                new ServerBootstrap()
                    .WorkerThreads(2)
                    .Executor(executor)
                    .SetTransport(TRANSPORT_TYPE)
                    .Build();
            var server = bootstrapper.NewReactor(NodeBuilder.BuildNode().Host(host).WithPort(port));
            server.OnConnection += (address, connection) =>
            {
                Console.WriteLine("Connected: {0}", address);
                //_users.Tell(new ConnectionEstablished(connection));
                
                connection.BeginReceive(ReceiveBytes);
            };
            server.OnDisconnection += (reason, address) =>
                Console.WriteLine("Disconnected: {0}; Reason: {1}", address.RemoteHost, reason.Type);
            server.Start();
            Console.WriteLine("Running, press any key to exit");
            Console.ReadKey();
            Console.WriteLine("Shutting down...");
            server.Stop();
            Console.WriteLine("Terminated");
        }


        public void ReceiveBytes(NetworkData data, IConnection channel)
        {
            //channel.Send(new NetworkData() { Buffer = new byte[] { 1 }, Length = 1, RemoteHost = data.RemoteHost });
            channel.StopReceive();
            BinarySerializer serializer = new BinarySerializer();
            object request = serializer.Deserialize<object>(data.Buffer);
            Console.WriteLine(request);
            if (request is ClientConnectRequest)
            {
                HandleClientConnectionRequest((ClientConnectRequest)request, channel, data.RemoteHost);
            }
            //channel.Send(data);
        }

        private void HandleClientConnectionRequest(ClientConnectRequest request, IConnection channel, INode remoteHost)
        {
            _users.Tell(new ConnectionEstablished(channel, remoteHost));
        }

        public static void Receive(NetworkData data, IConnection channel)
        {
            var command = Encoding.UTF8.GetString(data.Buffer);

            //Console.WriteLine("Received: {0}", command);
            if (command.ToLowerInvariant() == "gettime")
            {
                var time = Encoding.UTF8.GetBytes(DateTime.Now.ToLongTimeString());
                channel.Send(new NetworkData() { Buffer = time, Length = time.Length, RemoteHost = channel.RemoteHost });
                //Console.WriteLine("Sent time to {0}", channel.Node);
            }
            else
            {
                Console.WriteLine("Invalid command: {0}", command);
                var invalid = Encoding.UTF8.GetBytes("Unrecognized command");
                channel.Send(new NetworkData() { Buffer = invalid, Length = invalid.Length, RemoteHost = channel.RemoteHost });
            }
        }
    }
}
