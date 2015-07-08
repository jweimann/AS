using System;

namespace AS.Client.Messages
{
    [Serializable]
    public class ClientSpawnEntity
    {
        public string EntityType { get; set; }
        public int Count { get; set; }

        public ClientSpawnEntity(string entityType, int count)
        {
            EntityType = entityType;
            Count = count;
        }
    }
}
