using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AS.Messages.Game
{
    public class JoinGame
    {
        public IActorRef ActorRef { get; private set; }
        public JoinGame(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }
    }
}
