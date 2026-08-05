using Mirror;
using UnityEngine;
using Zenject;

namespace Networking
{
    public sealed class NetworkBootstrap : MonoBehaviour
    {
        private Services.INetworkMessageService _networkService;

        [Inject]
        private void Construct(Services.INetworkMessageService networkService)
        {
            _networkService = networkService;
        }

        private void Awake()
        {
            Debug.Log("NetworkBootstrap initialized");
        }

        public void OnServerStarted()
        {
            _networkService.RegisterServerHandlers();
        }

        public void OnClientStarted()
        {
            _networkService.RegisterClientHandlers();
        }
    }
}
