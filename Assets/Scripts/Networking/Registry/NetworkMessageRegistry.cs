using System;
using System.Collections.Generic;

namespace Networking.Registry
{
    public sealed class NetworkMessageRegistry
    {
        private readonly Dictionary<Type, ushort> _typeToId = new();
        private readonly Dictionary<ushort, Type> _idToType = new();

        public void Register<T>(ushort id)
        {
            Register(typeof(T), id);
        }

        public void Register(Type type, ushort id)
        {
            if (_typeToId.ContainsKey(type))
                throw new InvalidOperationException($"Message type '{type.Name}' is already registered.");

            if (_idToType.ContainsKey(id))
                throw new InvalidOperationException($"Message id '{id}' is already registered.");

            _typeToId.Add(type, id);
            _idToType.Add(id, type);
        }

        public ushort GetId<T>()
        {
            return GetId(typeof(T));
        }

        public ushort GetId(Type type)
        {
            if (!_typeToId.TryGetValue(type, out ushort id))
                throw new InvalidOperationException($"Message type '{type.Name}' is not registered.");

            return id;
        }

        public bool TryGetType(ushort id, out Type type)
        {
            return _idToType.TryGetValue(id, out type);
        }

        public bool IsRegistered(Type type)
        {
            return _typeToId.ContainsKey(type);
        }

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }
    }
}
