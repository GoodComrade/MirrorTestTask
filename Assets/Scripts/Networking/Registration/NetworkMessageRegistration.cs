using Networking.Messages;
using Networking.Registry;

namespace Networking.Registration
{
    public sealed class NetworkMessageRegistration
    {
        public NetworkMessageRegistration(NetworkMessageRegistry registry)
        {
            RegisterMessages(registry);
        }

        private static void RegisterMessages(NetworkMessageRegistry registry)
        {
            registry.Register<HelloMessage>(NetworkMessageIds.Hello);

            // Add here another message types registration.
            //
            // registry.Register<DamageMessage>(NetworkMessageIds.Damage);
            // registry.Register<InventoryMessage>(NetworkMessageIds.Inventory);
            // registry.Register<ChatMessage>(NetworkMessageIds.Chat);
        }
    }
}
