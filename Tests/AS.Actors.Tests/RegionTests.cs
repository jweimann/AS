using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using AS.Common;
using AS.Messages.Entities;
using AS.Messages.Game;
using AS.Messages.Region;
using Xunit;
using AS.Actors.GameActors;

namespace AS.Actors.Tests
{
    public class RegionTests : ASTestBase
    {

        [Fact]
        public void game_startup_spawnsDefaultRegions()
        {
            var game = ActorOf<Game>("game");
            var regions = Sys.ActorSelection("akka://test/user/game/RegionManager");
            System.Threading.Thread.Sleep(2000);

            Debug.WriteLine($"Search Path: {regions.PathString}");
            IActorRef regionsActor = regions.ResolveOne(TimeSpan.FromSeconds(1)).Result;
        }

        [Fact]
        public void regions_startup_spawnRootRegion()
        {
            Props props = Props.Create<RegionManager>(new object[] { 100.0f, 3 });
            var regionManager = ActorOfAsTestActorRef<RegionManager>(props, "RegionManager");
            Debug.WriteLine($"Regionmanager spawned at path {regionManager.Path.ToString()}");

            var regions = Sys.ActorSelection("akka://test/user/RegionManager/RootRegion");
            Debug.WriteLine($"Search Path: {regions.PathString}");

            IActorRef regionActor = regions.ResolveOne(TimeSpan.FromSeconds(1)).Result;
        }

        [Fact]
        public void regions_AddEntityToRegion_addsEntityToDictionary()
        {
            Props props = Props.Create<Region>(new object[] { new Bounds(Vector3.Zero, Vector3.One * 100f), 3 });
            var region = ActorOfAsTestActorRef<Region>(props, "Region1");
            region.Tell(new AddEntityToRegion(1, TestActor, Vector3.Zero));

            var regionInternal = GetPrivate(region);

            Assert.Equal(1, ((Dictionary<long, IActorRef>)regionInternal.GetFieldOrProperty("_entities")).Count);
        }

        [Fact]
        public void regions_AddTooManyEntityToRegion_spawnsChildren()
        {
            var region = GetRootRegion(3);

            SpawnEntitiesInRegion(region, new Vector3[]
            {
                new Vector3(10, 0, 0),
                new Vector3(10, 0, 0),
                new Vector3(-10, 0, 0),
                new Vector3(5, 0, 0)
            });

            EntitiesInRegionList entitiesInRegion = region.Ask<EntitiesInRegionList>(new RequestEntityList()).Result;
            Assert.Equal(0, entitiesInRegion.Entities.Count);
        }

        [Fact]
        public void regions_addEntityOutsideBounds_failsToAddReturnsError()
        {
            var rootRegion = GetRootRegion();
            SpawnEntitiesInRegion(rootRegion, new Vector3[]
             {
                    new Vector3(1000, 0, 0)
             });
            rootRegion.Tell(new RequestEntityList());
            var msg = ExpectMsg<EntitiesInRegionList>();
            Assert.Equal(0, msg.Entities.Count);
        }

        [Fact]
        public void regions_updatePosition_sentToUsers()
        {
            var rootRegion = GetRootRegion();
            rootRegion.Tell(new SubscribeUserToRegion(this.TestActor));
            rootRegion.Tell(new UpdatePosition(1, Vector3.One));
            var response = ExpectMsg<UpdatePosition>();
            Assert.Equal(1, response.EntityId);
            Assert.Equal(Vector3.One, response.Position);
        }

        //[Fact]
        //public void regions_messasgeEntities_allEntitiesRecieve()
        //{
        //    /// NOT IMPLEMENTED YET.
        //    Props props = Props.Create<Region>(new object[] { new Bounds(Vector3.Zero, Vector3.One * 100f), 3 });
        //    var region = ActorOfAsTestActorRef<Region>(props, "Region1");

        //    /*
        //    var entity1 = SpawnEntityAndAddToRegion(region, 1, new Vector3(10, 0, 0));
        //    var entity2 = SpawnEntityAndAddToRegion(region, 2, new Vector3(10, 0, 0));
        //  */

        //    //region.Tell(new UpdatePosition(1, new Vector3(20, 20, 20)));
        //    //entity1.Tell()
        //    ExpectMsg<string>(TimeSpan.FromSeconds(90));
        //    Assert.True(false);
        //}

        private void SpawnNEntitiesInRegion(IActorRef region, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnEntityAndAddToRegion(region, i + 1, Vector3.Zero);
            }
        }

        private void SpawnEntitiesInRegion(IActorRef region, params Vector3[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                SpawnEntityAndAddToRegion(region, i + 1, positions[i]);
            }
        }
        private TestActorRef<EntityManager> _entityManager;
        private void SpawnEntityAndAddToRegion(IActorRef region, int id, Vector3 position)
        {
            if (_entityManager.IsNobody())
                _entityManager = ActorOfAsTestActorRef<EntityManager>("EntityManager");

            string name = "entity" + id;
            _entityManager.Tell(new SpawnEntity(id, name, position));
        }

