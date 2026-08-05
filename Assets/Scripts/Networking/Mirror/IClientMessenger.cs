using System;
using Mirror;
using Networking.Messages;

namespace Networking.Mirror
{
    public interface IClientMessenger
    {
        bool IsActive { get; }

        void Send<T>(T message)
            where T : struct, NetworkMessage;

        void RegisterEnvelopeHandler(Action<NetworkEnvelope> handler);
    }
}
