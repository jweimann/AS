using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Messages
{
    public class AuthenticateResult
    {
        public bool Result { get; private set; }
        public AuthenticateResult(bool result)
        {
            this.Result = result;
        }
    }
}
