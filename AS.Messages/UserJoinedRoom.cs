using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class UserJoinedRoom
    {
        public string Username { get; private set; }
        public UserJoinedRoom(string username)
        {
            this.Username = username;
        }
    }
}
