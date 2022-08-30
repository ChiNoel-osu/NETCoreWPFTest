using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Test.Model;

namespace Test.View
{
	/// <summary>
	/// WhatDatabind.xaml 的交互逻辑
	/// </summary>
	public partial class WhatDatabind : UserControl
	{
		TheViewModel _main =  new TheViewModel();
		public WhatDatabind()
		{
			InitializeComponent();
			DataContext = _main;
		}

		private void MakeTextSmaller(object sender, RoutedEventArgs e)
		{
			_main.ChangeTextSize((short)TextBlockOne.FontSize, -2);
		}

		private void MakeTextBigger(object sender, RoutedEventArgs e)
		{
			_main.ChangeTextSize((short)TextBlockOne.FontSize, 2);
		}

		private async void asyncButton_Click(object sender, RoutedEventArgs e)
		{
			asyncButton.Content = await new DoSth().superlong();
			//_main.PathDelay.PathToDelay = (string)((Button)sender).Content;
		}
	}
}
