namespace ImageLoader
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBoxWebpath = new System.Windows.Forms.TextBox();
            this.textBoxFilepath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chooseFilePath = new System.Windows.Forms.Button();
            this.startProgram = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxImageFolder = new System.Windows.Forms.TextBox();
            this.chooseImageFolder = new System.Windows.Forms.Button();
            this.openErrorLog = new System.Windows.Forms.Button();
            this.openImageFolder = new System.Windows.Forms.Button();
            this.openSuccessLog = new System.Windows.Forms.Button();
            this.clearLogs = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBoxClearLogs = new System.Windows.Forms.CheckBox();
            this.checkBoxAddJpg = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxWebpath
            // 
            this.textBoxWebpath.Location = new System.Drawing.Point(149, 12);
            this.textBoxWebpath.Name = "textBoxWebpath";
            this.textBoxWebpath.Size = new System.Drawing.Size(543, 20);
            this.textBoxWebpath.TabIndex = 0;
            // 
            // textBoxFilepath
            // 
            this.textBoxFilepath.Location = new System.Drawing.Point(149, 38);
            this.textBoxFilepath.Name = "textBoxFilepath";
            this.textBoxFilepath.Size = new System.Drawing.Size(475, 20);
            this.textBoxFilepath.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "Web path";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 20);
            this.button2.TabIndex = 3;
            this.button2.Text = "File path";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chooseFilePath
            // 
            this.chooseFilePath.Location = new System.Drawing.Point(630, 38);
            this.chooseFilePath.Name = "chooseFilePath";
            this.chooseFilePath.Size = new System.Drawing.Size(62, 20);
            this.chooseFilePath.TabIndex = 4;
            this.chooseFilePath.Text = "...";
            this.chooseFilePath.UseVisualStyleBackColor = true;
            this.chooseFilePath.Click += new System.EventHandler(this.chooseFilePath_Click);
            // 
            // startProgram
            // 
            this.startProgram.BackColor = System.Drawing.Color.ForestGreen;
            this.startProgram.Location = new System.Drawing.Point(12, 158);
            this.startProgram.Name = "startProgram";
            this.startProgram.Size = new System.Drawing.Size(680, 80);
            this.startProgram.TabIndex = 5;
            this.startProgram.Text = "START!";
            this.startProgram.UseVisualStyleBackColor = false;
            this.startProgram.Click += new System.EventHandler(this.startProgram_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 20);
            this.button3.TabIndex = 6;
            this.button3.Text = "Download folder";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxImageFolder
            // 
            this.textBoxImageFolder.Location = new System.Drawing.Point(149, 65);
            this.textBoxImageFolder.Name = "textBoxImageFolder";
            this.textBoxImageFolder.Size = new System.Drawing.Size(475, 20);
            this.textBoxImageFolder.TabIndex = 7;
            // 
            // chooseImageFolder
            // 
            this.chooseImageFolder.Location = new System.Drawing.Point(630, 64);
            this.chooseImageFolder.Name = "chooseImageFolder";
            this.chooseImageFolder.Size = new System.Drawing.Size(62, 20);
            this.chooseImageFolder.TabIndex = 8;
            this.chooseImageFolder.Text = "...";
            this.chooseImageFolder.UseVisualStyleBackColor = true;
            this.chooseImageFolder.Click += new System.EventHandler(this.chooseImageFolder_Click);
            // 
            // openErrorLog
            // 
            this.openErrorLog.Location = new System.Drawing.Point(541, 307);
            this.openErrorLog.Name = "openErrorLog";
            this.openErrorLog.Size = new System.Drawing.Size(150, 23);
            this.openErrorLog.TabIndex = 9;
            this.openErrorLog.Text = "Open error log";
            this.openErrorLog.UseVisualStyleBackColor = true;
            this.openErrorLog.Click += new System.EventHandler(this.openErrorLog_Click);
            // 
            // openImageFolder
            // 
            this.openImageFolder.Location = new System.Drawing.Point(12, 307);
            this.openImageFolder.Name = "openImageFolder";
            this.openImageFolder.Size = new System.Drawing.Size(325, 49);
            this.openImageFolder.TabIndex = 10;
            this.openImageFolder.Text = "Open download folder in explorer";
            this.openImageFolder.UseVisualStyleBackColor = true;
            this.openImageFolder.Click += new System.EventHandler(this.openImageFolder_Click);
            // 
            // openSuccessLog
            // 
            this.openSuccessLog.Location = new System.Drawing.Point(385, 307);
            this.openSuccessLog.Name = "openSuccessLog";
            this.openSuccessLog.Size = new System.Drawing.Size(150, 23);
            this.openSuccessLog.TabIndex = 11;
            this.openSuccessLog.Text = "Open success log";
            this.openSuccessLog.UseVisualStyleBackColor = true;
            this.openSuccessLog.Click += new System.EventHandler(this.openSuccessLog_Click);
            // 
            // clearLogs
            // 
            this.clearLogs.Location = new System.Drawing.Point(385, 332);
            this.clearLogs.Name = "clearLogs";
            this.clearLogs.Size = new System.Drawing.Size(306, 23);
            this.clearLogs.TabIndex = 12;
            this.clearLogs.Text = "Clear Logs";
            this.clearLogs.UseVisualStyleBackColor = true;
            this.clearLogs.Click += new System.EventHandler(this.clearLogs_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 245);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(679, 23);
            this.progressBar1.TabIndex = 13;
            // 
            // checkBoxClearLogs
            // 
            this.checkBoxClearLogs.AutoSize = true;
            this.checkBoxClearLogs.Location = new System.Drawing.Point(13, 92);
            this.checkBoxClearLogs.Name = "checkBoxClearLogs";
            this.checkBoxClearLogs.Size = new System.Drawing.Size(160, 17);
            this.checkBoxClearLogs.TabIndex = 14;
            this.checkBoxClearLogs.Text = "Clear logs before every start.";
            this.checkBoxClearLogs.UseVisualStyleBackColor = true;
            this.checkBoxClearLogs.CheckedChanged += new System.EventHandler(this.checkBoxClearLogs_CheckedChanged);
            // 
            // checkBoxAddJpg
            // 
            this.checkBoxAddJpg.AutoSize = true;
            this.checkBoxAddJpg.Location = new System.Drawing.Point(13, 116);
            this.checkBoxAddJpg.Name = "checkBoxAddJpg";
            this.checkBoxAddJpg.Size = new System.Drawing.Size(192, 17);
            this.checkBoxAddJpg.TabIndex = 15;
            this.checkBoxAddJpg.Text = "Add .jpg if there is no file extension.";
            this.checkBoxAddJpg.UseVisualStyleBackColor = true;
            this.checkBoxAddJpg.CheckedChanged += new System.EventHandler(this.checkBoxAddJpg_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 367);
            this.Controls.Add(this.checkBoxAddJpg);
            this.Controls.Add(this.checkBoxClearLogs);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.clearLogs);
            this.Controls.Add(this.openSuccessLog);
            this.Controls.Add(this.openImageFolder);
            this.Controls.Add(this.openErrorLog);
            this.Controls.Add(this.chooseImageFolder);
            this.Controls.Add(this.textBoxImageFolder);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.startProgram);
            this.Controls.Add(this.chooseFilePath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxFilepath);
            this.Controls.Add(this.textBoxWebpath);
            this.Name = "Form1";
            this.Text = "ImageLoader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxWebpath;
		private System.Windows.Forms.TextBox textBoxFilepath;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button chooseFilePath;
		private System.Windows.Forms.Button startProgram;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox textBoxImageFolder;
		private System.Windows.Forms.Button chooseImageFolder;
		private System.Windows.Forms.Button openErrorLog;
		private System.Windows.Forms.Button openImageFolder;
		private System.Windows.Forms.Button openSuccessLog;
		private System.Windows.Forms.Button clearLogs;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.CheckBox checkBoxClearLogs;
		private System.Windows.Forms.CheckBox checkBoxAddJpg;
	}
}