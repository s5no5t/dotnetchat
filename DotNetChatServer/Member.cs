using Lidgren.Network;

namespace DotNetChatServer
{
    internal class Member
    {
        public string Name { get; set; }
        public NetConnection Connection { get; set; }
    }
}