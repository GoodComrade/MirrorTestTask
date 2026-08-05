using System;
using System.Text;
using UnityEngine;

namespace Networking.Serialization
{
    public sealed class JsonNetworkSerializer : INetworkSerializer
    {
        public byte[] Serialize<T>(T message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            string json = JsonUtility.ToJson(message);

            return Encoding.UTF8.GetBytes(json);
        }

        public object Deserialize(byte[] data, Type type)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            string json = Encoding.UTF8.GetString(data);

            return JsonUtility.FromJson(json, type);
        }
    }
}
