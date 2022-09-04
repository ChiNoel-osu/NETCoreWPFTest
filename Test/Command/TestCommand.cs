using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Test.ViewModel;

namespace Test.Command
{
	public class TestCommand : CMDBase
	{
		private readonly TestVM testVM;
		public TestCommand(TestVM testVM)
		{
			this.testVM = testVM;
		}
		public override void Execute(object parameter)
		{
			MessageBox.Show(testVM.DNString,testVM.UPString);
		}
	}
}
