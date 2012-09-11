using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using DotNetChat.Properties;

namespace DotNetChat.ChatModel
{
    class ChatModel
    {
        //private NetPeer _peer;

        //public void Connect()
        //{
        //    NetPeerConfiguration configuration = new NetPeerConfiguration(Settings.Default.AppIdentifier);
        //    _peer = new NetPeer(configuration);
        //    _peer.RegisterReceivedCallback(s => HandleMessages());
        //    _peer.DiscoverLocalPeers(Settings.Default.Port);
        //}

        //private void HandleMessages()
        //{
        //    NetIncomingMessage inc;
        //    while ((inc = _peer.ReadMessage()) != null)
        //    {
        //        switch (inc.MessageType)
        //        {
        //            case NetIncomingMessageType.StatusChanged:
        //                break;
        //            case NetIncomingMessageType.DiscoveryRequest:
        //                break;
        //            case NetIncomingMessageType.DiscoveryResponse:
        //                SendDiscoveryResponse
        //                break;
        //        }
        //    }
        //}

        //private void SendDiscoveryResponse()
        //{
        //    NetOutgoingMessage response = _server.CreateMessage();
        //    response.Write(_serverName);
        //    response.Write((byte)_clients.Count);
        //    response.Write(_currentState != GameServerState.Lobby);
        //    response.WritePadBits(7);
        //    _server.SendDiscoveryResponse(response, serverMessage.SenderEndpoint);   
        //}
    }
}
