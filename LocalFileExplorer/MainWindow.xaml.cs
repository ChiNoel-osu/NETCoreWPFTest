using System;
using System.Windows;

namespace LocalFileExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Closed += MainWindow_Closed;
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			foreach (Window wnd in View.DirBoxAndContent.wnds)
			{
				wnd.Close();
			}
		}
	}
}
