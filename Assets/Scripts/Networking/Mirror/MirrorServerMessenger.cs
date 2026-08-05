using Mirror;
using Networking.Messages;
using System;
using System.Collections.Generic;

namespace Networking.Mirror
{
    public sealed class MirrorServerMessenger : IServerMessenger
    {
        public bool IsActive => NetworkServer.active;

        public IReadOnlyCollection<NetworkConnectionToClient> Connections =>
            NetworkServer.connections.Values;

        public void Send<T>(
            NetworkConnectionToClient connection,
            T message)
            where T : struct, NetworkMessage
        {
            connection.Send(message);
        }

        public void RegisterSubscriptionHandler(
            Action<NetworkConnectionToClient, SubscriptionMessage> handler)
        {
            NetworkServer.RegisterHandler(handler);
        }
    }
}
