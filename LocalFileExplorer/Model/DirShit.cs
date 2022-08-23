using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LocalFileExplorer.Model
{
	public class DirShit
	{
		public string[] GetLDrives() => Directory.GetLogicalDrives();
		public string[] DirInPath(string path)
		{
			try
			{
				return Directory.GetDirectories(path);
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
		}
		public string[] FileInPath(string path)
		{
			try
			{
				return Directory.GetFiles(path);
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
		}
		public string GetFileFolderName(string path) => path.Substring(path.LastIndexOf('\\') + 1);
	}
}
