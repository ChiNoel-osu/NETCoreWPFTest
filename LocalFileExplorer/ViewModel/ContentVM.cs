using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using LocalFileExplorer.Model;

namespace LocalFileExplorer.ViewModel
{
	public class ContentVM : VMBase
	{
		private string _PATHtoShow;
		public string PATHtoShow
		{
			get { return _PATHtoShow; }
			set
			{
				_PATHtoShow = value;
				_content.Clear();    //The collection ain't gonna clear it self.
				OnPropertyChange(nameof(Content));
			}
		}
		public ushort SliderValue { get; set; } = 160;
		private string _selectedPath;
		public string SelectedPath
		{
			get { return _selectedPath; }
			set
			{
				_selectedPath = value;
				Process.Start("explorer.exe",_selectedPath);
			}
		}
		private ObservableCollection<TreeViewItem> _content = new ObservableCollection<TreeViewItem>();
		public ObservableCollection<TreeViewItem> Content
		{
			get
			{
				DirShit dirShit = new DirShit();
				if (dirShit.ContentExistsInPath(PATHtoShow))
				{
					string[] dirs = dirShit.DirInPath(PATHtoShow);
					if (dirs != null)
					{
						foreach (string dir in dirs)
						{
							FileAttributes dirAtt = new DirectoryInfo(dir).Attributes;
							if (!(dirAtt.HasFlag(FileAttributes.System) || dirAtt.HasFlag(FileAttributes.Hidden)))	//Actually hidden folders can be read now, it's handled.
							{
								string[] allowedExt = { ".jpg", ".png", ".jpeg", ".gif" };
								string firstFilePath;
								try
								{
									//Get first image file
									firstFilePath = Directory.EnumerateFiles(dir, "*.*").Where(s => allowedExt.Any(s.ToLower().EndsWith)).First();
								}
								catch (InvalidOperationException)
								{
									//No such image file, set default
									firstFilePath = Directory.GetCurrentDirectory() + "\\folder.png";
								}
								BitmapImage bitmapImage = new BitmapImage();
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri(firstFilePath);
								bitmapImage.DecodePixelWidth = SliderValue + 32;
								try
								{
									bitmapImage.EndInit();
								}
								catch (NotSupportedException)	//Bad file, ignoring.
								{
									bitmapImage = new BitmapImage();
									bitmapImage.BeginInit();
									bitmapImage.UriSource = new Uri(firstFilePath);
									bitmapImage.DecodePixelWidth = SliderValue + 32;
									bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + "\\folder.png");
									bitmapImage.EndInit();
								}
								TreeViewItem newItem = new TreeViewItem();
								newItem.ToolTip = bitmapImage;	//Use ToolTip as temp variable
								newItem.Header = dirShit.GetFileFolderName(dir);
								_content.Add(newItem);
							}
						}
					}
					return _content;
				}
				else
				{
					return _content;
				}
			}
			set { _content = value; OnPropertyChange(nameof(Content)); }
		}
	}
}
