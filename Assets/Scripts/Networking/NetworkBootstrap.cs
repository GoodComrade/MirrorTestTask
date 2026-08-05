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
