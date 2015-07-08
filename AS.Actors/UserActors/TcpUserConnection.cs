using Akka.Actor;
using AS.Interfaces;
using AS.Serialization;
using Helios.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AS.Actors.UserActors
{
    // Converting this from taking IActorRef to taking a connection
    // Instead of converting, add a new actortype and switch out which to spawn based on the 
    // connection source.
    public class TcpUserConnection : UntypedActor, IWithUnboundedStash
    {
        private IActorRef _user;
        private IConnection _connection;
        private Dictionary<int, IActorRef> _actorIdMap;

        public TcpUserConnection(IConnection connection)
        {
            _actorIdMap = new Dictionary<int, IActorRef>();
            _user = Context.Parent;
            _connection = connection;
            Debug.WriteLine("Connection: " + _connection.ToString());
        }

        protected override void OnReceive(object message)
        {
            var senderSelection = Context.ActorSelection(Sender.Path);
            if (Sender.Path != _user.Path)
                ForwardMessageToUser(message);
            else
                ForwardMessageToClient(message);
        }

        private void ForwardMessageToClient(object message)
        {
            if (_connection != null)
            {
                Type t = message.GetType();
                if (t.IsSerializable)
                    SerializeAndSendMessageToRemoteConnection(message);
                else
                    MapAndSendCommandToClient((IMapToClientCommand)message);
            }
        }

        private void MapAndSendCommandToClient(IMapToClientCommand message)
        {
            SerializeAndSendMessageToRemoteConnection(message.GetClientCommand());
        }

        private void SerializeAndSendMessageToRemoteConnection(object message)
        {
            BinarySerializer serializer = new BinarySerializer();
            var bytes = serializer.Serialize(message);
            _connection.Send(new NetworkData() { Buffer = bytes, Length = bytes.Length });
        }

        private void ForwardMessageToUser(object message)
        {
            Debug.WriteLine("Forwarding Message To User: " + message.ToString());
            Console.WriteLine("Forwarding Message To User: " + message.ToString());
            _user.Tell(message);
        }

        public IStash Stash { get; set; }
    }
}