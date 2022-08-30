using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;

namespace Test.Model
{
	public class DoSth
	{
		public short ChangeSize(short current, short diff)
		{
			if (current + diff < 6 || current + diff > 72)
				return current;
			else
				return (short)(current + diff);
		}

		public async Task<string[]> SthToAsync(string path)
		{
			try
			{
				string[] bruh = await Task.FromResult(longAction(path));
				return bruh;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
		}
		private string[] longAction(string path)
		{
			Thread.Sleep(2000);
			return Directory.GetDirectories(path);
		}
		public Task<string> superlong()
		{
			var omg = new Task<string>(()=> { Thread.Sleep(2000);	return "Finished Waiting."; });
			omg.Start();
			return omg;
		}
	}
}
