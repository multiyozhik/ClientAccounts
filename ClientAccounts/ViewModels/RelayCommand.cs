using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientAccounts.ViewModels
{
	internal class RelayCommand : ICommand // реализует методы CanExecute, Execute и событие CanExecuteChanged
	{
        readonly Action<object> _execute;  // !readonly-чтоб установить 1 раз в констр-ре, запрет на случайн. изм. логики команды
        readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
		
		public event EventHandler? CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}
		public bool CanExecute(object parameter)=> _canExecute?.Invoke(parameter) ?? true;
		public void Execute(object parameter)=> _execute(parameter);		
	}
}
