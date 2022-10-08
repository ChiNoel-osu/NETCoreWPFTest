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
		private PhotoViewerVM PVVM;
		public PhotoViewer(string folderPath)
		{
			InitializeComponent();
			this.Tag = folderPath;  //Set tag so ImageControl can use it.
			PVVM = new PhotoViewerVM(folderPath);
			this.DataContext = PVVM;
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
			if (e.RightButton == MouseButtonState.Pressed)
			{
				if (e.Delta > 0 && BigImageScaleFactor.ScaleX < 7)
					BigImageScaleFactor.ScaleX = BigImageScaleFactor.ScaleY += 0.3;
				else if (e.Delta < 0 && BigImageScaleFactor.ScaleX > 1)
					BigImageScaleFactor.ScaleX = BigImageScaleFactor.ScaleY -= 0.3;
				else
					return;
			}
			else
			{
				if (e.Delta > 0 && PVVM.ListSelectedIndex > 0)
					PVVM.ListSelectedIndex--;
				else if (e.Delta < 0 && PVVM.ListSelectedIndex < PVVM.ImageCount)
					PVVM.ListSelectedIndex++;
				else
					return;
			}
		}

		private void BigImage_MouseMove(object sender, MouseEventArgs e)
		{
			double w = BigImage.ActualWidth;
			double h = BigImage.ActualHeight;
			if (e.RightButton == MouseButtonState.Pressed)
			{
				Point mousePosOnImg = e.GetPosition(BigImage);
				Point reletivePos = new Point(mousePosOnImg.X / w, mousePosOnImg.Y / h);
				BigImage.RenderTransformOrigin = reletivePos;
			}
			else
			{
				Point mousePosOnWnd = e.GetPosition(this);
				Point reletivePos = new Point(mousePosOnWnd.X / w, mousePosOnWnd.Y / h);
				reletivePos.X = BigImage.RenderTransformOrigin.X;   //Don't change X coord.
				double startYfrom = 0.4;	double startYto = 0.6;	//Set startpos reletive to window.
				if (reletivePos.Y > startYfrom && reletivePos.Y < startYto)
					reletivePos.Y = (reletivePos.Y - startYfrom) / (startYto - startYfrom);
					//X[Rescaled]=(X-X[min]/X[max]-X[min])	this rescales X to a range of [0,1]
				else
					reletivePos.Y = BigImage.RenderTransformOrigin.Y;	//Don't change Y coord.
				BigImage.RenderTransformOrigin = reletivePos;
			}
		}
	}
}
