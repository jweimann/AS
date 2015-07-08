using AS.Client.Messages;
using System.Collections.Generic;
using UnityEngine;

namespace AS.Client.Unity3D
{
    public static class UnityClientActorFactory
    {
        private static Dictionary<UnityClientActorType, UnityClientActor> _clientActorTypes = new Dictionary<UnityClientActorType, UnityClientActor>();

        public static UnityClientActor Create(UnityClientActorType type)
        {
            GameObject clientActorGameObject = GameObject.Instantiate(_clientActorTypes[type].gameObject);
            UnityClientActor clientActorScript = clientActorGameObject.GetComponent<UnityClientActor>();
            return clientActorScript;
        }

        public static void RegisterActorType(UnityClientActorType type, UnityClientActor actorDefinition)
        {
            _clientActorTypes.Add(type, actorDefinition);
        }
    }
}
