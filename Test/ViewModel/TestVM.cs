using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Test.Command;

namespace Test.ViewModel
{
	public class TestVM : VMBase
	{
		private string _upString;
		public string UPString
		{
			get { return _upString; }
			set
			{
				_upString = value;
				OnPropertyChanged(nameof(UPString));
			}
		}
		private string _dnString;
		public string DNString
		{
			get { return _dnString; }
			set
			{
				_dnString = value;
				OnPropertyChanged(nameof(DNString));
			}
		}
		public ICommand TestCommand { get; }
		public TestVM()
		{
			TestCommand = new TestCommand(this);
		}
	}
}
