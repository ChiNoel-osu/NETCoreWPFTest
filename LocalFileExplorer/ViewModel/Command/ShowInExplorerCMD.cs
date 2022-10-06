using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace LocalFileExplorer.ViewModel.Command
{
	public class ShowInExplorerCMD : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			Process.Start("explorer.exe", (string)parameter);
		}
	}
}
