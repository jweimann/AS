namespace AS.Admin.ServerStatsClient.Controllers
{
    internal class SystemStats
    {
        public int RoomCount { get; private set; }
        public SystemStats(int roomCount)
        {
            RoomCount = roomCount;
        }
    }
}