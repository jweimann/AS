using AS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class ConnectionEstablished
    {
        public IConnection Connection { get; private set; }
        public ConnectionEstablished(IConnection connection)
        {
            if (connection == null)
                throw new NullReferenceException("Connection can not be null");
            this.Connection = connection;
        }

    }
}
