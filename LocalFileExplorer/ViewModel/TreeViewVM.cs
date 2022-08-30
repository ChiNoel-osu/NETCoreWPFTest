using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using LocalFileExplorer.ViewModel;

namespace LocalFileExplorer
{
	public class TreeViewVM : VMBase
	{
		private string _testbox;
		public string TESTBOX
		{
			get { return _testbox; }
			set { _testbox = value;
			OnPropertyChange(nameof(TESTBOX));}
		}
	}
}