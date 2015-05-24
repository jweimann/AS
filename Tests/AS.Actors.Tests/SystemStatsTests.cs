using Akka.Actor;
using AS.Actors.StatsActors;
using AS.Messages;
using AS.Messages.SystemStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AS.Actors.Tests
{
    public class SystemStatsTests : ASTestBase
    {
        [Fact]
        public void systemStats_requestStats_returnsResponse()
        {
            IActorRef statsGatherer = ActorOf<StatsGatherer>("StatsGatherer");
            Thread.Sleep(1000);
            statsGatherer.Tell(new GetSystemStats());
            SystemStats response = ExpectMsg<SystemStats>();
            Assert.Equal(1, response.RoomCount);
        }

        [Fact]
        public void systemStats_requestStatsWith2Rooms_returnsResponseWith2Rooms()
        {
            _lobby.Tell(new JoinRoom(this.TestActor, "Room1")); // OpenChat room is added automatically so this is #2
            IActorRef statsGatherer = ActorOf<StatsGatherer>("StatsGatherer");
            Thread.Sleep(1000);
            ExpectMsg<JoinRoomSuccess>();
            ExpectMsg<UserList>();
            //ReceiveWhile<object>(TimeSpan.FromSeconds(2), () => true);
            statsGatherer.Tell(new GetSystemStats());
            SystemStats response = ExpectMsg<SystemStats>();
            Assert.Equal(1, response.RoomCount);
        }
    }
}
