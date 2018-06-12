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

        String missingFiles = "";
        String foundFiles = "";
        Thread myThread;

        public Form1()
        {
            InitializeComponent();
        }





        /// <summary>
        /// Loads the main form. Prepares the second thread for the main method.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            readSettings();
            myThread = new System.Threading.Thread(new ThreadStart(downoadDataFromWeb));
        }








        /// <summary>
        /// This method generates a second thread, that does the main work.
        /// If the thread is alive, this method kills it.
        /// </summary>
        private void mainMethod()
        {
            checkAndAddEndingSlashes(); //If the webpath or folderpath is missing a / or \ at the end, it gets added here

            //If the thread is running, end it. If it is not running, start it.
            if (myThread.IsAlive)
            {
                myThread.Interrupt();
                myThread.Abort();
                startProgram.BackColor = Color.Red;
                startProgram.Text = "Download aborted! Click again to restart.";
            }
            else
            {
                myThread = new System.Threading.Thread(new ThreadStart(downoadDataFromWeb));
                myThread.Start();
                startProgram.BackColor = Color.Gold;
                startProgram.Text = "Currently downloading. Click again to stop!";
            }

            writeSettings();
        }





        /// <summary>
        /// Reads the input text file line by line.
        /// Downloads the files from the internet.
        /// </summary>
        private void downoadDataFromWeb()
        {
            string[] readText = File.ReadAllLines(textBoxFilepath.Text);

            MethodInvoker resetProgress = new MethodInvoker(() => progressBar.Value = 0); //Reset the progressbar to zero
            progressBar.Invoke(resetProgress);

            MethodInvoker setMaximum = new MethodInvoker(() => progressBar.Maximum = readText.Length); //How many lines does the document have? -> Maximum value for progress-bar
            progressBar.Invoke(setMaximum);

            foreach (string s in readText)
            {
                string myStringWebResource = textBoxWebpath.Text + s; //Source (web) path
                string fileName = textBoxImageFolder.Text + s; //Destination path

                //Downloads images from the web
                try
                {
                    using (WebClient myWebClient = new WebClient())
                    {
                        myWebClient.DownloadFileCompleted += DownloadCompleted;
                        myWebClient.DownloadFileAsync(new Uri(myStringWebResource), fileName);

                        foundFiles += s + "\r\n";
                    }
                }
                catch (Exception ex)
                {
                    MethodInvoker perfStep = new MethodInvoker(() => progressBar.PerformStep()); //Increase progressbar value
                    progressBar.Invoke(perfStep);
                    missingFiles += s + " \r\n";
                }

                
            }

        }

        /// <summary>
        /// If a download completes, the progress bar... Progresses.
        /// The user will be notified, if the progress bar is at 100%.
        /// </summary>
        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MethodInvoker perfStep = new MethodInvoker(() => progressBar.PerformStep()); //Increase progressbar value
            progressBar.Invoke(perfStep);

            //If the progress bar is at 100%: write logs to files and show a message box
            if (progressBar.Value >= progressBar.Maximum)
            {
                showFinishedMessageAndWriteToLogFiles();
            }
        }



        /// <summary>
        /// This function changes the main buttons color to a light green. It also changes it's text to "Download finished!"
        /// After that, a message-box appeares to tell the user, that the program is done.
        /// A different message-box pops up, if there were any errors.
        /// Finally the error and success logs get written to a text file, so the user can view them later.
        /// </summary>
        private void showFinishedMessageAndWriteToLogFiles()
        {
            MethodInvoker dlFinishedCol = new MethodInvoker(() => startProgram.BackColor = Color.LimeGreen); //Change button color
            progressBar.Invoke(dlFinishedCol);
            MethodInvoker dlFinished = new MethodInvoker(() => startProgram.Text = "Download finished!"); //Change button text
            progressBar.Invoke(dlFinished);

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
            File.AppendAllText(foundLog, foundFiles);
            File.AppendAllText(foundLog, "\r\n");
        }



        /// <summary>
        /// To function correctly, every path has to end with an / (web path) or with \\ (explorer/windows path).
        /// This function adds the slashes, if they don't exist already.
        /// </summary>
        private void checkAndAddEndingSlashes()
        {
            if (!textBoxWebpath.Text.EndsWith("/"))
                textBoxWebpath.Text = textBoxWebpath.Text + "/";

            if (!textBoxImageFolder.Text.EndsWith("\\"))
                textBoxImageFolder.Text = textBoxImageFolder.Text + "\\";
        }





        /// <summary>
        /// Reads the web-path, file-path and image-folder-path from a text document.
        /// This allows to repeat actions with the same variables fast. The user doesn't have to input them again.
        /// </summary>
        private void readSettings()
        {
            if(File.Exists(settingsFile)){
                string[] readText = File.ReadAllLines(settingsFile);
                textBoxWebpath.Text = readText[0];
                textBoxFilepath.Text = readText[1];
                textBoxImageFolder.Text = readText[2];
            }
        }



        /// <summary>
        /// Writes the web-path, file-path and image-folder-path to a text document.
        /// The settings get red, when you restart the program.
        /// This allows to repeat actions with the same variables fast. The user doesn't have to input them again.
        /// </summary>
        private void writeSettings()
        {
            File.WriteAllText("settings.txt", textBoxWebpath.Text);
            File.AppendAllText("settings.txt", "\r\n");
            File.AppendAllText("settings.txt", textBoxFilepath.Text);
            File.AppendAllText("settings.txt", "\r\n");
            File.AppendAllText("settings.txt", textBoxImageFolder.Text);
        }




        /// <summary>
        /// If you click the button with the three dots beside the imagepath textbox a folder browse dialog opens.
        /// The dialog lets you choose, in which document the filelist lies, that should be downloaded.
        /// The filelist ist a list of strings. The strings are seperated by an enter (\r\n).
        /// The filelist must not contain additional information.
        /// </summary>
        private void chooseFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilepath.Text = openFileDialog.InitialDirectory + openFileDialog.FileName;
            }
        }




        /// <summary>
        /// If you click the button with the three dots beside the imagefolder textbox a folder browse dialog opens.
        /// The dialog lets you choose, which folder should be used to store the downloaded files.
        /// You can also set the path manually, by just writing it into the textbox.
        /// Note: This function doesn't create folders, if they don't already exist.
        /// </summary>
        private void chooseImageFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objDialog = new FolderBrowserDialog();
            DialogResult objResult = objDialog.ShowDialog(this);
            if (objResult == DialogResult.OK)
            {
                textBoxImageFolder.Text = objDialog.SelectedPath;
            }    
        }


        




        /// <summary>
        /// Opens the folder, where the images will be downloaded to.
        /// </summary>
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


        /// <summary>
        /// Opens the error log.
        /// The error log contains every unsuccessfully downloaded file.
        /// </summary>
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


        /// <summary>
        /// Opens the success log.
        /// The success log contains every successfully downloaded file.
        /// </summary>
        private void openSuccessLog_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(foundLog);
            }
            catch(Exception ex)
            {

            }
        }



        /// <summary>
        /// If you click the progress bar, the main method starts.
        /// </summary>
        private void progressBar_Click(object sender, EventArgs e)
        {
            mainMethod();
        }

        /// <summary>
        /// If you click the start button, the main method starts.
        /// </summary>
        private void startProgram_Click(object sender, EventArgs e)
        {
            mainMethod();
        }

    }
}
