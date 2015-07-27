using AS.Client.Messages;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AS.Client.Logging;
using AS.Client.Core;
using AS.Client.Unity3D.Entities;

namespace AS.Client.Unity3D
{
    public class UnityClientActorFactory : IClientActorFactory
    {
        private Dictionary<AS.Common.EntityType, UnityClientMonoActor> _unityClientMonoActorTypes = new Dictionary<AS.Common.EntityType, UnityClientMonoActor>();
        private Dictionary<UnityClientActorType, Type> _clientActorTypes = new Dictionary<UnityClientActorType, Type>();

        public UnityClientActorFactory()
        {
            RegisterActorType<UnityClientUserActor>(UnityClientActorType.ClientUser);
            RegisterActorType<UnityEntityActor>(UnityClientActorType.Entity);
        }

        public void SetClientMonoActors(UnityClientMonoActor[] clientMonoActors)
        {
            _unityClientMonoActorTypes = new Dictionary<AS.Common.EntityType, UnityClientMonoActor>();
            foreach (var clientMonoActor in clientMonoActors)
            {
                UnityEngine.Debug.LogFormat("Registring ClientMonoActor: {0} for Type: {1}", clientMonoActor.name, clientMonoActor.EntityType.ToString());
                Logger.LogDebug("Registring ClientMonoActor: {0} for Type: {1}", clientMonoActor.name, clientMonoActor.EntityType.ToString());
                _unityClientMonoActorTypes.Add(clientMonoActor.EntityType, clientMonoActor);
                Logger.LogDebug("Registered ClientMonoActor: {0}", clientMonoActor.name);
            }
        }

        public UnityClientMonoActor CreateGameObject(Common.EntityType type)
        {
            Logger.LogDebug("Creating GameObject of Type: {0}", type.ToString());
            Logger.LogDebug("Dictionary Info - Length: {0}  First: {1} / {2}",
                _unityClientMonoActorTypes.Count.ToString(),
                _unityClientMonoActorTypes.First().Value.gameObject.name,
                _unityClientMonoActorTypes.First().Key.ToString());

            if (_unityClientMonoActorTypes.ContainsKey(type) == false)
            {
                Logger.LogDebug("ERROR: MonoActorTypeNotFound - Type: {0}", type.ToString());
                return null;
            }

            UnityClientMonoActor prefab = _unityClientMonoActorTypes[type];
            Logger.LogDebug("Prefab: {0}", prefab.ToString());
            Logger.LogDebug("Prefab GameObject: {0}", prefab.gameObject.ToString());

            GameObject clientActorGameObject = GameObject.Instantiate(_unityClientMonoActorTypes[type].gameObject);
            
            UnityClientMonoActor clientActorScript = clientActorGameObject.GetComponent<UnityClientMonoActor>();
            return clientActorScript;
        }

        public ClientActor Create(UnityClientActorType clientActorType)
        {
            Type type = _clientActorTypes[clientActorType];
            return Activator.CreateInstance(type, new object[] { "NA" }) as ClientActor;
        }

        public void RegisterActorType(Common.EntityType entityType, UnityClientMonoActor actorDefinition)
        {
            _unityClientMonoActorTypes.Add(entityType, actorDefinition);
        }

        public void RegisterActorType<TClientActor>(UnityClientActorType clientActorType) where TClientActor : IClientActor
        {
            _clientActorTypes.Add(clientActorType, typeof(TClientActor));
        }

        public IClientActor CreateActor(ClientMessage message)
        {
            if (message.ClientActorType == UnityClientActorType.ClientUser)
                return Create(message.ClientActorType);
            else
            {
                var monoActor = CreateGameObject(message.EntityType);
                monoActor.gameObject.name = String.Format("Entity_{0}", message.ActorId);
                return monoActor;
            }
        }
    }
}