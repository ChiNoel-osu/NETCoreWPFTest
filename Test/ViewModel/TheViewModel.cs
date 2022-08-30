using System;
using System.Collections.Generic;
using System.Text;
using Test.ViewModel;
namespace Test
{
	public class TheViewModel : ViewModelBase
	{
		public WhatDatabindVM BIND { get; private set; }
		public TextSizeVM TextSizeShit { get; private set; }
		public MesasgeBoxVMC BindThisToShow { get; private set; }
		public ComboBoxVM PathDelay { get; private set; }
		public TheViewModel()
		{
			BIND = new WhatDatabindVM();
			TextSizeShit = new TextSizeVM();
			BindThisToShow = new MesasgeBoxVMC();
			PathDelay = new ComboBoxVM();
		}
		public void ChangeTextSize(short current, short diff)
		{
			TextSizeShit.CurrentTextSize = current;
			TextSizeShit.Diff = diff;
		}
	}
}