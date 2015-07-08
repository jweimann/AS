using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AS.Serialization
{
    public class BinarySerializer //: IMessageSerializer
    {
        public byte[] Serialize(object command)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, command);
                return stream.GetBuffer();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                T command = (T)serializer.Deserialize(stream);
                return command;
            }
        }
    }
}