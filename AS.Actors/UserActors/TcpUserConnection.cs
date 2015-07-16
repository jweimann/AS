using Akka.Actor;
using AS.Interfaces;
using AS.Serialization;
using Helios.Net;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using AS.Client.Messages;
using AS.Messages;
using Helios.Topology;

namespace AS.Actors.UserActors
{
    // Converting this from taking IActorRef to taking a connection
    // Instead of converting, add a new actortype and switch out which to spawn based on the 
    // connection source.
    public class TcpUserConnection : UntypedActor, IWithUnboundedStash
    {
        private IActorRef _user;
        private IConnection _connection;
        private Dictionary<int, ActorPath> _actorIdToRefMap;
        private IActorRef _self;
        private INode _remoteHost;

        public TcpUserConnection(ConnectionEstablished connectionEstablishedMessage)
        {
            _connection = connectionEstablishedMessage.Connection;
            _remoteHost = connectionEstablishedMessage.RemoteHost;

            _actorIdToRefMap = new Dictionary<int, ActorPath>();
            _user = Context.Parent;
            _connection.BeginReceive(ReceiveBytes);
            _connection.Send(NetworkData.Empty);
            _self = Self;
            //_connection.Receive += _connection_Receive;
            Debug.WriteLine("Connection: " + _connection.ToString());
        }

        private void ReceiveBytes(NetworkData incomingData, IConnection responseChannel)
        {
            BinarySerializer serializer = new BinarySerializer();
            var message = serializer.Deserialize<object>(incomingData.Buffer);
            Console.WriteLine($"ReceiveBytes {message.ToString()}");
            _user.Tell(message, _self);
        }

        private int GetSenderEntityId()
        {
            if (_actorIdToRefMap.ContainsValue(Sender.Path))
                return _actorIdToRefMap.FirstOrDefault(t => t.Value == Sender.Path).Key;

            _actorIdToRefMap.Add(_nextEntityId++, Sender.Path);
            return _nextEntityId;
        }

        private int _nextEntityId = 0;

        protected override void OnReceive(object message)
        {
            //Console.WriteLine($"UserConnection Received message of type: " + message.GetType().ToString());
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
                //Console.WriteLine($"Sending message to client of type: " + t.ToString());
                if (t.IsSerializable)
                    SerializeAndSendMessageToRemoteConnection(message);
                else
                    MapAndSendCommandToClient((IMapToClientCommand)message);
            }
        }

        private void MapAndSendCommandToClient(IMapToClientCommand message)
        {
            SerializeAndSendMessageToRemoteConnection(message.GetClientCommand(GetSenderEntityId()));
        }

        private void SerializeAndSendMessageToRemoteConnection(object message)
        {
            BinarySerializer serializer = new BinarySerializer();
            var bytes = serializer.Serialize(message);
            _connection.Send(new NetworkData() { Buffer = bytes, Length = bytes.Length, RemoteHost = _remoteHost });
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