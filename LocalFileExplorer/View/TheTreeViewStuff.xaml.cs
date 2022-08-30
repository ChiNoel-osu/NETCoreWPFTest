using System;
using System.IO;
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
using LocalFileExplorer.Model;
using LocalFileExplorer.ViewModel;

namespace LocalFileExplorer.View
{
	/// <summary>
	/// TheTreeViewStuff.xaml 的交互逻辑
	/// </summary>
	public partial class TheTreeViewStuff : UserControl
	{
		public TheTreeViewStuff()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
		}
		private void LeftTreeViewUCLoaded(object sender, RoutedEventArgs e)
		{
			DirShit logicalDrive = new DirShit();
			foreach (string drives in logicalDrive.GetLDrives())
			{
				TreeViewItem item = new TreeViewItem();
				item.Header = drives;   //Only for displaying
				item.Tag = drives;  //The node's tag (Actual path) is this
				item.Items.Add(null);   //Add dummy item so it can expand, will clear later.
				item.Expanded += FolderExpanded;    //Subscribe event.
				FolderView.Items.Add(item); //Add the logical drives into FolderView
			}
		}
		private async void FolderExpanded(object sender, RoutedEventArgs e)   //This only runs when expanding
		{
			DirShit subItem = new DirShit();
			TreeViewItem folder = (TreeViewItem)sender;
			if (folder.Items.Count != 1 || folder.Items[0] != null)
				return; //If the folder has been expanded before and thus have sth in it, just return.
			folder.Items.Clear();   //If not, it would be the first time expanding bc of the null dummy item.

			//string[] directories = subItem.DirInPath((string)folder.Tag);
			//string[] files = subItem.FileInPath((string)folder.Tag);
			string[] directories = await subItem.DirInPathTask((string)folder.Tag);
			string[] files = await subItem.FileInPathTask((string)folder.Tag);
			//It is indeed doing it asynchronizely but it doesn't matter in this case.
			if (directories != null && files != null)
			{
				foreach (string directory in directories)   //Add every dir found
				{
					TreeViewItem newDir = new TreeViewItem();   //Create a new newDir object of type TreeViewItem bc this is what we're goin to add.
					newDir.Header = subItem.GetFileFolderName(directory) + '\\';
					newDir.Tag = directory;
					newDir.Items.Add(null);
					newDir.Expanded += FolderExpanded;  //When the current node gets expanded, it calls FolderExpanded. This is a set property.
					folder.Items.Add(newDir);   //Add the folders into folder into folder into folder
				}
				foreach (string file in files)
				{
					TreeViewItem newFile = new TreeViewItem();
					newFile.Header = subItem.GetFileFolderName(file);
					newFile.Tag = file;
					folder.Items.Add(newFile);  //Add the files into folder
				}
			}
			else
				MessageBox.Show("u can't do this my friend", "it's null");
		}

		private void FolderView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			//The e actually represents the treeviewitem, how useful.
			MainViewModel.TVVM.TESTBOX = ((TreeViewItem)(e.NewValue)).Tag.ToString();
		}
	}
}