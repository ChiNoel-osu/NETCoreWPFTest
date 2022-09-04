using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Test.Command
{
	public abstract class CMDBase : ICommand
	{
		public event EventHandler CanExecuteChanged;
		public virtual bool CanExecute(object parameter)	//Virtual so it can be overrided.
		{
			return true;
		}
		public abstract void Execute(object parameter);	//Abstract so it must be implemented.
		protected void OnCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
