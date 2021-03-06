﻿using System;
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
                    string fileName = "";
                    if (s.Contains(";"))
                    {
                        fileName = s.Substring(0, s.IndexOf(";")); //Cut everything after ; - We can work better with csv files that way.
                    }
                    else
                    {
                        fileName = s;
                    }

					if (checkBoxAddJpgBefore.Checked)
					{
						fileName = CheckForExtensions(fileName);
					}

                    //Wildcard, if the filename is not on the urls end
                    string myStringWebResource = "";
                    if (textBoxWebpath.Text.Contains("%%%")){
                        myStringWebResource = textBoxWebpath.Text.Replace("%%%", fileName); //Source (web) path
                    }
                    else
                    {
                        myStringWebResource = textBoxWebpath.Text + fileName; //Source (web) path
                    }

                    //Add http, if it doesn't already start with it
                    if (!myStringWebResource.StartsWith("http"))
                    {
                        myStringWebResource = "http://" + myStringWebResource;
                    }

                    //Add .jpg after download
                    if (checkBoxAddJpgAfter.Checked)
                    {
                        fileName = fileName + ".jpg";
                    }

                    string fileNamePath = textBoxImageFolder.Text + CheckFilepathForIllegalParameters(fileName); //Destination path		

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
					MessageBox.Show("Files got downloaded successfully!");
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
		/// Removes illegal characters from a file path.
		/// Also lowers the paths length to 260 characters.
		/// </summary>
		/// <param name="Input">The input, that should be checked for illegal chars.</param>
		/// <returns></returns>
		private string CheckFilepathForIllegalParameters(string Input)
		{
			String path = System.Text.RegularExpressions.Regex.Replace(Input, @"[\\/:*?""<>|]", string.Empty);
			if (path.Length > 260)
			{
				path = path.Substring(259);
			}
			return path;
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
				checkBoxAddJpgBefore.Checked = Convert.ToBoolean(readText[4]);
                checkBoxAddJpgAfter.Checked = Convert.ToBoolean(readText[5]);
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
			File.AppendAllText("settings.txt", checkBoxAddJpgBefore.Checked.ToString());
            File.AppendAllText("settings.txt", "\r\n");
            File.AppendAllText("settings.txt", checkBoxAddJpgAfter.Checked.ToString());
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

        /// <summary>
        /// These methods display help messages for the user, if they click the help-buttons.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Declare the base path of the webpage, where you want to download files from, here.\r\n\r\n" +
                "For example:\r\n" +
                "http://www.google.com/ \r\n\r\n" +
                "You can use %%% as a wildcard, if the file name is not at the end of the webpage. For example:\r\n" +
                "http://www.google.com/%%%/somethingElse");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Declare the filelist, you want to use, here.\r\n" +
                "Click the three dots, to open a file-browse-dialog and choose the file this way.\r\n" +
                "The file list ist a txt/csv file, where every entry is seperated by an enter. You must not put additional information in the filelist.\r\n\r\n" +
                "" +
                "A file list could look like this:\r\n" +
                "test01.jpg\r\n" +
                "test02.jpg\r\n" +
                "test.03\r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Declare the folder, where the program should download the files to.\r\n" +
                 "Click the three dots, to open a file-browse-dialog and choose the folder this way.");
        }
    }
}
