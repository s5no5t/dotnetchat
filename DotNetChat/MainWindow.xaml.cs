using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using DotNetChat.ViewModels;

namespace DotNetChat
{
    public partial class MainWindow
    {
        private readonly DotNetChatViewModel _dotNetChatViewModel;
        private readonly ChatService _chatService;
        private bool _shiftPressed;

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
            _chatService.Connected += Connected;
        }

        private void Connected(object sender, EventArgs args)
        {
            _dotNetChatViewModel.IsConnected = true;
        }

        private void MessageReceived(object sender, MessageReceivedHandlerArgs args)
        {
            _dotNetChatViewModel.AddChatEntry(new ChatEntryViewModel(args.Name, args.Content));
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox) sender;
            var viewModel = (DotNetChatViewModel)textBox.DataContext;
            switch (e.Key)
            {
                case Key.LeftShift:
                    _shiftPressed = true;
                    break;
                case Key.Enter:
                    if (_shiftPressed)
                    {
                        viewModel.CurrentContent += Environment.NewLine;
                        // TODO: move caret
                    }
                    else
                    {
                        _chatService.SendMessage(viewModel.CurrentContent);
                        viewModel.CurrentContent = "";
                    }
                    break;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftShift:
                    _shiftPressed = false;
                    break;
            }
        }
    }
}
