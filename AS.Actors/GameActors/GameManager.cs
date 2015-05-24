using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;
using AS.Messages.Game;

namespace AS.Actors.GameActors
{
    public class GameManager : ReceiveActor
    {
        private List<IActorRef> _games = new List<IActorRef>();

        public GameManager()
        {
            Debug.WriteLine($"GameManager Spawned: {Self.Path.ToString()}");
            Receive<CreateGame>(msg => CreateNewGame(msg));
        }

        private void CreateNewGame(CreateGame msg)
        {
            string gameName = "game1";
            IActorRef game = Context.ActorOf(Props.Create<Game>(new object[] { msg }), gameName);
            _games.Add(game);

            Sender.Tell(new JoinGameSuccess(game));
        }
    }
}