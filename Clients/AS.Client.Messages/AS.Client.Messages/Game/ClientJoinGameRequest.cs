namespace AS.Client.Messages.Game
{
    [System.Serializable]

    public class ClientJoinGameRequest : ClientMessage
    {
        public string GameName { get; private set; }
        public ClientJoinGameRequest(string gameName) : base(0, UnityClientActorType.ClientUser)
        {
            GameName = gameName;
        }
    }
}
