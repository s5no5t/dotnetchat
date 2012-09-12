
using DotNetChat.ViewModels;

namespace DotNetChat
{
    public partial class MainWindow
    {
        private readonly DotNetChatViewModel _dotNetChatViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _dotNetChatViewModel = new DotNetChatViewModel();
            _dotNetChatViewModel.AddMember(new MemberViewModel(1) {Name = "Peter"});
            _dotNetChatViewModel.AddChatEntry(new ChatEntryViewModel("Peter", "Hello World"));

            DataContext = _dotNetChatViewModel;
        }
    }
}
