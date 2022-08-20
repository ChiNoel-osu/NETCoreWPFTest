using System.Windows;
using System.Globalization;
using System.IO;

namespace AICDebugHelper
{
	public class Functions
	{
		static string[] langCFGContent = { "$Language=zh",
											"//更改这个选项来更换语言，\"zh\"代表中文。请不要添加任何空格。",
											"//Change the application's language here, \"en\" for English. Please avoid inserting any whitespaces." };
		public static string ReadCfgLang()
		{
			bool fileFound = false;
			string language = string.Empty;
			foreach (string filePaths in Directory.GetFiles(Directory.GetCurrentDirectory()))
			{
				if (filePaths.EndsWith("Language.cfg"))
				{
					fileFound = true;
					string[] langCFGContent = File.ReadAllLines(filePaths);
					foreach (string cfgOption in langCFGContent)
					{
						if (cfgOption.StartsWith("$Language"))
						{
							try
							{
								language = cfgOption.Substring(cfgOption.LastIndexOf('=') + 1);
								Localization.loc.Culture = new CultureInfo(language);
							}
							catch (CultureNotFoundException)
							{
								MessageBox.Show("There's something wrong with Language.cfg, please fix it or delete it for it to regenerate. Defaulting to Chinese.\nLanguage.cfg被玩坏啦，删除它以让其重新生成，请不要点炒饭。", "AAAAAAAAAAAAAAAA");
								Localization.loc.Culture = new CultureInfo("zh");
								language = "zh";
							}
						}
					}
				}
			}
			if (!fileFound)
			{
				File.AppendAllLines(Directory.GetCurrentDirectory() + "\\Language.cfg", langCFGContent);
				Localization.loc.Culture = new CultureInfo("zh");
				language = "zh";
			}
			return language;
		}
		public static void ChangeCfgLang(string culture)
		{
			bool fileFound = false;
			foreach (string filePaths in Directory.GetFiles(Directory.GetCurrentDirectory()))
			{
				if (filePaths.EndsWith("Language.cfg"))
				{
					fileFound = true;
					string[] langCFGContent = File.ReadAllLines(filePaths);
					byte contentIndex = 0;
					foreach (string cfgOption in langCFGContent)
					{
						if (cfgOption.StartsWith("$Language"))
						{
							langCFGContent[contentIndex] = cfgOption.Remove(cfgOption.LastIndexOf('=') + 1) + culture;
							Localization.loc.Culture = new CultureInfo(culture);
						}
						contentIndex++;
					}
					File.WriteAllLines("Language.cfg", langCFGContent);
				}
			}
			if (!fileFound)
			{
				MessageBox.Show("Language.cfg is gone. Regenerating....Defaulting to Chinese.\nLanguage.cfg搞丢啦！重新生成中....", "HEY!");
				File.AppendAllLines(Directory.GetCurrentDirectory() + "\\Language.cfg", langCFGContent);
				Localization.loc.Culture = new CultureInfo("zh");
			}
		}
	}
}