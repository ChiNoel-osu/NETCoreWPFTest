using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.IO;

namespace LocalFileExplorer.ViewModel
{
	public class FavoritesVM
	{
		public Dictionary<string, string> FavItem = new Dictionary<string, string>();
		private ComboBoxItem _cbBoxSelected = new ComboBoxItem();
		public ComboBoxItem CBBoxSelected
		{
			get
			{
				_cbBoxSelected.ToolTip = FavItem[_cbBoxSelected.Content.ToString()];
				return _cbBoxSelected;
			}
			set
			{ _cbBoxSelected = value; }
		}
		public string DirBoxText { get; set; }
		private ObservableCollection<ComboBoxItem> _comboBoxItems = new ObservableCollection<ComboBoxItem>();
		public ObservableCollection<ComboBoxItem> ComboBoxItems
		{
			get
			{
				//Read Favorite file
				string favPath = Directory.GetCurrentDirectory() + "\\Favorites.txt";
				if (!File.Exists(favPath))
					File.Create(favPath).Close();   //The Close() ensures that it has been created.
				string[] favTexts = File.ReadAllLines(favPath);
				bool isOddLine = true; string tempName = string.Empty;
				foreach (string str in favTexts)
				{
					if (isOddLine)
					{
						tempName = str.Substring(str.LastIndexOf('|') + 1);
					}
					else
					{
						FavItem.Add(tempName, str.Substring(str.LastIndexOf('|') + 1));
						ComboBoxItem comboBoxItem = new ComboBoxItem() { Content = tempName };
						_comboBoxItems.Add(comboBoxItem);
					}
					isOddLine = !isOddLine;
				}
				return _comboBoxItems;
			}
			set { _comboBoxItems = value; }
		}
	}
}