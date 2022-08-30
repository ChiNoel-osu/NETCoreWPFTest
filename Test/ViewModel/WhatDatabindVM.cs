using System;
using System.Collections.Generic;
using System.Text;
using Test.ViewModel;

namespace Test
{
	public class WhatDatabindVM : ViewModelBase
	{
		private string _strBind;
		public string StrBind
		{
			get
			{
				if (string.IsNullOrEmpty(_strBind))
					return "NOTHING HERE BRO";
				else
					return _strBind;
			}
			set
			{
				_strBind = value;
				OnPropertyChange("StrBind");
			}
		}
	}
}
