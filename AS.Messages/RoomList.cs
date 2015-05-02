using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class RoomList
    {
        public List<string> Rooms { get; private set; }
        public RoomList(List<string> rooms)
        {
            this.Rooms = rooms;
        }
    }
}
