using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Test.Command;

namespace Test.ViewModel
{
	public class TestListBoxVM
	{
		public ICommand AddNewCmd { get; }
		private ObservableCollection<string> _fuckCollect = new ObservableCollection<string>();
		public ObservableCollection<string> FuckCollect
		{
			get { return _fuckCollect; }
			set { _fuckCollect = value; }
		}
		public TestListBoxVM()
		{
			AddNewCmd = new AddNewStuffCommand();
		}
	}
}
