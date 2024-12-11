using System;
using System.Windows.Input;

namespace ClientAccounts.ViewModels
{
    /// <summary>
    /// Класс команд реализует ICommand (методы bool CanExecute, void Execute и событие EventHandler CanExecuteChanged)
    /// readonly - для того, чтоб установить однократно в констр-ре, запрет на случайн. изм. логики команды
    /// </summary>
    internal class RelayCommand : ICommand 
	{
        readonly Action<object> _execute;  
        readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public void Execute(object parameter) => _execute(parameter);
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public event EventHandler? CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}		
	}
}
