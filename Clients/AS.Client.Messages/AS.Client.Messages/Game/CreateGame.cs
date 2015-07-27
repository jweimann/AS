namespace AS.Client.Messages.Game
{
    [System.Serializable]
    public class CreateGame
    {
        public string GameName { get; private set; }
        public CreateGame(string gameName)
        {
            GameName = gameName;
        }
    }
}
