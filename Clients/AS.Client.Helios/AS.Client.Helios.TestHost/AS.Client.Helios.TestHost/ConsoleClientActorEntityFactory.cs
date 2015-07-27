using System;
using AS.Client.Messages;
using AS.Client.Core;
using AS.Client.Unity3D;
using AS.Client.Unity3D.Entities;

namespace AS.Client.Helios.TestHost
{
    public class ConsoleClientActorEntityFactory : ClientActorFactory, IClientActorFactory
    {
        public ConsoleClientActorEntityFactory()
        {
            RegisterActorType<UnityClientUserActor>(UnityClientActorType.ClientUser);
            RegisterActorType<UnityEntityActor>(UnityClientActorType.Entity);
        }

        public override IClientActor CreateActor(ClientMessage message)
        {
            Type type = _clientActorTypes[message.ClientActorType];
            return Activator.CreateInstance(type, new object[] { "NA" }) as ClientActor;
        }

    }
}
