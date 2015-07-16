using AS.Client.Messages;
using AS.Client.Messages.ClientRequests;
using AS.Client.Messages.Errors;
using AS.Client.Messages.Game;
using AS.Client.Messages.User;
using UnityEngine;

namespace AS.Client.Unity3D
{
    public class UnityClientUserActor : ClientActor
    {
        public UnityClientUserActor(string path) : base(path)
        {
        }

        public override void Receive(object message)
        {
            if (message is NotAuthenticated)
            {
                SendMessageToServer(new ClientAuthenticateRequest("jason", "jasonspass"));
                System.Console.WriteLine("NOT AUTHENTICATED");
                //throw new System.Exception("NOT AUTHENTICATED");
            }
            if (message is ClientUserCreated)
            {
                ClientUserCreated msg = (ClientUserCreated)message;
                SendMessageToServer(new CreateGame("gameone"));
            }
            if (message is ClientJoinGameSuccessResponse)
            {
                SendMessageToServer(new StartGame());
            }
            if (message is GameStarted)
            {
                SendMessageToServer(new ClientSpawnEntityRequest("test", 100000));
            }
            //Debug.Log("Got Message " + message.ToString());
        }

   
    }
}
