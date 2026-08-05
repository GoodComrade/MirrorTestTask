using Mirror;
using UnityEngine;

namespace Networking.Samples
{
    public sealed class NetworkTestGUI : MonoBehaviour
    {
        private void OnGUI()
        {
            if (!NetworkClient.active && !NetworkServer.active)
            {
                if (GUI.Button(
                    new Rect(20, 20, 150, 40),
                    "Host"))
                {
                    NetworkManager.singleton.StartHost();
                }

                if (GUI.Button(
                    new Rect(20, 70, 150, 40),
                    "Client"))
                {
                    NetworkManager.singleton.networkAddress = "localhost";
                    NetworkManager.singleton.StartClient();
                }
            }


            if (NetworkClient.isConnected)
            {
                GUI.Label(
                    new Rect(20, 120, 300, 40),
                    "Client connected");
            }


            if (NetworkServer.active)
            {
                GUI.Label(
                    new Rect(20, 160, 300, 40),
                    "Server active");
            }
        }
    }
}
