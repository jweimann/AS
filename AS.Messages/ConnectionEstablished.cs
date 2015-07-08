using AS.Interfaces;
using Helios.Net;
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
        public ConnectionEstablished(object connection)
        {
            if (connection is IASConnection)
                SetConnection((IASConnection)connection);
            else if (connection is IConnection)
                SetConnection((IConnection)connection);
        }

        private void SetConnection(IConnection connection)
        {
            if (connection == null)
                throw new NullReferenceException("Connection can not be null");
            this.Connection = connection;
        }


        public IASConnection MockConnection { get; private set; }
        private void SetConnection(IASConnection connection)
        {
            if (connection == null)
                throw new NullReferenceException("Connection can not be null");
            this.MockConnection = connection;
        }

    }
}
