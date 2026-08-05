using Mirror;
using Networking.Messages;
using System;
using System.Collections.Generic;

namespace Networking.Mirror
{
    public sealed class MirrorClientMessenger : IClientMessenger
    {
        private bool _handlerRegistered;

        public bool IsActive => NetworkClient.active;

        public void Send<T>(T message)
            where T : struct, NetworkMessage
        {
            NetworkClient.Send(message);
        }

        public void RegisterEnvelopeHandler(Action<NetworkEnvelope> handler)
        {
            if (_handlerRegistered)
                return;

            NetworkClient.RegisterHandler(handler);

            _handlerRegistered = true;
        }
    }
}
