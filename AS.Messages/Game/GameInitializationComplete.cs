namespace AS.Messages.Game
{
    public class GameInitializationComplete
    {
        private static GameInitializationComplete _instance;
        public static GameInitializationComplete Instance { get { if (_instance == null) _instance = new GameInitializationComplete(); return _instance; } }
    }
}
