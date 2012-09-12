using System;
using System.Collections.Generic;
using System.Linq;
using DotNetChatServer.Properties;
using Lidgren.Network;
using NLog;

namespace DotNetChatServer
{
    internal class ChatServer : IDisposable
    {
        private readonly List<Member> _members = new List<Member>();
        private readonly NetServer _netServer;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ChatServer(string appIdentifier, int port)
        {
            var peerConfiguration = new NetPeerConfiguration(appIdentifier)
            {
                AcceptIncomingConnections = true,
                Port = port,
            };
            peerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            _netServer = new NetServer(peerConfiguration);

            MemberLogon += UpdateBuddyLists;
        }

        private void UpdateBuddyLists(object sender, MemberLogonEventArgs args)
        {
            var message = _netServer.CreateMessage();
            message.Write(MessageKinds.MemberJoined.ToString());
            message.Write(args.LoggedOnMember.Name);
            var recipients = _members.Where(m => m != args.LoggedOnMember).Select(m => m.Connection).ToList();

            if (recipients.Count > 0)
                _netServer.SendMessage(message, recipients, NetDeliveryMethod.ReliableUnordered, 0);
        }

        public void Start()
        {
            if (_netServer.Status == NetPeerStatus.Starting || _netServer.Status == NetPeerStatus.Running)
                throw new InvalidOperationException("Server is already running.");

            Logger.Info("Starting server.");

            _netServer.Start();

            Logger.Info("Server name: '{0}' Port: {1}", Settings.Default.ServerName, _netServer.Port);
        }

        public void Stop()
        {
            if (_netServer.Status != NetPeerStatus.Starting && _netServer.Status != NetPeerStatus.Running)
                throw new InvalidOperationException("Server is not running.");

            Logger.Info("Stopping server.");
            _netServer.Shutdown("good bye");
        }

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
                    case NetIncomingMessageType.ConnectionApproval:
                        break;
                    case NetIncomingMessageType.Data:
                        HandleData(msg);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void HandleData(NetIncomingMessage msg)
        {
            var messageKind = (MessageKinds) Enum.Parse(typeof(MessageKinds), msg.ReadString());
            switch (messageKind)
            {
                case MessageKinds.MessageSent:
                    var message = msg.ReadString();
                    BroadcastMessage(message);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void BroadcastMessage(string message)
        {
            var msg = _netServer.CreateMessage();
            msg.Write(MessageKinds.MessageReceived.ToString());
            msg.Write(message);
            var recipients = _members.Select(m => m.Connection).ToList();

            if (recipients.Count > 0)
                _netServer.SendMessage(msg, recipients, NetDeliveryMethod.ReliableUnordered, 0);
        }

        private void HandleStatusChanged(NetIncomingMessage msg)
        {
            var connectionStatus = (NetConnectionStatus) msg.ReadByte();

            switch (connectionStatus)
            {
                case NetConnectionStatus.Connected:
                    {
                        string memberName = msg.SenderConnection.RemoteHailMessage.ReadString();

                        // TODO: what happens if the member is already connected?

                        var member = new Member {Connection = msg.SenderConnection, Name = memberName};
                        _members.Add(member);
                        Logger.Info("Member '{0}' joined the Chat", memberName);

                        OnMemberLogon(new MemberLogonEventArgs {LoggedOnMember = member});
                        break;
                    }
                case NetConnectionStatus.Disconnected:
                    {
                        var member = _members.FirstOrDefault(m => m.Connection == msg.SenderConnection);
                        if (member != null)
                        {
                            _members.Remove(member);
                            Logger.Info("Member '{0}' disconnected.", member.Name);
                        }
                        break;
                    }
                case NetConnectionStatus.RespondedConnect:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void SendDiscoveryResponse(NetIncomingMessage msg)
        {
            NetOutgoingMessage response = _netServer.CreateMessage();
            response.Write(Settings.Default.ServerName);
            _netServer.SendDiscoveryResponse(response, msg.SenderEndpoint);
        }

        public void Dispose()
        {
            if (_netServer.Status == NetPeerStatus.Starting || _netServer.Status == NetPeerStatus.Running)
                Stop();
        }

        private event MemberLogonHandler MemberLogon;

        private void OnMemberLogon(MemberLogonEventArgs args)
        {
            MemberLogonHandler handler = MemberLogon;
            if (handler != null) handler(this, args);
        }
    }

    internal delegate void MemberLogonHandler(object sender, MemberLogonEventArgs args);

    internal class MemberLogonEventArgs
    {
        public Member LoggedOnMember { get; set; }
    }
}