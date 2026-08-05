using Mirror;
using UnityEngine;
using Zenject;

namespace Networking
{
    public sealed class NetworkBootstrap : MonoBehaviour
    {
        [Inject]
        private Services.NetworkMessageService _service;

        public void OnServerStarted()
        {
            _service.RegisterServerHandlers();
        }

        public void OnClientStarted()
        {
            _service.RegisterClientHandlers();
        }
    }
}
