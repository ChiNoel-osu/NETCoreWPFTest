using LocalFileExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocalFileExplorer.View
{
	/// <summary>
	/// PhotoViewer.xaml 的交互逻辑
	/// </summary>
	public partial class PhotoViewer : Window
	{
		public PhotoViewer(string folderPath)
		{
			InitializeComponent();
			this.Tag = folderPath;  //Set tag so ImageControl can use it.
			DataContext = new PhotoViewerVM(folderPath);
		}

		private void ImageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GC.Collect();
			BitmapImage bigImage = new BitmapImage();
			bigImage.BeginInit();
			bigImage.UriSource = new Uri(((PhotoViewerVM.CustomListItem)e.AddedItems[0]).Path);
			bigImage.EndInit();
			BigImage.Source = bigImage;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					//I dont wanna cover all the states the user will figure it out.
					if (WindowState == WindowState.Maximized)
						WindowState = WindowState.Normal;
					else
						this.Close();
					break;
			}
		}

		private void BigImage_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0 && BigImageScaleFactor.ScaleX < 7)
				BigImageScaleFactor.ScaleX = BigImageScaleFactor.ScaleY += 0.3;
			else if (e.Delta < 0 && BigImageScaleFactor.ScaleX > 1)
				BigImageScaleFactor.ScaleX = BigImageScaleFactor.ScaleY -= 0.3;
			else
				return;
		}

		private void BigImage_MouseMove(object sender, MouseEventArgs e)
		{   //Oh my god this shit acts real time.
			Point mousePosOnImg = e.GetPosition(BigImage);
			BigImage.RenderTransformOrigin = new Point(mousePosOnImg.X / BigImage.ActualWidth, mousePosOnImg.Y / BigImage.ActualHeight);
		}
	}
}
