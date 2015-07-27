using AS.Actors.Initalization;
using Akka.Actor;
using AS.Messages.Entities;
using AS.Common;

namespace SG.Server.Actors
{
    public class SpaceGameInitializer : GameInitializer
    {
        public SpaceGameInitializer(IActorRef gameActor) : base(gameActor)
        {
        }

        protected override void Initialize()
        {
            for(int i = 0; i < 300; i++)
            {
                SendInitalizationMessage(new SpawnEntity(1, "Asteroid1", Vector3.Random(200), 1));
            }
            
            base.Initialize();

        }
    }
}
