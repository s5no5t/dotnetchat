
using DotNetChat.Framework;

namespace DotNetChat.ViewModels
{
    class MemberViewModel : ViewModel
    {
        public MemberViewModel(int id)
        {
            Id = id;
        }

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

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    FirePropertyChanged("Id");
                }
            }
        }
    }
}
