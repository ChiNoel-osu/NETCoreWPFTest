using System.Windows;
using System.Windows.Controls;
using Test.ViewModel;

namespace Test.View
{
	/// <summary>
	/// TestView.xaml 的交互逻辑
	/// </summary>
	public partial class TestView : UserControl
	{
		public static MainViewModel _main = new MainViewModel();
		LeftNavigation1 LN1 = new LeftNavigation1();
		LeftNavigation2 LN2 = new LeftNavigation2();
		public TestView()
		{
			InitializeComponent();
			DataContext = _main;
		}
		private void N1Click(object sender, RoutedEventArgs e)
		{
			_main.CtSource.TheLBEntry();
			//CCNavi.Content = LN1;
		}
		private void N2Click(object sender, RoutedEventArgs e)
		{
			CCNavi.Content = LN2;
		}
	}
}
