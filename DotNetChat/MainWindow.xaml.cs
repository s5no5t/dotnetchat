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

            _dotNetChatViewModel = new DotNetChatViewModel();

            DataContext = _dotNetChatViewModel;

            _chatService = new ChatService(Properties.Settings.Default.AppIdentifier);
            _chatService.Connect(Properties.Settings.Default.Port);

            _chatService.MemberJoined += AddMember;
        }

        private void AddMember(object sender, MemberJoinedHandlerArgs args)
        {
            _dotNetChatViewModel.AddMember(new MemberViewModel{Name = args.Name});
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            _chatService.Disconnect();
        }
    }
}
