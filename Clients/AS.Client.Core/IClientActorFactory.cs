using AS.Client.Messages;

namespace AS.Client.Core
{
    public interface IClientActorFactory
    {
        IClientActor CreateActor(ClientMessage message);
        void RegisterActorType<TClientActor>(UnityClientActorType clientActorType) where TClientActor : IClientActor;
    }
}
