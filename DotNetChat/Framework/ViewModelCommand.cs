using System;
using System.Windows.Input;

namespace DotNetChat.Framework
{
    class ViewModelCommand : ICommand
    {
        private bool _executable;
        private readonly Action _action;

        public bool Executable
        {
            get { return _executable; }
            set
            {
                if (_executable != value)
                {
                    _executable = value;
                    if (CanExecuteChanged!= null)
                        CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public ViewModelCommand(Action action)
        {
            _action = action;
            Executable = true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return Executable;
        }

        public event EventHandler CanExecuteChanged;
    }
}
