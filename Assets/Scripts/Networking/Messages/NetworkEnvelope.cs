using Mirror;

namespace Networking.Messages
{
    /// <summary>
    /// Universal container for all user messages.
    /// </summary>
    public struct NetworkEnvelope : NetworkMessage
    {
        /// <summary>
        /// Message type identificator.
        /// </summary>
        public ushort TypeId;

        /// <summary>
        /// Serialized message data.
        /// </summary>
        public byte[] Payload;
    }
}
