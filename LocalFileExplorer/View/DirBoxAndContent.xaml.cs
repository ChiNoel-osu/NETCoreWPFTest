using LocalFileExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

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
			MessageBox.Show("TODOLIST:\nWeird binding errors despite nothing happens.\nAsync loading????");
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
			AddNewFav addNewFav = new AddNewFav(DirBox.Text);
			addNewFav.Show();
			addNewFav.Closed += AddNewFav_Closed;
			AddButton.IsEnabled = false;
		}
		private void AddNewFav_Closed(object sender, EventArgs e)
		{
			FavCB.Items.Refresh();
			var thing = _main.FavoritesVM.ComboBoxItems;
			thing = null;
			GC.Collect();	//Does it delete the thing? idk.
			AddButton.IsEnabled = true;
		}
	}
}
