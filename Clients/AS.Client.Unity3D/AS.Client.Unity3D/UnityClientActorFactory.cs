using AS.Client.Messages;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AS.Client.Unity3D
{
    public static class UnityClientActorFactory
    {
        private static Dictionary<UnityClientActorType, UnityClientMonoActor> _unityClientMonoActorTypes = new Dictionary<UnityClientActorType, UnityClientMonoActor>();
        private static Dictionary<UnityClientActorType, Type> _clientActorTypes = new Dictionary<UnityClientActorType, Type>();

        public static UnityClientMonoActor CreateGameObject(UnityClientActorType type)
        {
            GameObject clientActorGameObject = GameObject.Instantiate(_unityClientMonoActorTypes[type].gameObject);
            UnityClientMonoActor clientActorScript = clientActorGameObject.GetComponent<UnityClientMonoActor>();
            return clientActorScript;
        }

        public static ClientActor Create(UnityClientActorType clientActorType, string path)
        {
            Type type = _clientActorTypes[clientActorType];
            return Activator.CreateInstance(type, new object[] { path }) as ClientActor;
        }

        public static void RegisterActorType(UnityClientActorType clientActorType, UnityClientMonoActor actorDefinition)
        {
            _unityClientMonoActorTypes.Add(clientActorType, actorDefinition);
        }

        public static void RegisterActorType<TClientActor>(UnityClientActorType clientActorType) where TClientActor : ClientActor
        {
            _clientActorTypes.Add(clientActorType, typeof(TClientActor));
        }
    }
}