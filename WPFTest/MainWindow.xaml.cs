using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Threading;
//DebugFileEZMode targetting AliceInCradle_v020s
namespace WPFTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string aicDEBUGPATH = null; //This is where the actual txt debug file is
		DispatcherTimer timer = new DispatcherTimer();
		public MainWindow()
		{
			InitializeComponent();
			Opacity = 0;
			timer.Tick += new EventHandler(FadeInForm);
			timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
			timer.Start();
		}
		private void buttonExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		private void buttonMaxorRestore_Click(object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Normal)
				WindowState = WindowState.Maximized;
			else if (WindowState == WindowState.Maximized)
				WindowState = WindowState.Normal;
		}
		private void buttonMini_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}
		private void DragToMove_LMouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
		private void openFileBtn_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
			//ofd.DefaultExt = ".txt";
			ofd.Filter = "Executable (*.exe)|*.exe|Text File (*.txt)|*.txt";
			if (ofd.ShowDialog() == true)
			{
				dirBox.Text = ofd.FileName;
				dirBox.BorderBrush = Brushes.Gray;
			}
			else
			{
				dirBox.Text = "<User canceled or an error occured.>";
				dirBox.BorderBrush = Brushes.Red;
			}
		}
		private void dirBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			saveButton.IsEnabled = true;    //The sign of a valid path
			bool isAICDir(string aicSAPath)
			{
				string dbgFileLocation = aicSAPath;
				string[] filesInAICSA;
				try
				{ filesInAICSA = Directory.GetFiles(dbgFileLocation); }
				catch (Exception)
				{ saveButton.IsEnabled = false; return false; }
				//Detects AIC Directory
				for (int i = 0; i < filesInAICSA.Length; i++)   //Gets file name
					filesInAICSA[i] = filesInAICSA[i].Substring(filesInAICSA[i].LastIndexOf('\\') + 1);
				if (filesInAICSA.Contains("_debug.txt"))
				{
					fileBox.Text = File.ReadAllText(dbgFileLocation + "\\_debug.txt");
					aicDEBUGPATH = dbgFileLocation + "\\_debug.txt";
					//Create backup just in case
					File.WriteAllText("_debug.txt.BACKUP", File.ReadAllText(aicDEBUGPATH));
					return true;
				}
				else
				{
					saveButton.IsEnabled = false;
					return false;
				}
			}
			if (dirBox.Text.EndsWith("\\\\")) //User error
			{
				notAIC.Content = "Are u trying to kill me?";
				notAIC.Foreground = Brushes.Red;
				saveButton.IsEnabled = false;
			}
			else if (dirBox.Text.EndsWith(".exe"))
			{
				dirBox.BorderBrush = Brushes.Gray;  //Reset status
				string aicDir = dirBox.Text.Remove(dirBox.Text.LastIndexOf('\\') + 1);
				string aicDebugFileLocaton = aicDir + "AliceInCradle_Data\\StreamingAssets";
				if (isAICDir(aicDebugFileLocaton))
				{
					notAIC.Content = "AIC Directory Found.";
					notAIC.Foreground = Brushes.FloralWhite;
				}
				else
				{
					notAIC.Content = "The target directory doesn't seem to be a valid AIC directory....";
					notAIC.Foreground = Brushes.Red;
					saveButton.IsEnabled = false;
				}
			}
			else if (dirBox.Text.EndsWith('\\')) //It's a directory
			{
				dirBox.BorderBrush = Brushes.Gray;  //Reset status
				string aicDebugFileLocaton = dirBox.Text + "AliceInCradle_Data\\StreamingAssets";
				if (isAICDir(aicDebugFileLocaton))
				{
					notAIC.Content = "AIC Directory Found.";
					notAIC.Foreground = Brushes.FloralWhite;
				}
				else
				{
					notAIC.Content = "The target directory doesn't seem to be a valid AIC directory....";
					notAIC.Foreground = Brushes.Red;
					saveButton.IsEnabled = false;
				}
			}
			else    //It's a file
			{
				try     //Check if the file can be opened
				{
					saveButton.IsEnabled = Directory.GetParent(dirBox.Text).ToString().EndsWith("StreamingAssets") ? true : false;
					fileBox.Text = File.ReadAllText(dirBox.Text);
					dirBox.BorderBrush = Brushes.Gray;  //Reset status
					if (!saveButton.IsEnabled)
					{
						notAIC.Content = "You're opening a random text file, stuff's not going to work.";
						notAIC.Foreground = Brushes.Yellow;
					}
				}
				catch (Exception)   //System.IO.FileNotFoundException is the main cause
				{
					dirBox.BorderBrush = Brushes.Red;
					notAIC.Content = "The target directory doesn't seem to be a valid AIC directory....";
					notAIC.Foreground = Brushes.Red;
					saveButton.IsEnabled = false;
				}
			}
			//Work with _debug.txt, extract lines.
			if (saveButton.IsEnabled)
			{
				UseDebug.IsEnabled = true;
				string[] dbgContent = File.ReadAllLines(aicDEBUGPATH);
				foreach (string dbgOption in dbgContent)
				{
					if (dbgOption == "") continue;  //Empty line
					//Seperating Option and its state, if current line is comment, option will be "//" and state False
					string option = dbgOption.Remove(dbgOption.IndexOf(' ') + 1).Split(' ')[0];
					bool state = dbgOption.Substring(dbgOption.IndexOf(' ')).Split(' ')[1] == "1" ? true : false;
					switch (option)
					{
						case "<DEBUG>":
							UseDebug.IsChecked = state;
							break;
						case "nosnd":
							nosnd.IsChecked = state;
							break;
						case "reloadmtr":
							reloadmtr.IsChecked = state;
							break;
						case "nocfg":
							nocfg.IsChecked = state;
							break;
						case "mighty":
							mighty.IsChecked = state;
							break;
						case "nodamage":
							nodamage.IsChecked = state;
							break;
						case "weak":
							weak.IsChecked = state;
							break;
						case "allskill":
							allskill.IsChecked = state;
							break;
						case "supercyclone":
							supercyclone.IsChecked = state;
							break;
						default:
							break;
					}
				}
			}
			else
			{
				UseDebug.IsEnabled = false;
				UseDebug.IsChecked = false;
			}
		}
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if (aicDEBUGPATH != null)
			{
				//Same, extract lines.
				//Read text from filebox will cause unwanted new lines to come up, help.
				//string[] savingContent = fileBox.Text.Split("\n");
				string[] savingContent;
				try
				{
					savingContent = File.ReadAllLines(aicDEBUGPATH);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return;
				}
				byte lineIndex = 0;
				byte[] changingLinesIndex = new byte[9];    //Get the lines that are going to change
				CheckBox[] checkBoxes = { UseDebug, nosnd, reloadmtr, nocfg, mighty, nodamage, weak, allskill, supercyclone };
				foreach (string savingOption in savingContent)
				{
					if (savingOption == "")
					{ lineIndex++; continue; }  //Empty line
					string option = savingOption.Remove(savingOption.IndexOf(' ') + 1).Split(' ')[0];
					switch (option)
					{
						case "<DEBUG>":
							changingLinesIndex[0] = lineIndex;
							break;
						case "nosnd":
							changingLinesIndex[1] = lineIndex;
							break;
						case "reloadmtr":
							changingLinesIndex[2] = lineIndex;
							break;
						case "nocfg":
							changingLinesIndex[3] = lineIndex;
							break;
						case "mighty":
							changingLinesIndex[4] = lineIndex;
							break;
						case "nodamage":
							changingLinesIndex[5] = lineIndex;
							break;
						case "weak":
							changingLinesIndex[6] = lineIndex;
							break;
						case "allskill":
							changingLinesIndex[7] = lineIndex;
							break;
						case "supercyclone":
							changingLinesIndex[8] = lineIndex;
							break;
						default:
							break;
					}
					lineIndex++;
				}
				lineIndex = 0;  //Reuse, it's now checkbox index
				foreach (byte index in changingLinesIndex)  //Replace option with checkbox status
					savingContent[index] = savingContent[index].Replace(savingContent[index].Remove(savingContent[index].IndexOf(' ') + 1) + savingContent[index][savingContent[index].IndexOf(' ') + 1], savingContent[index].Remove(savingContent[index].IndexOf(' ') + 1) + Convert.ToByte(checkBoxes[lineIndex++].IsChecked));
				if (notAIC.Foreground == Brushes.Red)   //Will not trigger now....it's fixed.
					MessageBox.Show("Invalid directory or file, saved to fallback path: " + aicDEBUGPATH, "Hold on.", MessageBoxButton.OK, MessageBoxImage.Information);
				try		//Now gonna save the file
				{
					//WriteAllLines will make the debug file CRLF instead of LF only.
					//Thus a new line will be at the end of the file.
					File.WriteAllLines(aicDEBUGPATH, savingContent);
					savedLabel.Foreground = Brushes.Lime;
					savedLabel.Visibility = Visibility.Visible;
					timer.Start();  //Saved text delay timer
				}
				catch (Exception)
				{
					savedLabel.Content = "Save Failed.";
					savedLabel.Foreground = Brushes.Red;
					savedLabel.Visibility = Visibility.Visible;
					timer.Start();
				}
			}
			else
			{
				savedLabel.Content = "ERROR";
				savedLabel.Foreground = Brushes.Red;
				savedLabel.Visibility = Visibility.Visible;
				timer.Start();
			}
		}
		private void timer_Tick(object sender, EventArgs e)
		{
			timer.Stop();
			savedLabel.Visibility = Visibility.Hidden;
			savedLabel.Content = "File Saved!";
			savedLabel.Foreground = Brushes.Lime;
		}
		private void FadeInForm(object sender, EventArgs e)
		{
			if (Opacity >= 1)
			{
				timer.Stop();   //this stops the timer if the form is completely displayed
				timer.Interval = new TimeSpan(0, 0, 2);
				timer.Tick += new EventHandler(timer_Tick);
			}
			else
				Opacity += 0.5;
		}
		private void UseDebug_Change(object sender, RoutedEventArgs e)
		{
			nosnd.IsEnabled = reloadmtr.IsEnabled = nocfg.IsEnabled = mighty.IsEnabled = nodamage.IsEnabled = weak.IsEnabled = allskill.IsEnabled = supercyclone.IsEnabled = (bool)((CheckBox)sender).IsChecked;
		}
	}
}