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
            _dotNetChatViewModel.AddMember(new MemberViewModel(1) {Name = "Peter"});
            _dotNetChatViewModel.AddChatEntry(new ChatEntryViewModel("Peter", "Hello World"));

            DataContext = _dotNetChatViewModel;

            _chatService = new ChatService(Properties.Settings.Default.AppIdentifier);
            _chatService.Connect(Properties.Settings.Default.Port);
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
