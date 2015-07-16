using AS.Client.Core;
using AS.Client.Messages;
using AS.Client.Unity3D.Entities;
using System;
using System.Collections.Generic;

namespace AS.Client.Unity3D
{
    public class UnityClientActorSystem
    {
        private Dictionary<string, IClientActor> _actors;
        private Unity3DClient _client;

        private static UnityClientActorSystem _instance;
        public static UnityClientActorSystem Instance { get { return _instance; } }

        public UnityClientActorSystem()
        {
            if (_instance != null)
                throw new Exception("Only one instance of UnityClientActorSystem can exist at once.");
            _instance = this;

            // need to decide how to do this, can't find a good way to mix/match gameobject and non gameobject entities, not sure if they should be
            // Pretty sure I want to be able to register non-visible ones in code, but maybe I want to use a monobehavior base (which it's set to now) and force it to be on a gameobject.
            // it could be a new gameobject instead of a prefab, need to figure that part out.
            // definitely need prefabs too though for things like units/characters/etc..s
            UnityClientActorFactory.RegisterActorType<UnityClientUserActor>(UnityClientActorType.ClientUser);
            UnityClientActorFactory.RegisterActorType<UnityEntityActor>(UnityClientActorType.Entity);

            _actors = new Dictionary<string, IClientActor>();
            _client = new Unity3DClient();
            _client.MessageReceived += HandleMessageReceived;
            _client.Disconnected += HandleDisconnected;
            _client.Initialize();
        }

        private void HandleDisconnected(string reason)
        {
            Console.WriteLine("Disconnected!  Reason: " + reason);
        }

        public void SendMessage(object message)
        {
            _client.SendMessage(message);
        }

        private void HandleMessageReceived(object obj)
        {
            lock(this) // Temporary lock, needs a mailbox style system.
            {
                ClientMessage message = obj as ClientMessage;
                if (message == null)
                {
                    Console.WriteLine("ERROR - MESSAGE NOT HANDLED ON CLIENT: " + obj.GetType().ToString());
                    throw new Exception("ERROR - MESSAGE NOT HANDLED ON CLIENT: " + obj.GetType().ToString());
                }
                RouteMessage(message, message.Path);
                ClientActorSystemStats.LogMessage(message);
            }
        }

        public void RouteMessage(object message, string path)
        {
            if (_actors.ContainsKey(path) == false)
                CreateActor(message as ClientMessage);
            _actors[path].Tell(message);
        }

        /// <summary>
        /// This needs to know what type of actor to instantiate.
        /// Can request it from the server or always include a byte with the type.
        /// </summary>
        /// <param name="path"></param>
        private void CreateMonoActor(ClientMessage message)
        {
            UnityClientMonoActor actor = UnityClientActorFactory.CreateGameObject(message.ClientActorType);
            _actors.Add(actor.Path, actor);
        }

        private void CreateActor(ClientMessage message)
        {
            ClientActor actor = UnityClientActorFactory.Create(message.ClientActorType, message.Path);
             _actors.Add(actor.Path, actor);
        }
    }
}
