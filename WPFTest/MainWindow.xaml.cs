using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;

namespace WPFTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		private void buttonExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		private void buttonMaxorRestore_Click(object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Normal)
				WindowState = WindowState.Maximized;
			else if (WindowState == WindowState.Maximized)
				WindowState = WindowState.Normal;
		}
		private void buttonMini_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}
		private void DragToMove_LMouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
		private void openFileBtn_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.DefaultExt = ".txt";
			ofd.Filter = "Text files (*.txt)|*.txt";
			if (ofd.ShowDialog() == true)
			{
				dirBox.Text = ofd.FileName;
				dirBox.BorderBrush = Brushes.Gray;
			}
			else
			{
				dirBox.Text = "<User canceled or an error occured.>";
				dirBox.BorderBrush = Brushes.Red;
			}
		}

		private void dirBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				fileBox.Text = File.ReadAllText(dirBox.Text);
				dirBox.BorderBrush = Brushes.Gray;
			}
			catch (System.IO.FileNotFoundException)
			{
				dirBox.BorderBrush = Brushes.Red;
			}
		}
	}
}
