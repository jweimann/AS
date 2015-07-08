namespace AS.Messages.Game
{
    public class CreateGame
    {
        public string GameName { get; set; }
        public CreateGame(string gameName)
        {
            GameName = gameName;
        }
    }
}
