using EasyNetQ;
using ProtoBuf;

namespace Solvers.App.Serializers
{
    public class ProtobufSerializer : ISerializer
    {
        public object BytesToMessage(Type messageType, byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);

            var message = Serializer.Deserialize(messageType, stream);

            return message;
        }

        public byte[] MessageToBytes(Type messageType, object message)
        {
            using var stream = new MemoryStream();

            Serializer.Serialize(stream, message);

            return stream.ToArray();
        }
    }
}
