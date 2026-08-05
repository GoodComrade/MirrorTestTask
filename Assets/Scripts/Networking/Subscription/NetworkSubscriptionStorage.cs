using System.Collections.Generic;
using Mirror;

namespace Networking.Subscription
{
    public sealed class NetworkSubscriptionStorage
    {
        private readonly Dictionary<NetworkConnectionToClient, HashSet<ushort>>
            _subscriptions = new();

        public void RegisterConnection(NetworkConnectionToClient connection)
        {
            if (_subscriptions.ContainsKey(connection))
                return;

            _subscriptions.Add(connection, new HashSet<ushort>());
        }

        public void RemoveConnection(NetworkConnectionToClient connection)
        {
            _subscriptions.Remove(connection);
        }

        public void AddSubscription(NetworkConnectionToClient connection, ushort typeId)
        {
            RegisterConnection(connection);

            _subscriptions[connection].Add(typeId);
        }

        public bool HasSubscription(NetworkConnectionToClient connection, ushort typeId)
        {
            if (!_subscriptions.TryGetValue(connection, out var types))
                return false;

            return types.Contains(typeId);
        }

        public IReadOnlyCollection<ushort> GetSubscriptions(NetworkConnectionToClient connection)
        {
            if (_subscriptions.TryGetValue(connection, out var types))
                return types;

            return System.Array.Empty<ushort>();
        }

        public void Clear()
        {
            _subscriptions.Clear();
        }
    }
}
