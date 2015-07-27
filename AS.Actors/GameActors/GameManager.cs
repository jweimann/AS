using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;
using AS.Messages.Game;
using AS.Messages.SystemStats;
using AS.Client.Messages.Game;

namespace AS.Actors.GameActors
{
    public class GameManager : ReceiveActor
    {
        private List<IActorRef> _games = new List<IActorRef>();
        System.Type _gameType;

        public GameManager(System.Type gameType)
        {
            Debug.WriteLine($"GameManager Spawned: {Self.Path.ToString()}");
            _gameType = gameType;
            Receive<CreateGame>(msg => CreateNewGame(msg));
            Receive<GetSystemStats>(message =>
            {
                foreach (var child in Context.GetChildren())
                    child.Tell(message, Sender);
            });
        }

        private void CreateNewGame(CreateGame msg)
        {
            IActorRef game = Context.ActorOf(Props.Create<Game>(new object[] { msg, _gameType }));
            _games.Add(game);

            game.Tell(new JoinGame(Sender), Sender);
        }
    }
}