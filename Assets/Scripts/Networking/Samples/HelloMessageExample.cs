using System.Collections;
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
            Debug.Log("HelloMessageSample: Injected");
            _networkService = networkService;
        }


        private void Start()
        {
            Debug.Log("HelloMessageSample: Start");

            _networkService.RegisterHandler<HelloMessage>(
                OnHelloMessage);


            Debug.Log("HelloMessageSample: Subscribe to ClientSubscribed event");

            _networkService.ClientSubscribed += OnClientSubscribed;


            StartCoroutine(CheckClientConnection());
        }

        private void OnDestroy()
        {
            NetworkClient.OnConnectedEvent -= Subscribe;

            if (_networkService != null)
            {
                _networkService.ClientSubscribed -= OnClientSubscribed;
            }
        }

        private IEnumerator CheckClientConnection()
        {
            while (!NetworkClient.isConnected)
            {
                yield return null;
            }

            Debug.Log("HelloMessageSample: Client connected");

            Subscribe();
        }

        private void Subscribe()
        {
            Debug.Log("HelloMessageSample: Subscribe called");

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
            Debug.Log(
        $"HelloMessageSample: ClientSubscribed. Connection={connection.connectionId}, Type={typeId}");

            _networkService.Send(
                connection,
                new HelloMessage
                {
                    Text = "Hello Client!"
                });
        }
    }
}
