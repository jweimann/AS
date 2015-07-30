using System;
using System.Collections.Generic;
using AS.Client.Messages;

namespace AS.Client.Core
{
    public abstract class ClientActorFactory : IClientActorFactory
    {
        protected Dictionary<UnityClientActorType, Type> _clientActorTypes = new Dictionary<UnityClientActorType, Type>();

        public abstract IClientActor CreateActor(ClientMessage message);

        public void RegisterActorType<TClientActor>(UnityClientActorType clientActorType) where TClientActor : IClientActor
        {
            _clientActorTypes.Add(clientActorType, typeof(TClientActor));
        }
    }
}