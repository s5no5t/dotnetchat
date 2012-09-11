
using System;
using System.Collections.Generic;
using System.Linq;
using DotNetChatServer.Properties;
using Lidgren.Network;

namespace DotNetChatServer
{
    class ChatServer
    {
        private NetServer _netServer;
        private List<Member> _member;
        private int _currentMaxId;

        public ChatServer()
        {
            StartServer(Settings.Default.AppIdentifier, Settings.Default.Port);
        }

        public void StartServer(string appIdentifier, int port)
        {
            if (_netServer == null)
            {
                var peerConfiguration = new NetPeerConfiguration(appIdentifier)
                {
                    AcceptIncomingConnections = true,
                    Port = port,
                };
                _netServer = new NetServer(peerConfiguration);
            }
            else
            {
                Console.Write("Shutdown current server...");
                _netServer.Shutdown("");
                Console.WriteLine("done");
            }

            Console.Write("Starting Server...");

            Settings.Default.AppIdentifier = appIdentifier;
            Settings.Default.Port = port;
            Settings.Default.Save();

            _member = new List<Member>();
            _currentMaxId = 1;
   
            _netServer.Start();
            Console.WriteLine("done");
            Console.WriteLine("ServerName: '{0}' Port: {1}", Settings.Default.ServerName, port);
            Running = true;
        }

        public bool Running { get; private set; }

        public void Update()
        {
            NetIncomingMessage msg;
            while ((msg = _netServer.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        SendDiscoveryResponse(msg);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChanged(msg);
                        break;
                }
            }
        }

        private void HandleStatusChanged(NetIncomingMessage msg)
        {
            NetConnectionStatus connectionStatus = (NetConnectionStatus)msg.ReadByte();

            if (connectionStatus == NetConnectionStatus.Connected)
            {
                string memberName = msg.SenderConnection.RemoteHailMessage.ReadString();
                Member member = new Member(_currentMaxId++, msg.SenderEndpoint) {Name = memberName};
                _member.Add(member);
                Console.WriteLine("Member '{0}' joined the Chat", memberName);
            }
            else if (connectionStatus == NetConnectionStatus.Disconnected)
            {
                Member member = _member.FirstOrDefault(m => m.Ip == msg.SenderEndpoint);
                if (member != null)
                {
                    _member.Remove(member);
                    Console.WriteLine("Member '{0}' disconnected.", member.Name);
                }
            }
        }

        private void SendDiscoveryResponse(NetIncomingMessage msg)
        {
            NetOutgoingMessage response = _netServer.CreateMessage();
            response.Write(Settings.Default.ServerName);
            _netServer.SendDiscoveryResponse(response, msg.SenderEndpoint);
        }
    }
}
