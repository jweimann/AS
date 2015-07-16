using AS.Client.Messages.Entities;
using AS.Client.Unity3D.Converters;

namespace AS.Client.Unity3D.Entities
{
    public class UnityEntityActor : ClientActor
    {
        private UnityEngine.Vector3 _position;

        public UnityEntityActor(string path) : base(path)
        {
        }

        public override void Receive(object message)
        {
            if (message is UpdatePosition)
            {
                _position = ((UpdatePosition)message).Position.ToUnity();
                //System.Console.WriteLine("Entity Moved: " + _position.ToString());
            }
        }
    }
}
