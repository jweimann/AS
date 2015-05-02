using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class UserList
    {
        public List<string> Usernames { get; private set; }
        public UserList(List<string> usernames)
        {
            this.Usernames = usernames;
        }
    }
}
