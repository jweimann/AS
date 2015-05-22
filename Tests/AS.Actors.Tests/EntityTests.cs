using Akka.Actor;
using AS.Messages.Entities;
using AS.Messages.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AS.Actors.Tests
{
    public class EntityTests : ASTestBase
    {
        [Fact]
        public void entity_setPosition_notifiesRegion()
        {
            Props props = Props.Create<Entity>(new object[] { 1, Vector3.Zero });
            IActorRef entity = Sys.ActorOf(props, "testentity");
            entity.Tell(new JoinRegionSuccess(this.TestActor));
            entity.Tell(new SetPosition(Vector3.One));
            UpdatePosition response = ExpectMsg<UpdatePosition>(TimeSpan.FromSeconds(1));
            Assert.Equal(Vector3.One, response.Position);
        }
    }
}
