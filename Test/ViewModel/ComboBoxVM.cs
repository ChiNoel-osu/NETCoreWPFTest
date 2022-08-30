using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Test.Model;

namespace Test.ViewModel
{
	public class ComboBoxVM:ViewModelBase
	{
		private string _pathToDelay;
		public string PathToDelay
		{
			get { return _pathToDelay; }
			set
			{
				_pathToDelay = value;
				OnPropertyChange(nameof(DelayDirs));
			}
		}
		ObservableCollection<string> _delayDirs = new ObservableCollection<string>();
		public ObservableCollection<string> DelayDirs
		{
			get
			{
				_delayDirs.Clear();
				if (string.IsNullOrEmpty(PathToDelay))
					return _delayDirs;
				string[] sth = new DoSth().SthToAsync(PathToDelay).Result;
				if (sth != null)
					foreach (string dir in sth)
						_delayDirs.Add(dir);
				return _delayDirs;
			}
		}

	}
}
