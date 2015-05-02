using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class Authenticate
    {
        public Authenticate(string name)
        {
            this.Name = name;
        }
        public string Name { get; private set; }
    }
}
