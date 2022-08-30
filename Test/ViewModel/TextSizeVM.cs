using System;
using System.Collections.Generic;
using System.Text;
using Test.Model;

namespace Test.ViewModel
{
	public class TextSizeVM : ViewModelBase
	{
		public short CurrentTextSize{get; set;}
		private short _diff = 0;
		public short Diff
		{
			get	//What equals Diff
			{
				return new DoSth().ChangeSize(CurrentTextSize, _diff);
			}
			set	//Diff equals What
			{
				_diff = value;
				OnPropertyChange("Diff");
			}
		}
	}
}
