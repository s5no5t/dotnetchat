
using System.Collections.ObjectModel;
using DotNetChat.Framework;

namespace DotNetChat.ViewModels
{
    class DotNetChatViewModel : ViewModel
    {
        private readonly ObservableCollection<MemberViewModel> _members;
        public ReadOnlyObservableCollection<MemberViewModel> Members {get{ return new ReadOnlyObservableCollection<MemberViewModel>(_members);}}

        private readonly ObservableCollection<ChatEntryViewModel> _chatEntries;

        public ReadOnlyObservableCollection<ChatEntryViewModel> ChatEntries { get { return new ReadOnlyObservableCollection<ChatEntryViewModel>(_chatEntries); } }

        public DotNetChatViewModel()
        {
            _members = new ObservableCollection<MemberViewModel>();
            _chatEntries = new ObservableCollection<ChatEntryViewModel>();
        }

        public void AddMember(MemberViewModel member)
        {
            _members.Add(member);
        }

        public void RemoveMember(MemberViewModel member)
        {
            _members.Remove(member);
        }

        public void AddChatEntry(ChatEntryViewModel chatEntry)
        {
            _chatEntries.Add(chatEntry);
        }

        private string _currentContent;

        public string CurrentContent
        {
            get { return _currentContent; }
            set
            {
                if (_currentContent != value)
                {
                    _currentContent = value;
                    FirePropertyChanged("CurrentContent");
                }
            }
        }
    }
}
