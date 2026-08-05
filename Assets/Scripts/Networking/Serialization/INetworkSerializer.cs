using System;

namespace Networking.Serialization
{
    public interface INetworkSerializer
    {
        byte[] Serialize<T>(T message);

        object Deserialize(byte[] data, Type type);
    }
}