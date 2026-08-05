using Mirror;
using Networking;

public class CustomNetworkManager : NetworkManager
{
    public NetworkBootstrap Bootstrap;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Bootstrap.OnServerStarted();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Bootstrap.OnClientStarted();
    }
}
