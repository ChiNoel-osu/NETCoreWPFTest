using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Test.ViewModel.Command
{
	public class MessageBoxCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;
		private Action _msgAction;
		public MessageBoxCommand(Action msgAction)
		{
			_msgAction = msgAction;
		}
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_msgAction.Invoke();
		}
	}
}
