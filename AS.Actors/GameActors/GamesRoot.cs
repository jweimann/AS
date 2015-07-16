using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;
using AS.Messages.Game;
using AS.Client.Messages.Game;

namespace AS.Actors.GameActors
{
    public class GamesRoot : ReceiveActor
    {
        private List<IActorRef> _gameManagers = new List<IActorRef>();
        public GamesRoot()
        {
            _gameManagers.Add(Context.ActorOf<GameManager>("GameManager1"));
            Debug.WriteLine($"GamesRoot Spawned: {Self.Path.ToString()}");
            Receive<CreateGame>(msg => CreateNewGame(msg));
        }

        private void CreateNewGame(CreateGame msg)
        {
            _gameManagers[0].Tell(msg, Sender);
        }
    }
}
