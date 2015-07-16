using Akka.Actor;
using AS.Client.Messages.Entities;
using AS.Messages.Entities;
using AS.Messages.Region;
using System;
using AS.Common;
using Xunit;

namespace AS.Actors.Tests
{
    public class EntityTests : ASTestBase
    {
        [Fact]
        public void entity_setPosition_notifiesRegion()
        {
            Props props = Props.Create<Entity>(new object[] { 1, Vector3.zero });
            IActorRef entity = Sys.ActorOf(props, "testentity");
            entity.Tell(new JoinRegionSuccess(this.TestActor));
            entity.Tell(new SetPosition(Vector3.one, 1));
            UpdatePosition response = ExpectMsg<UpdatePosition>(TimeSpan.FromSeconds(1));
            Assert.Equal(Vector3.one, response.Position);
        }
    }
}
