using LocalFileExplorer.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace LocalFileExplorer.View
{
	/// <summary>
	/// ImageControl.xaml 的交互逻辑
	/// </summary>
	public partial class ImageControl : UserControl
	{
		Window parentWnd;
		WindowStyle previousStyle;
		WindowState previousState;
		public ImageControl()
		{
			InitializeComponent();
		}
		private void UCLoaded(object sender, RoutedEventArgs e)
		{   //Gets the parent window.
			parentWnd = Window.GetWindow((DependencyObject)sender);
			ShowInExplorer.ToolTip = parentWnd.Tag;	//The tag is the opened folderPath.
			ImgPosition.Focus();
		}
		private void MaximizeClick(object sender, RoutedEventArgs e)
		{   //Please do not use Win+UpArrow to maximize;
			if (parentWnd.WindowStyle != WindowStyle.None)
			{   //Not in Fullscreen mode
				previousStyle = parentWnd.WindowStyle;
				previousState = parentWnd.WindowState;
				parentWnd.WindowStyle = WindowStyle.None;
				parentWnd.WindowState = WindowState.Maximized;
				MaximizeBtn.Content = '⇲';
			}
			else
			{
				parentWnd.WindowStyle = previousStyle;
				parentWnd.WindowState = previousState;
				MaximizeBtn.Content = '⇱';
			}
		}

		private void PreviousClick(object sender, RoutedEventArgs e)
		{
			if(ImgPosition.Value>0)
				ImgPosition.Value--;
		}
		private void ImgPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			//Get the reference of parent window's data context.
			PhotoViewerVM parentVM = (PhotoViewerVM)parentWnd.DataContext;
		}
		private void NextClick(object sender, RoutedEventArgs e)
		{
			ImgPosition.Value++;
		}
	}
}
