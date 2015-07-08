namespace AS.Messages.SystemStats
{
    public class SystemStats
    {
        public SystemStats(int roomCount, int gameCount)
        {
            RoomCount = roomCount;
            GameCount = gameCount;
        }

        public int GameCount { get; private set; }
        public int RoomCount { get; private set; }
    }
}
