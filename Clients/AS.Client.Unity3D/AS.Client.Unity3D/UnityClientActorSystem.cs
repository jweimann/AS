using AS.Client.Core;
using AS.Client.Logging;
using AS.Client.Messages;
using AS.Client.Messages.ClientRequests;
using AS.Client.Messages.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AS.Client.Unity3D
{
    public class UnityClientActorSystem
    {
        private Dictionary<long, IClientActor> _actors;
        private Unity3DClient _client;
        private int _messageSinceLastRead;
        private Dictionary<long, List<ClientMessage>> _stash = new Dictionary<long, List<ClientMessage>>();
        private Queue<ClientMessage> _mailbox = new Queue<ClientMessage>();

        private static UnityClientActorSystem _instance;

        public static UnityClientActorSystem Instance { get { return _instance; } }
        public IClientActorFactory ClientActorEntityFactory { get; set; }

        public string GetStats()
        {
            var result = String.Format("Mailbox Count: {0}\nMessages/Second: {1}",
                _mailbox.Count,
                _messageSinceLastRead);
            _messageSinceLastRead = 0;
            return result;
        }

        public UnityClientActorSystem(IClientActorFactory clientActorEntityFactory)
        {
            if (_instance != null)
                throw new Exception("Only one instance of UnityClientActorSystem can exist at once.");
            _instance = this;

            ClientActorEntityFactory = clientActorEntityFactory;

            Logger.LogDebug("Client Actor System Created");

            // need to decide how to do this, can't find a good way to mix/match gameobject and non gameobject entities, not sure if they should be
            // Pretty sure I want to be able to register non-visible ones in code, but maybe I want to use a monobehavior base (which it's set to now) and force it to be on a gameobject.
            // it could be a new gameobject instead of a prefab, need to figure that part out.
            // definitely need prefabs too though for things like units/characters/etc..s


            _actors = new Dictionary<long, IClientActor>();
            _client = new Unity3DClient();
            _client.MessageReceived += HandleMessageReceived;
            _client.Disconnected += HandleDisconnected;
            _client.Initialize();
            Logger.LogDebug("Client Actor System Initialized");
        }

        private void HandleDisconnected(string reason)
        {
            Console.WriteLine("Disconnected!  Reason: " + reason);
        }

        public void SendMessage(object message)
        {
            _client.SendMessage(message);
            Logger.LogDebug(String.Format("Sent Message of Type: {0}", message));
        }

        private void HandleMessageReceived(object obj)
        {
            //lock(this) // Temporary lock, needs a mailbox style system.
            {
                ClientMessage message = obj as ClientMessage;
                if (message == null)
                {
                    Logger.LogError("ERROR - MESSAGE NOT HANDLED ON CLIENT: " + obj.GetType().ToString());
                    throw new Exception("ERROR - MESSAGE NOT HANDLED ON CLIENT: " + obj.GetType().ToString());
                }
                _mailbox.Enqueue(message);
                _messageSinceLastRead++;
                //Logger.LogWarning("HandleMessageReceived: {0}", message.ToString());
            }
        }

        public void Tick()
        {
            lock (this)
            {
                var messagesToProcess = new List<ClientMessage>();
                while (_mailbox.Count > 0)
                {
                    var msg = _mailbox.Dequeue();
                    if (msg is ClientMessage)
                        messagesToProcess.Add(msg);
                    else if (msg != null)
                        Logger.LogError("Received Non-ClientMessage: " + msg.ToString());
                    else
                        Logger.LogError("Received NULL Message");
                }


                _mailbox.Clear();
                foreach (var message in messagesToProcess)
                {
                    if (message == null)
                    {
                        var nullCount = messagesToProcess.Count(t => t == null);
                        throw new NullReferenceException("Received a NULL message - NullCount: " + nullCount + " NonNullCount: " + (messagesToProcess.Count - nullCount));
                    }

                    RouteMessage(message, message.ActorId);
                    ClientActorSystemStats.LogMessage(message);
                }
            }
        }

        public void RouteMessage(object message, long actorId)
        {
            ClientMessage clientMessage = message as ClientMessage;

            if (_actors.ContainsKey(actorId) == false)
            {
                if (CreateActor(clientMessage) == ClientActorState.RequestedFromServer)
                    return;
            }

            if (_actors.ContainsKey(actorId) == false)
                Logger.LogWarning("Unable to find actor ID: " + actorId);

            if (_actors[actorId] == null)
                Logger.LogWarning("Actor NULL - ID: " + actorId);

            _actors[actorId].Tell(clientMessage);

        }

        private ClientActorState CreateActor(ClientMessage message)
        {
            if (message.ClientActorType == UnityClientActorType.ClientUser || message.GetType() == typeof(EntityDetails))
            {
                IClientActor actor = ClientActorEntityFactory.CreateActor(message);
                _actors.Add(message.ActorId, actor);
                UnstashEntityMessages(message);
                return ClientActorState.Created;
            }
            else
            {
                SendMessage(new ClientRequestEntityDetails(message.ActorId));
                StashEntityMessage(message);
                return ClientActorState.RequestedFromServer;
            }
        }

        private void UnstashEntityMessages(ClientMessage message)
        {
            if (_stash.ContainsKey(message.ActorId))
            {
                foreach (var stashedMessage in _stash[message.ActorId])
                {
                    _mailbox.Enqueue(stashedMessage);
                }
            }
        }

        private void StashEntityMessage(ClientMessage message)
        {
            if (_stash.ContainsKey(message.ActorId) == false)
                _stash.Add(message.ActorId, new List<ClientMessage>());

            _stash[message.ActorId].Add(message);
        }
        private enum ClientActorState
        {
            RequestedFromServer,
            Created
        }
    }
}