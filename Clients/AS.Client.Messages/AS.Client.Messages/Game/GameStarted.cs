namespace AS.Client.Messages.Game
{
    [System.Serializable]
    public class GameStarted : ClientMessage
    {
        public GameStarted() : base(0, UnityClientActorType.ClientUser)
        {
        }
    }
}
