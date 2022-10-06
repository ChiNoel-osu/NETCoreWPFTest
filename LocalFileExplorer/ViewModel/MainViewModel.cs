using System;
using System.Collections.Generic;
using System.Text;
using LocalFileExplorer.ViewModel;
using LocalFileExplorer.ViewModel.Command;

namespace LocalFileExplorer
{
	public class MainViewModel// : VMBase	//It somehow worked without VMBase
	{
		public static TreeViewVM TVVM { get; set; }
		public ContentVM ContentVM { get; set; }
		public FavoritesVM FavoritesVM { get; set; }
		public MainViewModel()
		{
			TVVM = new TreeViewVM();
			ContentVM = new ContentVM();
			FavoritesVM = new FavoritesVM();
		}
	}
}