using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test.View
{
	/// <summary>
	/// WhatDatabind.xaml 的交互逻辑
	/// </summary>
	public partial class WhatDatabind : UserControl
	{
		public WhatDatabind()
		{
			InitializeComponent();
			DataContext = new WhatDatabindVM();
		}
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			
		}
	}
}
