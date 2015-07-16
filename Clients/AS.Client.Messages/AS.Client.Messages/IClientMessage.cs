using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.Client.Messages
{
    public interface IClientMessage
    {
        object GetServerMessage();
    }
}
