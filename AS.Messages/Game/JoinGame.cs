using System;
using Akka.Actor;
using AS.Interfaces;
using AS.Client.Messages.Game;

namespace AS.Messages.Game
{
    public class JoinGame : IMapToClientCommand
    {
        public IActorRef ActorRef { get; private set; }
        public JoinGame(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }

        public object GetClientCommand(int entityId)
        {
            return new ClientJoinGameRequest(ActorRef.Path.ToString());
        }
    }
}
