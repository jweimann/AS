using System.Numerics;

namespace AS.Messages.Entities
{
    public class TellPosition
    {
        public Vector3 Position { get; private set; }

        public TellPosition(Vector3 position)
        {
            Position = position;
        }
    }
}