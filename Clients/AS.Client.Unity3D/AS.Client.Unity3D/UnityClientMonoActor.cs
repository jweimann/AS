using AS.Client.Core;
using AS.Common;
using System.Collections.Generic;

namespace AS.Client.Unity3D
{
    public abstract class UnityClientMonoActor : UnityEngine.MonoBehaviour, IClientActor
    {
        private Queue<object> _mailbox;
        public string Path { get; private set; }
        public long EntityId { get; private set; }

        public abstract EntityType EntityType { get; }

        public UnityClientMonoActor()
        {
            _mailbox = new Queue<object>();
            Path = null;
        }

        public UnityClientMonoActor(string path, long entityId)
        {
            _mailbox = new Queue<object>();
            Path = path;
            EntityId = entityId;
        }

        public void SetEntityId(long entityId) { EntityId = entityId; }

        protected void Update()
        {
            while (_mailbox.Count > 0)
            {
                object message = _mailbox.Dequeue();
                Receive(message);
            }
        }

        public void Tell(object message)
        {
            _mailbox.Enqueue(message);
        }

        public abstract void Receive(object message);
    }
}
