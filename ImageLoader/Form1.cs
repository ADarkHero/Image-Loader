using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageLoader
{
	public partial class Form1 : Form
	{
		string settingsFile = "settings.txt";
		string logFile = "error.log";
		string foundLog = "found.log";

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			readSettings();
		}

		private void startProgram_Click(object sender, EventArgs e)
		{
			Thread t = new Thread(delegate ()
			{
				if (checkBoxClearLogs.Checked)
				{
					clearAllLogs();
				}

				checkAndAddEndingSlashes(); //If the webpath or folderpath is missing a / or \ at the end, it gets added here



				String missingFiles = "";
				String foundFiles = "";

				string[] readText = File.ReadAllLines(textBoxFilepath.Text);
				int lineCount = readText.Count();

				//Sets maximum value of the progress bar
				Invoke(new Action(() =>
				{
					progressBar1.Maximum = lineCount;
				}));


				//Use TLS, if the address contains https
				if (textBoxWebpath.Text.Contains("https"))
				{
					ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
				}

				//Read all filenames and try to download them
				foreach (string s in readText)
				{
					string fileName = s.Substring(0, s.IndexOf(";")); //Cut everything after ; - We can work better with csv files that way.

					if (checkBoxAddJpg.Checked)
					{
						fileName = CheckForExtensions(fileName);
					}

					string myStringWebResource = textBoxWebpath.Text + fileName; //Source (web) path
					string fileNamePath = textBoxImageFolder.Text + fileName; //Destination path		

					//Downloads images from the web
					try
					{
						using (WebClient myWebClient = new WebClient())
						{
							myWebClient.DownloadFile(myStringWebResource, fileNamePath);

							foundFiles += fileName + "\r\n";
						}
					}
					catch (Exception ex)
					{
						missingFiles += fileName + ";" + ex.ToString() + "\r\n";
					}

					//Increments progress bar
					Invoke(new Action(() =>
					{
						progressBar1.Increment(1);
					}));
				}

				//If some file were not found, write them to error log
				if (String.IsNullOrEmpty(missingFiles))
				{
					MessageBox.Show("Pictures got downloaded successfully!");
				}
				else
				{
					MessageBox.Show("There where some file that couldn't be downloaded! Take a look at the error log.");
					File.AppendAllText(logFile, missingFiles);
					File.AppendAllText(logFile, "\r\n");
					File.AppendAllText(logFile, "\r\n");
				}
				File.AppendAllText(foundLog, foundFiles);
				File.AppendAllText(foundLog, "\r\n");
				File.AppendAllText(foundLog, "\r\n");


				writeSettings();
			});
			t.Start();
		}

		/// <summary>
		/// Checks if the filename contains a file extension. If not, add .jpg.
		/// </summary>
		/// <param name="fileName">The filename, that should be checked.</param>
		/// <returns>If the filename contains a dot, return it unchanged. If not, return it with a .jpg at the end.</returns>
		private string CheckForExtensions(string fileName)
		{
			if (!fileName.Contains("."))
			{
				return fileName + ".jpg";
			}
			return fileName;
		}

		private void checkAndAddEndingSlashes()
		{
			if (!textBoxWebpath.Text.EndsWith("/"))
				textBoxWebpath.Text = textBoxWebpath.Text + "/";

			if (!textBoxImageFolder.Text.EndsWith("\\"))
				textBoxImageFolder.Text = textBoxImageFolder.Text + "\\";
		}

		private void readSettings()
		{
			if (File.Exists(settingsFile))
			{
				string[] readText = File.ReadAllLines(settingsFile);
				textBoxWebpath.Text = readText[0];
				textBoxFilepath.Text = readText[1];
				textBoxImageFolder.Text = readText[2];
				checkBoxClearLogs.Checked = Convert.ToBoolean(readText[3]);
				checkBoxAddJpg.Checked = Convert.ToBoolean(readText[4]);
			}
		}

		private void writeSettings()
		{
			File.WriteAllText("settings.txt", textBoxWebpath.Text);
			File.AppendAllText("settings.txt", "\r\n");
			File.AppendAllText("settings.txt", textBoxFilepath.Text);
			File.AppendAllText("settings.txt", "\r\n");
			File.AppendAllText("settings.txt", textBoxImageFolder.Text);
			File.AppendAllText("settings.txt", "\r\n");
			File.AppendAllText("settings.txt", checkBoxClearLogs.Checked.ToString());
			File.AppendAllText("settings.txt", "\r\n");
			File.AppendAllText("settings.txt", checkBoxAddJpg.Checked.ToString());
		}



		private void chooseFilePath_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				textBoxFilepath.Text = openFileDialog.InitialDirectory + openFileDialog.FileName;
				writeSettings();
			}
		}



		private void chooseImageFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog objDialog = new FolderBrowserDialog();
			DialogResult objResult = objDialog.ShowDialog(this);
			if (objResult == DialogResult.OK)
			{
				textBoxImageFolder.Text = objDialog.SelectedPath;
				writeSettings();
			}
		}

		private void openErrorLog_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(logFile);
			}
			catch (Exception ex)
			{
				File.AppendAllText(logFile, ex.ToString());
			}
		}

		private void openImageFolder_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(textBoxImageFolder.Text);
			}
			catch (Exception ex)
			{
				File.AppendAllText(logFile, ex.ToString());
			}
		}

		private void openSuccessLog_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(foundLog);
			}
			catch (Exception ex)
			{
				File.AppendAllText(logFile, ex.ToString());
			}
		}

		private void clearLogs_Click(object sender, EventArgs e)
		{
			clearAllLogs();
		}
		public void clearAllLogs()
		{
			try
			{
				File.WriteAllText(logFile, String.Empty);
				File.WriteAllText(foundLog, String.Empty);
			}
			catch (Exception ex)
			{
				File.AppendAllText(logFile, ex.ToString());
			}
		}

		private void checkBoxClearLogs_CheckedChanged(object sender, EventArgs e)
		{
			writeSettings();
		}

		private void checkBoxAddJpg_CheckedChanged(object sender, EventArgs e)
		{
			writeSettings();
		}
	}
}
