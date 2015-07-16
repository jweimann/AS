using System.Collections.Generic;

namespace AS.Client.Unity3D
{
    public abstract class ClientActor : IClientActor
    {
        private Queue<object> _mailbox;
        public string Path { get; private set; }

        public ClientActor(string path)
        {
            Path = path;
            _mailbox = new Queue<object>();
        }

        /// <summary>
        /// This wont work because it's not a monobehavior..
        /// </summary>
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
            //_mailbox.Enqueue(message); // Can't mailbox yet, not a monobehavior, need to set this up.
            Receive(message);
        }

        public abstract void Receive(object message);

        protected void SendMessageToServer(object message)
        {
            UnityClientActorSystem.Instance.SendMessage(message);
        }
    }
}
