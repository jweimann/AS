using Akka.Actor;
using AS.Admin.ServerStatsClient.ViewModels;
using AS.Messages.SystemStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ServerStatsClient.Controllers
{
    public class ServerStatsController : ReceiveActor
    {
        private ActorSelection _serverSystemStatsActor;
        private ServerStatsViewModel _viewModel;
        public ServerStatsController(ServerStatsViewModel viewModel)
        {
            _viewModel = viewModel;
 
            Receive<SystemStats>(message => {
                _viewModel.RoomCount = message.RoomCount;
                _viewModel.GameCount = message.GameCount;
            });

            _serverSystemStatsActor = Context.System.ActorSelection("akka.tcp://as@localhost:8081/user/StatsGatherer");

            _serverSystemStatsActor.Tell(new SubscribeToStats(Self));
        }
    }
}
