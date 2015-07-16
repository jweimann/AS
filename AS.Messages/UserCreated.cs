using Akka.Actor;
using AS.Interfaces;
using AS.Client.Messages.User;

namespace AS.Messages
{
    public class UserCreated : IMapToClientCommand
    {
        public IActorRef UserActor { get; private set; }
        public IActorRef UserConnectionActor { get; private set; }

        public UserCreated(IActorRef userActor, IActorRef userConnectionActor)
        {
            UserActor = userActor;
            UserConnectionActor = userConnectionActor;
        }

        public object GetClientCommand(int entityId)
        {
            return new ClientUserCreated(entityId);
        }
    }

   
}