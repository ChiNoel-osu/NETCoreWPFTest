using LocalFileExplorer.Model;
using LocalFileExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LocalFileExplorer.View
{
	/// <summary>
	/// DirBoxAndContent.xaml 的交互逻辑
	/// </summary>
	public partial class DirBoxAndContent : UserControl
	{
		public static MainViewModel _main = new MainViewModel();
		public static Stack<Window> wnds = new Stack<Window>();
		public DirBoxAndContent()
		{
			InitializeComponent();
			DataContext = _main;
		}

		private void DirBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_main.ContentVM.PATHtoShow = DirBox.Text;
			if (_main.ContentVM.addItemTask != null && !_main.ContentVM.addItemTask.IsCompleted)
			{
				_main.ContentVM.cts.Cancel();   //Cancel the task to avoid old folder being added to the list
			}
			_main.ContentVM.ReGetContent();
		}

		private void ScaleSlider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{	//TODO: Feature list.
			MessageBox.Show("TODOLIST:\n" +
				"Weird binding errors despite nothing happens.\n" +
				"All the TODOs.\n" +
				"Customization.");
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			DirBox_TextChanged(null, null);
		}

		#region ListBox item event
		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			string imagePath = ((Image)sender).Source.ToString().Substring(8).Replace('/', '\\');
			string imageFolder = ((Image)sender).ToolTip.ToString();
			if (imagePath.EndsWith("folder.png"))
			{   //No image found (default folder.png), advance path.
				if (DirBox.Text.EndsWith('\\')) //Check drive root
					DirBox.Text = string.Format("{0}{1}", DirBox.Text, imageFolder);
				else
					DirBox.Text = string.Format("{0}\\{1}", DirBox.Text, imageFolder);
			}
			else
			{	//Image found, start Photo Viewer.
				string folderToView = ((Image)sender).Source.ToString().Substring(8).Replace('/', '\\');
				if (folderToView.EndsWith("folder.png")) return;    //No image in folder, return.
				folderToView = folderToView.Remove(folderToView.LastIndexOf('\\'));
				PhotoViewer photoViewer = new PhotoViewer(folderToView);
				photoViewer.Left = 0; photoViewer.Top = 0;  //Spawns window at top left corner.
				
				wnds.Push(photoViewer); //Add this to opened windows list to close it when mainwindows closes
				photoViewer.Show();
				if (((PhotoViewerVM)photoViewer.DataContext).ImageCount == 0)
					photoViewer.Close(); //No image after init, close the opened window.
				//Because the ImageCount is only used after Show() has called (It's bound to the view)
				//I have to check AFTER it's showed, which will make it confusing to users.
			}
		}
		private void Thumb_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			//Right click to Start explorer.exe
			string imageFolder = ((Image)sender).ToolTip.ToString();
			string path;
			if (DirBox.Text.EndsWith('\\'))
				path = DirBox.Text + imageFolder;
			else
				path = DirBox.Text + '\\' + imageFolder;
			//The SelectedPath changed and will run the "set property" code.
			_main.ContentVM.SelectedPath = path;
		}
		private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			string imageFolder = ((Label)sender).Content.ToString();
			if (DirBox.Text.EndsWith('\\')) //Check drive root
				DirBox.Text = string.Format("{0}{1}", DirBox.Text, imageFolder);
			else
				DirBox.Text = string.Format("{0}\\{1}", DirBox.Text, imageFolder);
		}
		#endregion

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DirBox.Text = _main.FavoritesVM.CBBoxSelected.ToolTip.ToString();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddNewFav addNewFav = new AddNewFav(DirBox.Text);
			wnds.Push(addNewFav);	//Add this to opened windows list to close it when mainwindows closes
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
			if (e.Key == Key.Back)  //Go up
				GoUP();
		}
		private void GoUpBtnClicked(object sender, RoutedEventArgs e)
		{
			GoUP();
		}

		private void GoUP()
		{
			if (DirBox.Text != string.Empty)
				if (DirBox.Text.Remove(DirBox.Text.LastIndexOf('\\')).Length == 2)
					DirBox.Text = string.Format("{0}:\\", DirBox.Text[0]);
				else
					DirBox.Text = DirBox.Text.Remove(DirBox.Text.LastIndexOf('\\'));
		}
	}
}
