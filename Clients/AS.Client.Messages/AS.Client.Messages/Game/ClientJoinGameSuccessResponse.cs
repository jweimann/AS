namespace AS.Client.Messages.Game
{
    [System.Serializable]
    public class ClientJoinGameSuccessResponse : ClientMessage
    {
        public string GameName { get; private set; }

        public ClientJoinGameSuccessResponse(string gameName) : base(0, UnityClientActorType.ClientUser)
        {
            GameName = gameName;
        }
    }
}
