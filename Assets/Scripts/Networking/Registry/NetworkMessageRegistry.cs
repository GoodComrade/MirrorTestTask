using System;
using System.Collections.Generic;

namespace Networking.Registry
{
    public sealed class NetworkMessageRegistry
    {
        private readonly Dictionary<Type, ushort> _typeToId = new();

        private readonly Dictionary<ushort, Type> _idToType = new();
        /// <summary>
        /// Registering user message.
        /// </summary>
        public void Register<T>(ushort id)
        {
            Type type = typeof(T);

            if (_idToType.ContainsKey(id))
            {
                throw new InvalidOperationException(
                    $"Message id '{id}' is already registered.");
            }

            _typeToId.Add(type, id);
            _idToType.Add(id, type);
        }

        /// <summary>
        /// Return message id.
        /// </summary>
        public ushort GetId<T>()
        {
            return GetId(typeof(T));
        }

        /// <summary>
        /// Return message id.
        /// </summary>
        public ushort GetId(Type type)
        {
            if (!_typeToId.TryGetValue(type, out ushort id))
                throw new InvalidOperationException(
                    $"Message type '{type.Name}' is not registered.");

            return id;
        }

        public Type GetType(ushort id)
        {
            if (!_idToType.TryGetValue(id, out Type type))
            {
                throw new InvalidOperationException(
                    $"Message id '{id}' is not registered.");
            }

            return type;
        }
        /// <summary>
        /// Check registry type.
        /// </summary>
        public bool IsRegistered<T>()
        {
            return _typeToId.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Check registry id.
        /// </summary>
        public bool IsRegistered(Type type)
        {
            return _typeToId.ContainsKey(type);
        }
    }
}
