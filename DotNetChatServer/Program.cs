using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNetChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer chatServer = new ChatServer();

            while(true)
            {
                chatServer.Update();
                Thread.Sleep(200);
            }
        }
    }
}
