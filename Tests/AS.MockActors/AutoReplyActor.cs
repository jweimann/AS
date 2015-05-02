﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.MockActors
{
    public class AutoReplyActor : UntypedActor
    {

        protected override void OnReceive(object message)
        {
            Sender.Tell(message);
        }
    }
}
