using System;
using System.Windows.Input;

namespace ScreenRecognition.Desktop.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc)
        {
            _execute = executeAction;
            _canExecute = canExecuteFunc;
        }

        public DelegateCommand(Action<object> executeAction) : this(executeAction, null) { }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }

            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
