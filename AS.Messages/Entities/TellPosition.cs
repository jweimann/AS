using UnityEngine;

namespace AS.Messages.Entities
{
    public class TellPosition
    {
        public AS.Common.Vector3 Position { get; private set; }

        public TellPosition(AS.Common.Vector3 position)
        {
            Position = position;
        }
    }
}