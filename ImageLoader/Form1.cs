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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageLoader
{
    public partial class Form1 : Form
    {
        string settingsFile = "settings.txt";
        string logFile = "error.log";

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

            WebClient myWebClient = new WebClient();

            String missingFiles = "";

            string[] readText = File.ReadAllLines(textBoxFilepath.Text);
            foreach (string s in readText)
            {
                string myStringWebResource = textBoxWebpath.Text + s; //Source (web) path
                string fileName = textBoxImageFolder.Text + s; //Destination path

                //Downloads images from the web
                try
                {
                    myWebClient.DownloadFile(myStringWebResource, fileName);
                }
                catch (Exception ex)
                {
                    missingFiles += s + "\r\n";
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
            }
            

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
            if(File.Exists(settingsFile)){
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
            Process.Start("error.log");
        }

        private void openImageFolder_Click(object sender, EventArgs e)
        {
            Process.Start(textBoxImageFolder.Text);
        }
    }
}
