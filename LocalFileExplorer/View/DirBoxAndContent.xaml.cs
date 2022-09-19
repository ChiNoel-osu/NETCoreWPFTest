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
			if (e.AddedItems.Count < 1) //Nothing in selected folder
				return;
			string head = ((TreeViewItem)e.AddedItems[0]).Header.ToString();
			//Get image path
			if (((TreeViewItem)e.AddedItems[0]).ToolTip.ToString().EndsWith("folder.png"))
			{   //No image found, advance path.
				if (DirBox.Text.EndsWith('\\'))
					DirBox.Text = string.Format("{0}{1}", DirBox.Text, head);
				else
					DirBox.Text = string.Format("{0}\\{1}", DirBox.Text, head);
			}
			else
			{   //Image found, Start explorer.exe
				string path;
				if (DirBox.Text.EndsWith('\\'))
					path = DirBox.Text + head;
				else
					path = DirBox.Text + '\\' + head;
				_main.ContentVM.SelectedPath = path;
			}
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
			//Update ItemsSource
			int beforeUpdateCnt = FavCB.Items.Count;
			int beforeUpdateIndex = FavCB.SelectedIndex;
			var thing = _main.FavoritesVM.ComboBoxItems; thing = null;
			if (beforeUpdateCnt == FavCB.Items.Count)
				FavCB.SelectedIndex = beforeUpdateIndex;
			else
				FavCB.SelectedIndex = FavCB.Items.Count - 1;
			GC.Collect();   //Does it delete the thing? idk.
			AddButton.IsEnabled = true;
		}

		private void UserControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back)	//Go up
			{
				DirBox.Text = DirBox.Text.Remove(DirBox.Text.LastIndexOf('\\'));
				if (DirBox.Text.Length == 2)    //Stuff like C: and D:
					DirBox.Text = DirBox.Text + '\\';
			}
		}
	}
}
