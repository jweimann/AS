using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class CreateTrackedEntity
    {
        public string Name { get; private set; }
        public CreateTrackedEntity(string name)
        {
            this.Name = name;
        }
    }
}
