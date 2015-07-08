namespace AS.Messages.SystemStats
{
    public class GameStats
    {
        public GameStats(GameState gameState)
        {
            this.GameState = gameState;
        }

        public GameState GameState { get; private set; }
    }

    public enum GameState
    {
        NotStarted,
        Playing
    }
}
