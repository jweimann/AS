using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.Client.Unity3D
{
    public interface IClientActor
    {
        void Tell(object message);
    }
}
