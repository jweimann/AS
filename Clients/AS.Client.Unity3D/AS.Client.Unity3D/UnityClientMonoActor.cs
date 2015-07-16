using System;
using System.Collections;
using System.Collections.Generic;

namespace AS.Client.Unity3D
{
    public abstract class UnityClientMonoActor : UnityEngine.MonoBehaviour, IClientActor
    {
        private Queue<object> _mailbox;
        public string Path { get; private set; }

        public UnityClientMonoActor()
        {
            Path = null;
        }

        public UnityClientMonoActor(string path)
        {
            Path = path;
        }

        void Update()
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
