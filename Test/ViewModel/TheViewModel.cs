using System;
using System.Collections.Generic;
using System.Text;
using Test.ViewModel;
using Test.View;
using Test.Model;

namespace Test
{
	public class TheViewModel : ViewModelBase
	{
		string shit()
		{
			DoSth doSth = new DoSth();
			doSth.Name = "BRUH";
			return doSth.Name;
		}
		public string BoundContent => shit();

	}
}