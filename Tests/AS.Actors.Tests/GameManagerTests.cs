using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Akka.Actor;
using AS.Actors.GameActors;
using AS.Messages.Game;
using Xunit;

namespace AS.Actors.Tests
{
    public class GameManagerTests : ASTestBase
    {
        [Fact]
        public void gameRoot_startup_spawnsManager()
        {
            //ActorOf<GamesRoot>("GamesRoot");
            var gameSelection = ActorSelection("akka://test/user/GamesRoot/GameManager1");
            Debug.WriteLine($"selectionpath = {gameSelection.PathString}");
            IActorRef gameManager = gameSelection.ResolveOne(TimeSpan.FromSeconds(1)).Result;
            Assert.False(gameManager.IsNobody());
        }

        [Fact]
        public void gameRoot_createGame_gameIsCreated()
        {
            //IActorRef gameRoot = ActorOf<GamesRoot>("GamesRoot");
            _gamesRoot.Tell(new CreateGame("TestGameName"));

            Debug.WriteLine($"{DateTime.Now} Waiting");
            System.Threading.Thread.Sleep(1000);
            //Task.Delay(5000);
            Debug.WriteLine($"{DateTime.Now} Waiting Done");

            var gameSelection = ActorSelection("akka://test/user/GamesRoot/GameManager1");
            Debug.WriteLine($"Searching for GameManager: {gameSelection.PathString}");
            IActorRef gameManager = gameSelection.ResolveOne(TimeSpan.FromSeconds(3)).Result;
            Assert.False(gameManager.IsNobody());

            JoinGameSuccess success = ExpectMsg<JoinGameSuccess>(TimeSpan.FromSeconds(1));
            Assert.False(success.Game.IsNobody());
        }

        [Fact]
        public void gameRoot_joinGame_addsPlayer()
        {
            //IActorRef gameRoot = ActorOf<GamesRoot>("GamesRoot");
            _gamesRoot.Tell(new CreateGame("TestGameName"));

            var gameSelection = ActorSelection("akka://test/user/GamesRoot/GameManager1");
            IActorRef gameManager = gameSelection.ResolveOne(TimeSpan.FromSeconds(3)).Result;
            Assert.False(gameManager.IsNobody());
            System.Threading.Thread.Sleep(1000);

            JoinGameSuccess success = ExpectMsg<JoinGameSuccess>(TimeSpan.FromSeconds(1));
            Assert.False(success.Game.IsNobody());
            var game = success.Game;
            game.Tell(new JoinGame(TestActor));
            ExpectMsg<JoinGameSuccess>();
        }
    }
}
