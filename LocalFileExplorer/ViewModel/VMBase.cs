using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LocalFileExplorer.ViewModel
{
	public class VMBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChange(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
