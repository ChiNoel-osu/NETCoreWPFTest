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

namespace LocalFileExplorer.View
{
	/// <summary>
	/// DirBoxAndContent.xaml 的交互逻辑
	/// </summary>
	public partial class DirBoxAndContent : UserControl
	{
		MainViewModel _main = new MainViewModel();
		public DirBoxAndContent()
		{
			InitializeComponent();
			DataContext = _main;
		}

		private void DirBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_main.ContentVM.PATHtoShow = DirBox.Text;
		}

		private void ScaleSlider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show("TODOLIST:\nLabel size change with slider.\nWeird binding errors despite nothing happens.\nAdd to favorites button.\nAsync loading????");
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			DirBox_TextChanged(null, null);
		}

		private void Content_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count < 1)	//Nothing in selected folder
				return;
			string head = ((TreeViewItem)e.AddedItems[0]).Header.ToString();
			string path;
			if (DirBox.Text.EndsWith('\\'))
				path = DirBox.Text + head;
			else
				path = DirBox.Text + '\\' + head;
			_main.ContentVM.SelectedPath = path;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DirBox.Text = _main.FavoritesVM.CBBoxSelected.ToolTip.ToString();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("This button isn't working yet!","HOLD UP");
		}
	}
}
