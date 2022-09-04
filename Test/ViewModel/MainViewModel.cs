using System;
using System.Collections.Generic;
using System.Text;

namespace Test.ViewModel
{
	public class MainViewModel : VMBase
	{
		public TestVM TestVM { get; set; }
		public VMBase CurrentVM { get; }
		public MainViewModel()
		{
			TestVM = new TestVM();
			CurrentVM = TestVM;
		}
	}
}
