using AS.Client.Messages;
using AS.Client.Messages.ClientRequests;
using AS.Client.Messages.Errors;
using AS.Client.Messages.Game;
using AS.Client.Messages.User;
using AS.Common;
using UnityEngine;

namespace AS.Client.Unity3D
{
    public class UnityClientUserActor : ClientActor
    {
        private const int DEBUG_ENTITY_COUNT = 0;
        public UnityClientUserActor(string path) : base(path)
        {
            SendMessageToServer(new ClientAuthenticateRequest("jason", "jasonspass"));
        }

        public override void Receive(object message)
        {
            Logging.Logger.LogDebug("UnityClientUserActor Receive - Message: {0}", message.ToString());
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
                SendMessageToServer(new ClientSpawnEntityRequest(EntityType.Asteroid, DEBUG_ENTITY_COUNT));
            }
            //Debug.Log("Got Message " + message.ToString());
        }

   
    }
}
