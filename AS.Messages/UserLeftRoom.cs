using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class UserLeftRoom
    {
        public string Username { get; private set; }
        public UserLeftRoom(string username)
        {
            this.Username = username;
        }
    }
}
