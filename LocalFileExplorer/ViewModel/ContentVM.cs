﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;
using LocalFileExplorer.Model;

namespace LocalFileExplorer.ViewModel
{
	public class ContentVM : VMBase
	{
		//These both to be public so it can be controlled from DirBoxAndContent.xaml.cs
		public CancellationTokenSource cts = new CancellationTokenSource();
		public Task addItemTask;
		private string _PATHtoShow;
		public string PATHtoShow
		{
			get { return _PATHtoShow; }
			set
			{ _PATHtoShow = value; }
		}
		public ushort SliderValue { get; set; } = 160;
		private string _selectedPath;
		public string SelectedPath
		{
			get { return _selectedPath; }
			set
			{
				_selectedPath = value;
				Process.Start("explorer.exe", _selectedPath);
			}
		}
		private ObservableCollection<TreeViewItem> _content = new ObservableCollection<TreeViewItem>();
		public ObservableCollection<TreeViewItem> Content
		{
			get
			{ return _content; }
			set { _content = value; OnPropertyChange(nameof(Content)); }
		}
		public void ReGetContent()
		{
			Content.Clear();
			cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;
			//Long ass task, offload to another thread.
			//This took me forever.
			addItemTask = new Task(() =>
			{
				DirShit dirShit = new DirShit();
				if (dirShit.ContentExistsInPath(PATHtoShow))
				{
					string[] dirs = dirShit.DirInPath(PATHtoShow);
					if (dirs != null)
					{
						TreeViewItem lastAdded = null;  //Mark the last added item.
						foreach (string dir in dirs)
						{
							try
							{
								ct.ThrowIfCancellationRequested();  //When task is canceled
							}
							catch (OperationCanceledException)
							{
								_content.Remove(lastAdded); //Sometimes last added item can still appear on the list, this fixes it.
								break;
							}
							FileAttributes dirAtt = new DirectoryInfo(dir).Attributes;
							if (!(dirAtt.HasFlag(FileAttributes.System) || dirAtt.HasFlag(FileAttributes.Hidden)))  //Actually hidden folders can be read now, it's handled.
							{
								string[] allowedExt = { ".jpg", ".png", ".jpeg", ".gif" };
								string firstFilePath;
								try
								{
									//Get first image file
									firstFilePath = Directory.EnumerateFiles(dir, "*.*").Where(s => allowedExt.Any(s.ToLower().EndsWith)).First();
								}
								catch (InvalidOperationException)
								{   //No such image file, set default
									firstFilePath = Directory.GetCurrentDirectory() + "\\folder.png";
								}
								catch (UnauthorizedAccessException)
								{   //Some top secret folder encounted
									MessageBox.Show("This is top-secret my friend.", "I can't do this.");
									return;
								}
								BitmapImage bitmapImage = new BitmapImage();
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri(firstFilePath);
								bitmapImage.DecodePixelWidth = SliderValue + 32;
								try
								{
									bitmapImage.EndInit();
								}
								catch (NotSupportedException)   //Bad file, ignoring.
								{
									bitmapImage = new BitmapImage();
									bitmapImage.BeginInit();
									bitmapImage.UriSource = new Uri(firstFilePath);
									bitmapImage.DecodePixelWidth = SliderValue + 128;   //Make it slightly clearer
									bitmapImage.UriSource = new Uri(Directory.GetCurrentDirectory() + "\\folder.png");
									bitmapImage.EndInit();
								}
								finally
								{
									//This is VITAL for it to be passed between threads.
									bitmapImage.Freeze();
								}
								//Items must be created in UI thread, and Dispatcher.Invoke does it.
								Application.Current.Dispatcher.Invoke(() =>
								{
									//I don't fucking know why but this has to be TreeViewItem and not ListBoxItem or else the thumbnail's not gonna show.
									//Basically I'm using TreeViewItem as a ListBoxItem. It works anyway.
									TreeViewItem newItem = new TreeViewItem();
									newItem.Tag = bitmapImage;
									newItem.Header = dirShit.GetFileFolderName(dir);
									lastAdded = newItem;
									Content.Add(lastAdded);
								});
							}
						}
					}
				}
			}, ct);
			addItemTask.Start();
		}
		public ContentVM()
		{
			BindingOperations.EnableCollectionSynchronization(Content, new object());
		}
	}
}
