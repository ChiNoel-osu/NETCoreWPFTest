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
			DataContext = new TreeViewVM();
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
				item.Expanded += FolderExpanded;
				FolderView.Items.Add(item);	//Add the logical drives into FolderView
			}
		}
		private void FolderExpanded(object sender, RoutedEventArgs e)	//This only runs when use click expand
		{
			DirShit subItem = new DirShit();
			TreeViewItem folder = (TreeViewItem)sender;
			if (folder.Items.Count != 1 || folder.Items[0] != null)
				return;
			folder.Items.Clear();
			string[] directories = subItem.DirInPath((string)folder.Tag);
			string[] files = subItem.FileInPath((string)folder.Tag);
			foreach (string directory in directories)   //Add every dir found
			{
				TreeViewItem newDir = new TreeViewItem();	//Create a new newDir object of type TreeViewItem bc this is what we're goin to add.
				newDir.Header = subItem.GetFileFolderName(directory) + '\\';
				newDir.Tag = directory;
				newDir.Items.Add(null);
				newDir.Expanded += FolderExpanded;  //When the current node gets expanded, it calls FolderExpanded. This is a set property.
				folder.Items.Add(newDir);	//Add the folders into folder into folder into folder
			}
			foreach (string file in files)
			{
				TreeViewItem newFile = new TreeViewItem();
				newFile.Header = subItem.GetFileFolderName(file);
				newFile.Tag = file;
				folder.Items.Add(newFile);	//Add the files into folder
			}

		}
	}
}
