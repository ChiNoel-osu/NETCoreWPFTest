using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Test.Command;

namespace Test.ViewModel
{
	public class TestListBoxVM : VMBase
	{
		public ICommand AddNewCmd { get; }
		private ObservableCollection<string> _fuckCollect = new ObservableCollection<string>();
		public ObservableCollection<string> FuckCollect
		{
			get
			{
				return _fuckCollect;
			}
			set
			{
				_fuckCollect = value;
			}
		}
		public void TheLBEntry()
		{
			Task task = new Task(() =>
			{
				Thread.Sleep(500);
				_fuckCollect.Add("TheEntry");
			});
			task.Start();
		}
		public TestListBoxVM()
		{
			AddNewCmd = new AddNewStuffCommand();
			//OMGOMGOMGOM THIS WORKS SO WWELL
			BindingOperations.EnableCollectionSynchronization(_fuckCollect, new object());
		}
	}
}
