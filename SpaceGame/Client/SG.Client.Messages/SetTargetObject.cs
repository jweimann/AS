using AS.Client.Messages;

namespace SG.Client.Messages
{
    [System.Serializable]
    public class SetTargetObject : ClientMessage
    {
        public int TargetActorId { get; private set; }
        public SetTargetObject(int actorId, int targetActorId) : base(actorId, UnityClientActorType.Entity)
        {
            TargetActorId = targetActorId;
        }
    }
}
