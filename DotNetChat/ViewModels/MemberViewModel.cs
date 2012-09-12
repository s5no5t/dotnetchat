
using DotNetChat.Framework;

namespace DotNetChat.ViewModels
{
    class MemberViewModel : ViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name!= value)
                {
                    _name = value;
                    FirePropertyChanged("Name");
                }
            }
        }
    }
}
