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
    public sealed class NetworkMessageService : INetworkMessageService
    {
        public event Action<NetworkConnectionToClient, ushort> ClientSubscribed;

        private readonly NetworkMessageRegistry _registry;
        private readonly INetworkSerializer _serializer;
        private readonly NetworkSubscriptionStorage _subscriptions;

        private readonly IServerMessenger _serverMessenger;
        private readonly IClientMessenger _clientMessenger;

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
            Debug.Log("SERVER: RegisterServerHandlers");
            _serverMessenger.RegisterSubscriptionHandler(OnSubscriptionReceived);
        }

        public void RegisterClientHandlers()
        {
            Debug.Log("CLIENT: Register handlers");
            _clientMessenger.RegisterEnvelopeHandler(OnEnvelopeReceived);
        }

        public void RegisterHandler<T>(Action<T> handler)
        {
            Type messageType = typeof(T);

            if (_handlers.ContainsKey(messageType))
                throw new InvalidOperationException($"Handler for '{messageType.Name}' is already registered.");

            _handlers.Add(messageType, payload =>
            {
                T message = (T)_serializer.Deserialize(payload, typeof(T));
                handler.Invoke(message);
            });
        }

        public void Subscribe<T>()
        {
            if (!_clientMessenger.IsActive)
                throw new InvalidOperationException("Subscriptions can only be sent from a client.");

            ushort typeId = _registry.GetId<T>();

            Debug.Log(
        $"CLIENT: Sending subscription {typeof(T).Name}, id={typeId}");
        
            _clientMessenger.Send(new SubscriptionMessage
                {
                    TypeId = typeId
                });
        }


        public void Send<T>(NetworkConnectionToClient connection, T message)
        {
            if (!_serverMessenger.IsActive)
                throw new InvalidOperationException("Messages can only be sent from the server.");

            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            ushort typeId = _registry.GetId<T>();

            Debug.Log(
    $"Checking subscription. Connection={connection.connectionId}, Type={typeId}");
            if (!_subscriptions.HasSubscription(connection, typeId))
            {
                Debug.Log(
        $"Connection {connection.connectionId} is not subscribed.");
                return;
            }

            byte[] payload = _serializer.Serialize(message);

            _serverMessenger.Send(connection, new NetworkEnvelope
            {
                TypeId = typeId,
                Payload = payload
            });

            Debug.Log(
        $"NetworkMessageService.Send called. Message={typeof(T).Name}");
        }

        public void Broadcast<T>(T message)
        {
            if (!_serverMessenger.IsActive)
                throw new InvalidOperationException("Broadcast can only be called on the server.");

            ushort typeId = _registry.GetId<T>();

            byte[] payload = _serializer.Serialize(message);

            NetworkEnvelope envelope = new()
            {
                TypeId = typeId,
                Payload = payload
            };

            foreach (NetworkConnectionToClient connection in _serverMessenger.Connections)
            {
                if (!_subscriptions.HasSubscription(connection, typeId))
                    continue;

                _serverMessenger.Send(connection, envelope);
            }
        }

        private void OnSubscriptionReceived(
            NetworkConnectionToClient connection,
            SubscriptionMessage message)
        {
            Debug.Log(
        $"NetworkMessageService: Subscription received. Connection={connection.connectionId}, Type={message.TypeId}");

            if (_subscriptions.HasSubscription(connection, message.TypeId))
                return;

            _subscriptions.AddSubscription(connection, message.TypeId);

            ClientSubscribed?.Invoke(connection, message.TypeId);
        }

        private void OnEnvelopeReceived(NetworkEnvelope envelope)
        {
            if (!_registry.TryGetType(envelope.TypeId, out Type messageType))
            {
                Debug.LogWarning($"Unknown message id '{envelope.TypeId}'.");
                return;
            }

            if (!_handlers.TryGetValue(messageType, out Action<byte[]> handler))
            {
                Debug.LogWarning($"No handler registered for '{messageType.Name}'.");
                return;
            }

            handler.Invoke(envelope.Payload);
        }
    }
}
