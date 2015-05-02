using Akka.Actor;
namespace AS.Messages
{
    public class UserCreated
    {
        public IActorRef UserActor { get; private set; }
        public IActorRef UserConnectionActor { get; private set; }
        public UserCreated(IActorRef userActor, IActorRef userConnectionActor)
        {
            this.UserActor = userActor;
            this.UserConnectionActor = userConnectionActor;
        }
    }
}