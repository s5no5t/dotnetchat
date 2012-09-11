
using DotNetChat.Framework;

namespace DotNetChat.ViewModels
{
    class ChatEntryViewModel : ViewModel
    {
        public ChatEntryViewModel(string name, string content)
        {
            Name = name;
            Content = content;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    FirePropertyChanged("Name");
                }
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    FirePropertyChanged("Content");
                }
            }
        }
    }
}
