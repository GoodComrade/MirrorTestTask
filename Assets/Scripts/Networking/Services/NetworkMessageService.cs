using System;
using System.Collections.Generic;
using Mirror;
using Networking.Messages;
using Networking.Mirror;
using Networking.Registry;
using Networking.Serialization;
using Networking.Subscription;
using UnityEngine;

namespace Networking.Services
{
    public sealed partial class NetworkMessageService : INetworkMessageService
    {
        private readonly NetworkMessageRegistry _registry;
        private readonly INetworkSerializer _serializer;
        private readonly NetworkSubscriptionStorage _subscriptions;

        private readonly IServerMessenger _serverMessenger;
        private readonly IClientMessenger _clientMessenger;

        /// <summary>
        /// Registered client hadlers.
        /// Key — Message TypeId.
        /// Value — function taking serialized payload.
        /// </summary>
        private readonly Dictionary<Type, Action<byte[]>> _handlers = new();

        public NetworkMessageService(
            NetworkMessageRegistry registry,
            INetworkSerializer serializer,
            NetworkSubscriptionStorage subscriptions,
            IServerMessenger serverMessenger,
            IClientMessenger clientMessenger)
        {
            _registry = registry;
            _serializer = serializer;
            _subscriptions = subscriptions;

            _serverMessenger = serverMessenger;
            _clientMessenger = clientMessenger;
        }

        public void RegisterServerHandlers()
        {
            _serverMessenger.RegisterSubscriptionHandler(OnSubscriptionReceived);
        }

        public void RegisterClientHandlers()
        {
            _clientMessenger.RegisterEnvelopeHandler(OnEnvelopeReceived);
        }

        public void RegisterHandler<T>(Action<T> handler)
        {
            Type messageType = typeof(T);

            if (_handlers.ContainsKey(messageType))
            {
                throw new InvalidOperationException(
                    $"Handler for '{messageType.Name}' is already registered.");
            }

            _handlers.Add(messageType, payload =>
            {
                T message = (T)_serializer.Deserialize(payload, typeof(T));

                handler.Invoke(message);
            });
        }

        public void Subscribe<T>()
        {
            if (!_clientMessenger.IsActive)
            {
                throw new InvalidOperationException(
                    "Subscriptions can only be sent from a client.");
            }

            ushort typeId = _registry.GetId<T>();

            _clientMessenger.Send(new SubscriptionMessage
            {
                TypeId = typeId
            });
        }

        public void Send<T>(NetworkConnectionToClient connection, T message)
        {
            if (!_serverMessenger.IsActive)
            {
                throw new InvalidOperationException(
                    "Messages can only be sent from the server.");
            }

            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            ushort typeId = _registry.GetId<T>();

            if (!_subscriptions.HasSubscription(connection, typeId))
            {
                Debug.Log(
                    $"Connection {connection.connectionId} is not subscribed to message {typeof(T).Name}.");

                return;
            }

            byte[] payload = _serializer.Serialize(message);

            var envelope = new NetworkEnvelope
            {
                TypeId = typeId,
                Payload = payload
            };

            _serverMessenger.Send(connection, envelope);
        }

        public void Broadcast<T>(T message)
        {
            if (!_serverMessenger.IsActive)
            {
                throw new InvalidOperationException(
                    "Broadcast can only be called on the server.");
            }

            ushort typeId = _registry.GetId<T>();

            byte[] payload = _serializer.Serialize(message);

            var envelope = new NetworkEnvelope
            {
                TypeId = typeId,
                Payload = payload
            };

            foreach (NetworkConnectionToClient connection in _serverMessenger.Connections)
            {
                if (!_subscriptions.HasSubscription(connection, typeId))
                {
                    continue;
                }

                _serverMessenger.Send(connection, envelope);
            }
        }

        private void OnSubscriptionReceived(
            NetworkConnectionToClient connection,
            SubscriptionMessage message)
        {
            _subscriptions.AddSubscription(connection, message.TypeId);

            Debug.Log(
                $"Connection {connection.connectionId} subscribed to message {message.TypeId}");
        }

        private void OnEnvelopeReceived(NetworkEnvelope envelope)
        {
            Type messageType = _registry.GetType(envelope.TypeId);

            if (!_handlers.TryGetValue(messageType, out Action<byte[]> handler))
            {
                Debug.LogWarning(
                    $"No handler registered for message '{messageType.Name}'.");

                return;
            }

            handler.Invoke(envelope.Payload);
        }
    }
}
