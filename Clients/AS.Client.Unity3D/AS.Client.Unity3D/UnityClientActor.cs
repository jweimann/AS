using System;

namespace AS.Client.Unity3D
{
    public class UnityClientActor : UnityEngine.MonoBehaviour
    {
        public string Path { get; private set; }

        public UnityClientActor(string path)
        {
            Path = path;
        }

        internal void Tell(object message)
        {
            throw new NotImplementedException();
        }
    }
}
