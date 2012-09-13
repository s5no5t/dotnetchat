using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using DotNetChatServer;
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
                        var messageKind = (MessageKinds)Enum.Parse(typeof(MessageKinds), inc.ReadString());
                        switch (messageKind)
                        {
                            case MessageKinds.MemberJoined:
                                {
                                    var name = inc.ReadString();
                                    OnMemberJoined(new MemberJoinedHandlerArgs { Name = name });
                                    break;
                                }
                            case MessageKinds.MemberLeft:
                                {
                                    var name = inc.ReadString();
                                    OnMemberLeft(new MemberLeftHandlerArgs { Name = name });
                                    break;
                                }
                            case MessageKinds.MessageReceived:
                                {
                                    var content = inc.ReadString();
                                    OnMessageReceived(new MessageReceivedHandlerArgs { Content = content });
                                    break;
                                }
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void SendMessage(string message)
        {
            var netMessage = _peer.CreateMessage();
            netMessage.Write(MessageKinds.MessageSent.ToString());
            netMessage.Write(message);
            _netConnection.SendMessage(netMessage, NetDeliveryMethod.ReliableUnordered, 0);
        }

        public void Disconnect()
        {
            _peer.Shutdown("good bye");
        }

        internal event MessageReceivedHandler MessageReceived;
        internal event MemberJoinedHandler MemberJoined;
        internal event MemberLeftHandler MemberLeft;

        private void OnMessageReceived(MessageReceivedHandlerArgs args)
        {
            MessageReceivedHandler handler = MessageReceived;
            if (handler != null) handler(this, args);
        }

        private void OnMemberJoined(MemberJoinedHandlerArgs args)
        {
            MemberJoinedHandler handler = MemberJoined;
            if (handler != null) handler(this, args);
        }

        private void OnMemberLeft(MemberLeftHandlerArgs args)
        {
            MemberLeftHandler handler = MemberLeft;
            if (handler != null) handler(this, args);
        }
    }

    internal delegate void MessageReceivedHandler(object sender, MessageReceivedHandlerArgs args);

    internal class MessageReceivedHandlerArgs
    {
        public string Content { get; set; }
    }

    internal delegate void MemberLeftHandler(object sender, MemberLeftHandlerArgs args);

    internal class MemberLeftHandlerArgs
    {
        public string Name { get; set; }
    }

    internal delegate void MemberJoinedHandler(object sender, MemberJoinedHandlerArgs args);

    internal class MemberJoinedHandlerArgs
    {
        public string Name { get; set; }
    }
}
