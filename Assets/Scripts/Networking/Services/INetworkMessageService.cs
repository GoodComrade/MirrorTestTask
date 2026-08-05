using System;
using Mirror;

namespace Networking.Services
{
    public interface INetworkMessageService
    {
        event Action<NetworkConnectionToClient, ushort> ClientSubscribed;

        void RegisterServerHandlers();

        void RegisterClientHandlers();

        void RegisterHandler<T>(Action<T> handler);

        void Subscribe<T>();

        void Send<T>(NetworkConnectionToClient connection, T message);

        void Broadcast<T>(T message);
    }
}
