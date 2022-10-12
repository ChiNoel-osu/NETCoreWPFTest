using LocalFileExplorer.ViewModel.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LocalFileExplorer.ViewModel
{
	public class PhotoViewerVM : VMBase
	{
		private readonly string path;
		private ushort _LSI;
		public ushort ListSelectedIndex   //Shared between PhotoViewer & ImageControl, and they need to be in the same DataContext.
		{
			get
			{ return _LSI; }
			set
			{
				_LSI = value;
				OnPropertyChanged(nameof(ListSelectedIndex));
			}
		}
		public ShowInExplorerCMD ShowInExplorerCMD { get; } = new ShowInExplorerCMD();
		private ushort _imageCount;
		public ushort ImageCount
		{
			get
			{
				if(_imageCount == 1)	//1 pictures
					return 1;
				else if(_imageCount == 0)	//Sorting errors
					return 0;
				else
					return (ushort)(_imageCount - 1);	//Index issues so subtract by 1
			}
			private set
			{ _imageCount = value; }
		}	//Bind target for slider maximum.
		public class CustomListItem //Use this as the ListBoxItem binding target
		{
			public BitmapImage Image { get; set; }
			public string Name { get; set; }
			public string Path { get; set; }
		}
		private ObservableCollection<CustomListItem> images = new ObservableCollection<CustomListItem>();
		public ObservableCollection<CustomListItem> Images
		{
			get
			{
				AddImgs(path);
				return images;
			}
			private set { images = value; }
		}
		private void AddImgs(string path)
		{
			IEnumerable<string> imgs;
			string[] allowedExt = { ".jpg", ".png", ".jpeg", ".gif" };
			try
			{   //Get image files
				imgs = Directory.EnumerateFiles(path, "*.*").Where(s => allowedExt.Any(s.ToLower().EndsWith));
			}
			catch (InvalidOperationException) { return; }

			//Extract the numbers in the file name and sort the image based on them.
			//When used as hentai viewer it's fine, but will cause problem when viewing large amout
			//of pictures that may have the same number in its file name.
			//TODO: Sorting mode configurable.
			Dictionary<int, string> map = new Dictionary<int, string>();
			short noNumberFix = 0;
			foreach (string filePath in imgs)
			{
				string fileNameWOExt = filePath.Substring(filePath.LastIndexOf('\\') + 1);
				fileNameWOExt = fileNameWOExt.Remove(fileNameWOExt.LastIndexOf('.'));
				if (Int32.TryParse(fileNameWOExt, out int id))
				{
					try
					{
						map.Add(id, filePath);
					}
					catch (Exception e)
					{
						MessageBox.Show("Error when sorting pictures:\n" + e.Message + "\nYou can use right click to open the folder in explorer.", "Oops.");
						return;
					}
				}
				else
				{
					string extractedNumber = string.Empty;
					for (ushort i = 0; i < fileNameWOExt.Length; i++)
						if (Char.IsDigit(fileNameWOExt[i]))
							extractedNumber += fileNameWOExt[i];
					int num;
					try
					{
						num = ushort.Parse(extractedNumber);
					}
					catch (OverflowException)
					{   //The number is to big, use the last 3 numbers.
						num = ushort.Parse(extractedNumber.Substring(extractedNumber.Length - 3));
					}
					catch (FormatException)
					{   //There's no number in the file name
						num = --noNumberFix;
					}
					try
					{
						map.Add(num, filePath);
					}
					catch (Exception e)
					{
						MessageBox.Show("Error when sorting pictures:\n" + e.Message + "\nYou can use right click to open the folder in explorer.", "Oops.");
						return;
					}
				}
			}
			IOrderedEnumerable<KeyValuePair<int, string>> sortedMap = map.OrderBy(x => x.Key);

			foreach (KeyValuePair<int, string> img in sortedMap)
			{
				CustomListItem imgItem = new CustomListItem();
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = new Uri(img.Value);
				bitmapImage.DecodePixelWidth = 128; //TODO: make it configurable.
				bitmapImage.EndInit();
				imgItem.Path = img.Value;
				imgItem.Name = img.Value.Substring(img.Value.LastIndexOf('\\') + 1);
				imgItem.Image = bitmapImage;
				images.Add(imgItem);
				_imageCount++;
			}
		}
		public PhotoViewerVM(string folderPath)
		{
			this.path = folderPath;
		}
	}
}
