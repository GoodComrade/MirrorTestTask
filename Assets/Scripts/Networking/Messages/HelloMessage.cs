
namespace Networking.Messages
{
    public struct HelloMessage : INetworkMessage
    {
        public string Text;

        public HelloMessage(string text)
        {
            Text = text;
        }
    }
}
