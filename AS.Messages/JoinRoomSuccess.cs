using Akka.Actor;
namespace AS.Messages
{
    public class JoinRoomSuccess
    {
        public JoinRoomSuccess(IActorRef roomActor)
        {
            this.RoomActor = roomActor;
        }
        public IActorRef RoomActor { get; private set; }
    }
}