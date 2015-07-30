namespace AS.Client.Messages.ClientRequests
{
    [System.Serializable]
    public class ClientRequestEntityDetails : ClientMessage
    {
        public ClientRequestEntityDetails(int actorId) : base(actorId, UnityClientActorType.Entity)
        {
        }
    }
}
