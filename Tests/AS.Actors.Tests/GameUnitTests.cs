using System;
using System.Collections.Generic;
using System.Numerics;
using Akka.Actor;
using AS.Actors.GameActors;
using AS.Messages;
using AS.Messages.Entities;
using AS.Messages.Game;
using AS.MockActors;
using Xunit;

namespace AS.Actors.Tests
{
    public class GameUnitTests : ASTestBase
    {
        [Fact]
        public void game_addUser_addsUserToCollection()
        {
            var testGameActor = ActorOfAsTestActorRef<Game>("game");
            testGameActor.Tell(new JoinGame(this.TestActor));
            Assert.Equal(1, testGameActor.UnderlyingActor.Players.Count);
        }

        [Fact]
        public void game_startGameWithNoPlayers_returnsError()
        {
            var testGameActor = ActorOfAsTestActorRef<Game>("game");
            testGameActor.Tell(new StartGame());
            string result = ExpectMsg<string>();
            Assert.Equal("Failed to start, not enough players", result);
        }

        [Fact]
        public void game_startGameWithPlayers_starts()
        {
            var testGameActor = ActorOfAsTestActorRef<Game>("game");
            testGameActor.Tell(new JoinGame(this.TestActor));
            testGameActor.Tell(new JoinGame(this.TestActor));
            ExpectMsg<JoinGameSuccess>();
            ExpectMsg<JoinGameSuccess>();
            testGameActor.Tell(new StartGame());
            testGameActor.Tell(new GetGameState());
            ExpectMsg<GameStarted>();
        }

        [Fact]
        public void game_initialState_notStarted()
        {
            var testGameActor = ActorOfAsTestActorRef<Game>("game");
            testGameActor.Tell(new GetGameState());
            string status = ExpectMsg<string>();
            Assert.Equal("NotStarted", status);
        }

        //[Fact]
        //public void game_spawnEntity_addsEntity()
        //{
        //    var testGameActor = ActorOfAsTestActorRef<Game>("game");
        //    testGameActor.Tell(new GetGameState());
        //    string status = ExpectMsg<string>();
        //    Assert.Equal("NotStarted", status);
        //    testGameActor.Tell(new JoinGame(this.TestActor));
        //    testGameActor.Tell(new StartGame());
        //    ExpectMsg<GameStarted>();
        //    testGameActor.Tell(new SpawnEntity(1, "JasonsEntity"));
        //    ExpectMsg<SpawnEntity>();

        //    //FishForMessage()
        //}

        [Fact]
        public void entityManager_spawnEntity_addsEntity()
        {
            var testGameActor = ActorOfAsTestActorRef<EntityManager>("EntityManager");
            testGameActor.Tell(new SpawnEntity(1, "JasonsEntity", Vector3.Zero));

            Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject privBase = 
                new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(testGameActor.UnderlyingActor, new
                    Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(EntityManager)));
            
            Assert.Equal(1, ((List<IActorRef>)privBase.GetFieldOrProperty("Entities")).Count);
        }

        [Fact]
        public void entityManager_spawnEntity_populatesEntitiesByIdDictionary()
        {
            var testGameActor = ActorOfAsTestActorRef<EntityManager>("EntityManager");
            testGameActor.Tell(new SpawnEntity(1, "JasonsEntity", Vector3.Zero));

            //PrivateObject privBase = new PrivateObject(testGameActor.UnderlyingActor, new PrivateType(typeof(EntityManager)));

            //testGameActor.UnderlyingActor.Entities
            Assert.Equal(1, testGameActor.UnderlyingActor.EntitiesById.Count);
        }

        // This is nolonger true.  Entitymanager doesn't respond now.
        //[Fact]
        //public void entityManager_spawnEntity_respondsToGameWithResultMessage()
        //{
            
        //    var testGameActor = ActorOfAsTestActorRef<EntityManager>("EntityManager");
        //    testGameActor.Tell(new SpawnEntity(1, "JasonsEntity", Vector3.Zero));
        //    ForwardToPlayers forwardMessage = ExpectMsg<ForwardToPlayers>();
        //    Assert.IsType<SpawnEntity>(forwardMessage.Message);
        //}

        [Fact]
        public void game_forwardToPlayers_sendsToPlayers()
        {
            //IActorRef users = ActorOf(Props.Create<ForwardToTestActor>(new object[] { TestActor }),
            //    "users");
            var testGameActor = ActorOfAsTestActorRef<Game>("Game");
            testGameActor.Tell(new JoinGame(TestActor));
            ExpectMsg<JoinGameSuccess>();
            testGameActor.Tell(new StartGame());
            ExpectMsg<GameStarted>();

            testGameActor.Tell(new ForwardToPlayers("Hello"));
            var forwardedMessage = ExpectMsg<string>(TimeSpan.FromSeconds(3));
        }

    }
}