using AS.Admin.ChatClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ServerStatsClient.ViewModels
{
    public class ServerStatsViewModel : ViewModelBase
    {
        private int _roomCount;
        public int RoomCount { get { return _roomCount; } set { _roomCount = value; OnPropertyChanged(() => RoomCount); } }
    }
}
