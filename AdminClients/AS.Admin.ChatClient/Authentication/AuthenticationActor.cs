using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using AS.Messages;

namespace AS.Admin.ChatClient.Authentication
{
    public class AuthenticationActor : ClientUIActorBase
    {
        public AuthenticationActor(Action<UserCreated> onUserCreated) : base()
        {
            Receive<UserCreated>(message =>
            {
                _myUserConnection = message.UserConnectionActor;
                if (onUserCreated != null)
                    onUserCreated(message);

                Context.Parent.Tell(message);

                Console.WriteLine(message);
                Become(Connected);
            });
        }

        private void Connected()
        {
            _myUserConnection.Tell(new Authenticate("jason"));
            Receive<AuthenticateResult>(message =>
            {
                Become(Authenticated);
                _myUserConnection.Tell(new GetRooms());
            });
        }

        private void Authenticated()
        {
            //throw new NotImplementedException();
        }
    }
}