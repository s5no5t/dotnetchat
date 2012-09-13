using System;
using System.Linq;
using DotNetChat.ViewModels;

namespace DotNetChat
{
    public partial class MainWindow
    {
        private readonly DotNetChatViewModel _dotNetChatViewModel;
        private readonly ChatService _chatService;

        public MainWindow()
        {
            InitializeComponent();

            _chatService = new ChatService(Properties.Settings.Default.AppIdentifier);

            _dotNetChatViewModel = new DotNetChatViewModel(_chatService);
            DataContext = _dotNetChatViewModel;

            _chatService.Connect(Properties.Settings.Default.Port);
            _chatService.MemberJoined += AddMember;
            _chatService.MemberLeft += RemoveMember;
            _chatService.MessageReceived += MessageReceived;
        }

        private void MessageReceived(object sender, MessageReceivedHandlerArgs args)
        {
            _dotNetChatViewModel.AddChatEntry(new ChatEntryViewModel("", args.Content));
        }

        private void AddMember(object sender, MemberJoinedHandlerArgs args)
        {
            _dotNetChatViewModel.AddMember(new MemberViewModel{Name = args.Name});
        }

        private void RemoveMember(object sender, MemberLeftHandlerArgs args)
        {
            _dotNetChatViewModel.RemoveMember(_dotNetChatViewModel.Members.First(m => m.Name == args.Name));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            _chatService.Disconnect();
        }
    }
}
