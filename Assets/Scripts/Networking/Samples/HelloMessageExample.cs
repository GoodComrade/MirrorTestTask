using Mirror;
using Networking.Messages;
using Networking.Services;
using Mirror;
using Networking.Messages;
using Networking.Services;
using UnityEngine;
using Zenject;

namespace Networking.Samples
{
    public sealed class HelloMessageSample : MonoBehaviour
    {
        private INetworkMessageService _networkService;


        [Inject]
        private void Construct(INetworkMessageService networkService)
        {
            _networkService = networkService;
        }


        private void Start()
        {
            _networkService.RegisterHandler<HelloMessage>(OnHelloMessage);

            _networkService.ClientSubscribed += OnClientSubscribed;

            NetworkClient.OnConnectedEvent += OnConnected;
        }


        private void OnDestroy()
        {
            NetworkClient.OnConnectedEvent -= OnConnected;

            if (_networkService != null)
            {
                _networkService.ClientSubscribed -= OnClientSubscribed;
            }
        }


        private void OnConnected()
        {
            Debug.Log("Client connected. Sending subscription.");

            _networkService.Subscribe<HelloMessage>();
        }


        private void OnHelloMessage(HelloMessage message)
        {
            Debug.Log($"Received from server: {message.Text}");
        }


        private void OnClientSubscribed(
            NetworkConnectionToClient connection,
            ushort typeId)
        {
            Debug.Log($"Subscription received: {typeId}");

            _networkService.Send(
                connection,
                new HelloMessage
                {
                    Text = "Hello Client!"
                });
        }
    }
}
