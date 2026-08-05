using Mirror;
using Networking;
using Networking.Services;
using Zenject;

public class CustomNetworkManager : NetworkManager
{
    private INetworkMessageService _networkService;

    [Inject]
    private void Construct(INetworkMessageService networkService)
    {
        _networkService = networkService;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        _networkService.RegisterServerHandlers();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        _networkService.RegisterClientHandlers();
    }
}
