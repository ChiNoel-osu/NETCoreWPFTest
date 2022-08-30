using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using Test.ViewModel.Command;

namespace Test.ViewModel
{
	public class MesasgeBoxVMC
	{
		public string MsgBoxContent { get; set; }
		public string MsgBoxTitle { get; set; }
		public MessageBoxCommand ThisIsForMsgBox { get; private set; }
		public MesasgeBoxVMC()
		{
			ThisIsForMsgBox = new MessageBoxCommand(ShowMsgBox);
		}
		public void ShowMsgBox()
		{
			MessageBox.Show(MsgBoxContent, MsgBoxTitle);
		}
	}
}
