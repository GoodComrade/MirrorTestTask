using System;
using System.Text;
using UnityEngine;

namespace Networking.Serialization
{
    public sealed class JsonNetworkSerializer : INetworkSerializer
    {
        [Serializable]
        private class JsonWrapper
        {
            public string Json;
        }

        public byte[] Serialize<T>(T message)
        {
            string json = JsonUtility.ToJson(message);

            return Encoding.UTF8.GetBytes(json);
        }

        public object Deserialize(byte[] data, Type type)
        {
            string json = Encoding.UTF8.GetString(data);

            return JsonUtility.FromJson(json, type);
        }
    }
}
