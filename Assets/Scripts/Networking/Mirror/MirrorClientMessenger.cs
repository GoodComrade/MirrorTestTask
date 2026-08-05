using Mirror;
using Networking.Messages;
using System;
using System.Collections.Generic;

namespace Networking.Mirror
{
    public sealed class MirrorClientMessenger : IClientMessenger
    {
        public bool IsActive => NetworkServer.active;

        public IReadOnlyCollection<NetworkConnectionToClient> Connections =>
            NetworkServer.connections.Values;

        public void Send<T>(T message)
            where T : struct, NetworkMessage
        {
            NetworkClient.Send(message);
        }

        public void RegisterEnvelopeHandler(Action<NetworkEnvelope> handler)
        {
            NetworkClient.RegisterHandler<NetworkEnvelope>(handler);
        }
    }
}
