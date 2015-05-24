namespace AS.Messages.SystemStats
{
    public class SystemStats
    {
        public SystemStats(int roomCount)
        {
            RoomCount = roomCount;
        }

        public int RoomCount { get; private set; }
    }
}
