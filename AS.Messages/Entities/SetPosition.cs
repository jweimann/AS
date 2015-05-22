using System.Numerics;

namespace AS.Messages.Entities
{
    public class SetPosition
    {
        public Vector3 Position { get; private set; }

        public SetPosition(Vector3 position)
        {
            this.Position = position;
        }
    }
}