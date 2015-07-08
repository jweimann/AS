using AS.Client.Messages;
using System;
using System.Collections.Generic;

namespace AS.Client.Unity3D
{
    public class UnityClientActorSystem
    {
        private Dictionary<string, UnityClientActor> _actors;
        private Unity3DClient _client;

        public UnityClientActorSystem()
        {
            // need to decide how to do this, can't find a good way to mix/match gameobject and non gameobject entities, not sure if they should be
            // Pretty sure I want to be able to register non-visible ones in code, but maybe I want to use a monobehavior base (which it's set to now) and force it to be on a gameobject.
            // it could be a new gameobject instead of a prefab, need to figure that part out.
            // definitely need prefabs too though for things like units/characters/etc..
            UnityClientActorFactory.RegisterActorType(UnityClientActorType.ClientUser, ) 

            _actors = new Dictionary<string, UnityClientActor>();
            _client = new Unity3DClient();
            _client.MessageReceived += HandleMessageReceived;
            _client.Initialize();
        }

        private void HandleMessageReceived(object obj)
        {
            ClientMessage message = obj as ClientMessage;
            RouteMessage(message, message.Path);
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
        private void CreateActor(ClientMessage message)
        {
            UnityClientActor actor = UnityClientActorFactory.Create(message.ClientActorType);
            _actors.Add(actor.Path, actor);
        }
    }
}
