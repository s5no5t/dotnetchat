using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lidgren.Network;
using DotNetChat.Properties;

namespace DotNetChat
{
    class ChatService
    {
        private NetPeer _peer;
        private readonly NetPeerConfiguration _configuration;

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
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
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
    }
}
