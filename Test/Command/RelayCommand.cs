using System;
using System.Windows.Input;

namespace Test.Command
{
	public class RelayCommand : ICommand
	{
		readonly Action<object> _execute;
		readonly Predicate<object> _canexecute;
		public RelayCommand(Action<object> execute, Predicate<object> canexecute)
		{
			if (execute == null)
				throw new NullReferenceException("execute");
			_execute = execute;
			_canexecute = canexecute;
		}
		public RelayCommand(Action<object> execute):this(execute, null)	{}
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		public bool CanExecute(object parameter)
		{
			return _canexecute == null ? true : _canexecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute.Invoke(parameter);
		}
	}
}
