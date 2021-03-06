﻿using AS.Common;
using System;

namespace AS.Client.Messages
{
    /// <summary>
    /// Is this a message from the client or to it?  Need to differentiate these somehow..
    /// </summary>
    [Serializable]
    public class ClientSpawnEntityRequest : IClientMessage
    {
        public EntityType EntityType { get; set; }
        public int Count { get; set; }

        public ClientSpawnEntityRequest(EntityType entityType, int count)
        {
            EntityType = entityType;
            Count = count;
        }

        public object GetServerMessage()
        {
            throw new NotImplementedException();
        }
    }
}
