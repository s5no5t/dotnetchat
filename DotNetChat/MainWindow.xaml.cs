
using DotNetChat.ViewModels;

namespace DotNetChat
{
    public partial class MainWindow
    {
        private readonly MToolChatViewModel _mToolChatViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _mToolChatViewModel = new MToolChatViewModel();
            _mToolChatViewModel.AddMember(new MemberViewModel(1) {Name = "Peter"});
            _mToolChatViewModel.AddChatEntry(new ChatEntryViewModel("Peter", "Hello World"));

            DataContext = _mToolChatViewModel;
        }
    }
}
