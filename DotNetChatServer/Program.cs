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
            using (var chatServer = new ChatServer(Properties.Settings.Default.AppIdentifier, 
                                                   Properties.Settings.Default.Port))
            {
                chatServer.Start();

                while (true)
                {
                    chatServer.Update();
                    Thread.Sleep(200);
                }
            }
        }
    }
}
