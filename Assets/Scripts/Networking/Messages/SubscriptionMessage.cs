using Mirror;

namespace Networking.Messages
{
    /// <summary>
    /// Tell to server whay type of messages client wants to recieve.
    /// </summary>
    public struct SubscriptionMessage : NetworkMessage
    {
        public ushort TypeId;
    }
}
