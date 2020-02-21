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
			checkAndAddEndingSlashes(); //If the webpath or folderpath is missing a / or \ at the end, it gets added here



			String missingFiles = "";
			String foundFiles = "";

			string[] readText = File.ReadAllLines(textBoxFilepath.Text);
			foreach (string s in readText)
			{
				string myStringWebResource = textBoxWebpath.Text + s; //Source (web) path
				string fileName = textBoxImageFolder.Text + s; //Destination path

				ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

				//Downloads images from the web
				try
				{
					using (WebClient myWebClient = new WebClient())
					{
						myWebClient.DownloadFile(myStringWebResource, fileName);

						foundFiles += s + "\r\n";
					}
				}
				catch (Exception ex)
				{
					missingFiles += s + " \r\n";
				}
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
			}
		}

		private void writeSettings()
		{
			File.WriteAllText("settings.txt", textBoxWebpath.Text);
			File.AppendAllText("settings.txt", "\r\n");
			File.AppendAllText("settings.txt", textBoxFilepath.Text);
			File.AppendAllText("settings.txt", "\r\n");
			File.AppendAllText("settings.txt", textBoxImageFolder.Text);
		}



		private void chooseFilePath_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				textBoxFilepath.Text = openFileDialog.InitialDirectory + openFileDialog.FileName;
			}
		}



		private void chooseImageFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog objDialog = new FolderBrowserDialog();
			DialogResult objResult = objDialog.ShowDialog(this);
			if (objResult == DialogResult.OK)
			{
				textBoxImageFolder.Text = objDialog.SelectedPath;
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

			}
		}
	}
}
