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

		/// <summary>
		/// Initialize form.
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Read settings on startup.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			readSettings();
		}

		/// <summary>
		/// The primary function of this program. Download files based on a list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
					progressBar1.Value = 0;
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

		/// <summary>
		/// Adds slashes to web path and image folder, if they are missing.
		/// </summary>
		private void checkAndAddEndingSlashes()
		{
			if (!textBoxWebpath.Text.EndsWith("/") && !String.IsNullOrEmpty(textBoxWebpath.Text))
			{
				Invoke(new Action(() =>
				{
					textBoxWebpath.Text = textBoxWebpath.Text + "/";
				}));
			}

			if (!textBoxImageFolder.Text.EndsWith("\\") && !String.IsNullOrEmpty(textBoxImageFolder.Text))
			{
				Invoke(new Action(() =>
				{
					textBoxImageFolder.Text = textBoxImageFolder.Text + "\\";
				}));
			}
		}

		/// <summary>
		/// Reads settings from txt file.
		/// </summary>
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

		/// <summary>
		/// Writes settings to txt file.
		/// </summary>
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


		/// <summary>
		/// Opens a dialog, which lets the user choose their filepath.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chooseFilePath_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				textBoxFilepath.Text = openFileDialog.InitialDirectory + openFileDialog.FileName;
				writeSettings();
			}
		}


		/// <summary>
		/// Opens a dialog, which lets the user choose their image folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Opens the error log.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Opens the image folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Opens the success log.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Clears all logs.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Writes the settings, if the clearlogs checkbox was changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBoxClearLogs_CheckedChanged(object sender, EventArgs e)
		{
			writeSettings();
		}

		/// <summary>
		/// Writes the settings, if the add jpg checkbox was changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBoxAddJpg_CheckedChanged(object sender, EventArgs e)
		{
			writeSettings();
		}
	}
}
