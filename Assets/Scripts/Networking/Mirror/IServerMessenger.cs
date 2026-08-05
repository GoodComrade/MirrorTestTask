using Mirror;
using Networking.Messages;
using System;
using System.Collections.Generic;

namespace Networking.Mirror
{
    public interface IServerMessenger
    {
        bool IsActive { get; }

        IReadOnlyCollection<NetworkConnectionToClient> Connections { get; }

        void Send<T>(NetworkConnectionToClient connection, T message)
            where T : struct, NetworkMessage;

        void RegisterSubscriptionHandler(
            Action<NetworkConnectionToClient, SubscriptionMessage> handler);
    }
}