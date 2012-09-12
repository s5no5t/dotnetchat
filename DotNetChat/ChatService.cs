using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using Lidgren.Network;
using DotNetChat.Properties;

namespace DotNetChat
{
    class ChatService
    {
        private NetPeer _peer;
        private readonly NetPeerConfiguration _configuration;
        private NetConnection _netConnection;

        public ChatService(string appIdentifier)
        {
            _configuration = new NetPeerConfiguration(appIdentifier);
            _configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
        }

        public void Connect(int port)
        {
            _peer = new NetPeer(_configuration);
            _peer.RegisterReceivedCallback(HandleMessages);
            _peer.Start();
            _peer.DiscoverLocalPeers(port);
        }

        private void HandleMessages(object state)
        {
            NetIncomingMessage inc;
            while ((inc = _peer.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        var message = _peer.CreateMessage();
                        message.Write(WindowsIdentity.GetCurrent().Name);
                        _netConnection = _peer.Connect(inc.SenderEndpoint, message);
                        break;
                    case NetIncomingMessageType.Data:
                        var name = inc.ReadString();
                        OnMemberJoined(new MemberJoinedHandlerArgs{Name = name});
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        //private void SendDiscoveryResponse()
        //{
        //    NetOutgoingMessage response = _server.CreateMessage();
        //    response.Write(_serverName);
        //    response.Write((byte)_clients.Count);
        //    response.Write(_currentState != GameServerState.Lobby);
        //    response.WritePadBits(7);
        //    _server.SendDiscoveryResponse(response, serverMessage.SenderEndpoint);
        //}

        public void Disconnect()
        {
            _peer.Shutdown("good bye");
        }

        internal event MemberJoinedHandler MemberJoined;

        private void OnMemberJoined(MemberJoinedHandlerArgs args)
        {
            MemberJoinedHandler handler = MemberJoined;
            if (handler != null) handler(this, args);
        }
    }

    internal delegate void MemberJoinedHandler(object sender, MemberJoinedHandlerArgs args);

    internal class MemberJoinedHandlerArgs
    {
        public string Name { get; set; }
    }
}