        [Fact]
        private void entityManager_spawnEntity_noErrors()
        {
            Props props = Props.Create<Region>(new object[] { new Bounds(Vector3.Zero, Vector3.One * 100f), 3 });
            var region = ActorOfAsTestActorRef<Region>(props, "Region1");
            SpawnEntitiesInRegion(region, new Vector3[] { Vector3.Zero });
        }

        [Fact]
        private void entityManager_spawnEntityCount10_noErrors()
        {
            Props props = Props.Create<Region>(new object[] { new Bounds(Vector3.Zero, Vector3.One * 100f), 3 });
            var region = ActorOfAsTestActorRef<Region>(props, "Region1");
            SpawnEntitiesInRegion(region, new Vector3[] { Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero });
        }

        [Fact]
        private void entityManager_spawnEntity_entityJoinsRegion()
        {
            var region = GetRootRegion();

            SpawnEntitiesInRegion(region, new Vector3[] { Vector3.Zero });
            EntitiesInRegionList entitiesInRegion = region.Ask<EntitiesInRegionList>(new RequestEntityList()).Result;

            Assert.Equal(1, entitiesInRegion.Entities.Count);
        }

        [Fact]
        private void region_requestEntityList_returnsChildRegionData()
        {
            var region = GetRootRegion(3);

            SpawnEntitiesInRegion(region, new Vector3[] { Vector3.One, Vector3.One, -Vector3.One, -Vector3.One, -Vector3.One });
            region.Tell(new RequestEntityList());

            //EntitiesInRegionList expectedFromRoot = new EntitiesInRegionList(new List<IActorRef>(), "akka://test/user/RegionManager/RootRegion");
            //EntitiesInRegionList expectedFromNegative = new EntitiesInRegionList(new List<IActorRef>(), "akka://test/user/RegionManager/RootRegion/-50_0_0");
            //EntitiesInRegionList expectedFromPositive = new EntitiesInRegionList(new List<IActorRef>(), " akka://test/user/RegionManager/RootRegion/50_0_0");
            //

            var msgs = ReceiveN(3);
            var entityLists = new List<EntitiesInRegionList>();
            foreach (var msg in msgs)
            {
                entityLists.Add(msg as EntitiesInRegionList);
            }

            //EntitiesInRegionList listChild1 = ExpectMsg<EntitiesInRegionList>();
            //var entityLists = ExpectMsgAllOf(TimeSpan.FromSeconds(5), new EntitiesInRegionList[] { expectedFromRoot, expectedFromNegative, expectedFromPositive });

            //return;

            EntitiesInRegionList listRoot = entityLists.FirstOrDefault(t => t.RegionPath.Contains("50") == false);
            EntitiesInRegionList childPositive = entityLists.FirstOrDefault(t => t.RegionPath.Contains("/50") == true);
            EntitiesInRegionList childNegative = entityLists.FirstOrDefault(t => t.RegionPath.Contains("-50") == true);

            Assert.Equal(2, childPositive.Entities.Count);
            Assert.Equal(3, childNegative.Entities.Count);
            Assert.Equal(0, listRoot.Entities.Count); // region needs to push entities to children when it has children!!.

            //Debug.WriteLine($"ListRoot: {listRoot.RegionPath} Count: {listRoot.Entities.Count}");
            //EntitiesInRegionList listChild1 = ExpectMsg<EntitiesInRegionList>();
            //Debug.WriteLine($"listChild1: {listChild1.RegionPath} Count: {listChild1.Entities.Count}");
            //EntitiesInRegionList listChild2 = ExpectMsg<EntitiesInRegionList>();
            //Debug.WriteLine($"listChild2: {listChild2.RegionPath} Count: {listChild2.Entities.Count}");

        }

        private IActorRef GetRootRegion(int maxEntitiesPerRegion = Int32.MaxValue, int regionSize = 100)
        {
            Props regionManagerProps = Props.Create<RegionManager>(new object[] { regionSize, maxEntitiesPerRegion });
            ActorOfAsTestActorRef<RegionManager>(regionManagerProps, "RegionManager");

            Debug.WriteLine("Getting RM");
            var regionManagerActor = ActorSelection("/user/RegionManager").ResolveOne(TimeSpan.FromSeconds(1)).Result;
            Debug.WriteLine("Getting Region");
            var region = ActorSelection("/user/RegionManager/RootRegion").ResolveOne(TimeSpan.FromSeconds(1)).Result;
            return region;
        }

        private static Dictionary<long, IActorRef> GetEntitiesDictionary(TestActorRef<Region> region)
        {
            return GetPrivate(region).GetField("_entities") as Dictionary<long, IActorRef>;
        }
    }
}