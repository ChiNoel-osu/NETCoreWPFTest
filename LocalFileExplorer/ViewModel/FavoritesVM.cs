using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Windows;

namespace LocalFileExplorer.ViewModel
{
	public class FavoritesVM
	{
		private Dictionary<string, string> FavItem = new Dictionary<string, string>();
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
		private ObservableCollection<ComboBoxItem> _comboBoxItems = new ObservableCollection<ComboBoxItem>();
		public ObservableCollection<ComboBoxItem> ComboBoxItems
		{
			get
			{	//Read Favorite file
				string favPath = Directory.GetCurrentDirectory() + "\\Favorites.txt";
				if (!File.Exists(favPath))
					File.Create(favPath).Close();	//The Close() ensures that it has been created.
				string[] favTexts = File.ReadAllLines(favPath);
				List<string> nameList = new List<string>();
				bool isOddLine = true;	bool isDuplicate = false;	bool duplicateFound = false;
				string tempName = string.Empty;
				//Clear everything first to refresh.
				FavItem.Clear();
				_comboBoxItems.Clear();
				foreach (string str in favTexts)
				{
					if (isDuplicate)
					{
						isDuplicate = false;
						continue;	//Skip twice
					}
					if (isOddLine)	//Getting Name
					{
						tempName = str.Substring(str.LastIndexOf('|') + 1);
						if (nameList.Contains(tempName))    //Check for duplicates
						{
							isDuplicate = true;
							duplicateFound = true;
							continue;	//Skip this line
						}
						nameList.Add(tempName);
					}
					else	//Getting Path
					{
						FavItem.Add(tempName, str.Substring(str.LastIndexOf('|') + 1));
						ComboBoxItem comboBoxItem = new ComboBoxItem() { Content = tempName };
						_comboBoxItems.Add(comboBoxItem);
					}
					isOddLine = !isOddLine;
				}
				if (duplicateFound)
					MessageBox.Show("Duplicated name found in Favorites.txt and are ignored, please fix that.", "NO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return _comboBoxItems;
			}
			set { _comboBoxItems = value; }
		}
	}
}