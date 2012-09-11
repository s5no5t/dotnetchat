
using System.Net;

namespace DotNetChatServer
{
    class Member
    {
        public string Name { get; set; }
        public int Id { get; private set; }
        public IPEndPoint Ip { get; private set; }

        public Member(int id, IPEndPoint ip)
        {
            Id = id;
            Ip = ip;
        }
    }
}
