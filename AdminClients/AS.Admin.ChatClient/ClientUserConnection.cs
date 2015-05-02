using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient
{
    public class ClientUserConnection : ReceiveActor
    {
        private IActorRef _clientLobby;
        private IActorRef _clientRoom;

        public ClientUserConnection()
        {

        }

    }
}
