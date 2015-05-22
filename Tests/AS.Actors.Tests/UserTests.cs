using System;
using System.Numerics;
using Akka.Actor;
using Akka.TestKit;
using AS.Actors.UserActors;
using AS.Messages;
using AS.Messages.Entities;
using AS.Messages.Game;
using Xunit;
using AS.Messages.Region;
using System.Diagnostics;
using System.Threading;

namespace AS.Actors.Tests
{
    public class UserTests : ASTestBase
    {
        [Fact]
        public void user_createGame_createsGame()
        {
            IActorRef game;
            var user = CreateUserAndGame(out game);
        }

        [Fact]
        public void user_createGame_getsValidEntitymanager()
        {
            IActorRef game;
            var user = CreateUserAndGame(out game);
            ActorSelection entityManagerSelection = (ActorSelection)GetPrivate(user).GetField("_entityManager");
            entityManagerSelection.ResolveOne(TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void user_spawnEntity_createsEntity()
        {
            IActorRef game;
            var user = CreateUserAndGame(out game);
            user.Tell(new SpawnEntity(1, "TestEntity", Vector3.Zero));

            ActorSelection rootRegionSelection = new Akka.Actor.ActorSelection(game, "RegionManager/RootRegion");
            IActorRef rootRegion = rootRegionSelection.ResolveOne(TimeSpan.FromSeconds(1)).Result;

            Thread.Sleep(2000);

            rootRegion.Tell(new RequestEntityList());
            EntitiesInRegionList response = ExpectMsg<EntitiesInRegionList>();
            Debug.WriteLine($"Response Entities: {response.Entities.Count}");

            Assert.Equal(1, response.Entities.Count);
        }

      
    }
}
