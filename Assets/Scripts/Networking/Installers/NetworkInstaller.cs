using Networking.Mirror;
using Networking.Registry;
using Networking.Serialization;
using Networking.Services;
using Networking.Subscription;
using Zenject;

namespace Networking.Installers
{
    public sealed class NetworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkMessageRegistry>()
                .AsSingle();

            Container.Bind<INetworkSerializer>()
                .To<JsonNetworkSerializer>()
                .AsSingle();

            Container.Bind<NetworkSubscriptionStorage>()
                .AsSingle();

            Container.Bind<IServerMessenger>()
                .To<MirrorServerMessenger>()
                .AsSingle();

            Container.Bind<IClientMessenger>()
                .To<MirrorClientMessenger>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<NetworkMessageService>()
                .AsSingle();
        }
    }
}
