using System.Collections.Generic;
using Akka.Actor;

namespace AS.Messages.Region
{
    public class EntitiesInRegionList
    {
        public List<IActorRef> Entities { get; private set; }
        public string RegionPath { get; private set; }
        public EntitiesInRegionList(List<IActorRef> entities, string regionPath)
        {
            Entities = entities;
            RegionPath = regionPath;
        }
    }
}
