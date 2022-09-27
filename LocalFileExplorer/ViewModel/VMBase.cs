using System.ComponentModel;

namespace LocalFileExplorer.ViewModel
{
	public class VMBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			//The '?' is used for nullcheck.
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
