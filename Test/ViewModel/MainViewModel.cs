using System;
using System.Collections.Generic;
using System.Text;

namespace Test.ViewModel
{
	public class MainViewModel : VMBase
	{
		public TestVM TestVM { get; set; }
		public TestListBoxVM CtSource { get; set; }
		public MainViewModel()
		{
			TestVM = new TestVM();
			CtSource = new TestListBoxVM();
		}
	}
}
