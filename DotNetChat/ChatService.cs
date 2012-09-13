using System;
using System.Security.Principal;
using DotNetChatShared;
using Lidgren.Network;

namespace DotNetChat
{
    class ChatService
    {
        private NetClient _netClient;
        private readonly NetPeerConfiguration _configuration;

        public ChatService(string appIdentifier)
        {
            _configuration = new NetPeerConfiguration(appIdentifier);
            _configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
        }

        public void Connect(int port)
        {
            _netClient = new NetClient(_configuration);
            _netClient.RegisterReceivedCallback(HandleMessages);
            _netClient.Start();
            _netClient.DiscoverLocalPeers(port);
        }

        private void HandleMessages(object state)
        {
            NetIncomingMessage inc;
            while ((inc = _netClient.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        HandleDiscoveryResponse(inc);
                        break;
                    case NetIncomingMessageType.Data:
                        HandleDataMessages(inc);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void HandleDiscoveryResponse(NetIncomingMessage inc)
        {
            _netClient.Connect(inc.SenderEndpoint, _netClient.CreateMessage(WindowsIdentity.GetCurrent().Name));
        }

        private void HandleDataMessages(NetIncomingMessage inc)
        {
            var messageKind = (DataMessageType)Enum.Parse(typeof(DataMessageType), inc.ReadString());
            switch (messageKind)
            {
                case DataMessageType.MemberJoined:
                    OnMemberJoined(new MemberJoinedHandlerArgs { Name = inc.ReadString() });
                    break;
                case DataMessageType.MemberLeft:
                    OnMemberLeft(new MemberLeftHandlerArgs { Name = inc.ReadString() });
                    break;
                case DataMessageType.MessageReceived:
                    OnMessageReceived(new MessageReceivedHandlerArgs { Content = inc.ReadString() });
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void SendMessage(string message)
        {
            var netMessage = _netClient.CreateMessage();
            netMessage.Write(DataMessageType.MessageSent.ToString());
            netMessage.Write(message);
            _netClient.SendMessage(netMessage, _netClient.ServerConnection, NetDeliveryMethod.ReliableUnordered);
        }

        public void Disconnect()
        {
            _netClient.Shutdown("good bye");
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
