﻿namespace GrepperView
{
	partial class MainUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBaseSearchPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lvwFileMatches = new System.Windows.Forms.ListView();
            this.colFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMatchCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwLineData = new System.Windows.Forms.ListView();
            this.lineNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lineData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeCounter = new System.Windows.Forms.Timer(this.components);
            this.grepperTip = new System.Windows.Forms.ToolTip(this.components);
            this.ddlFileExtensions = new System.Windows.Forms.ComboBox();
            this.lnkExtensions = new System.Windows.Forms.LinkLabel();
            this.ddlSearchCriteria = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbxMatchCase = new System.Windows.Forms.CheckBox();
            this.cbxRecursive = new System.Windows.Forms.CheckBox();
            this.cbxMatchPhrase = new System.Windows.Forms.CheckBox();
            this.rbLiteral = new System.Windows.Forms.RadioButton();
            this.rbRegular = new System.Windows.Forms.RadioButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(20, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search Criteria:";
            // 
            // txtBaseSearchPath
            // 
            this.txtBaseSearchPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBaseSearchPath.Location = new System.Drawing.Point(220, 80);
            this.txtBaseSearchPath.Margin = new System.Windows.Forms.Padding(6);
            this.txtBaseSearchPath.Name = "txtBaseSearchPath";
            this.txtBaseSearchPath.Size = new System.Drawing.Size(705, 31);
            this.txtBaseSearchPath.TabIndex = 2;
            this.txtBaseSearchPath.Click += new System.EventHandler(this.txtBaseSearchPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(20, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "Base Search Path:";
            // 
            // lvwFileMatches
            // 
            this.lvwFileMatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFileMatches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilePath,
            this.colMatchCount});
            this.lvwFileMatches.Location = new System.Drawing.Point(0, 0);
            this.lvwFileMatches.Margin = new System.Windows.Forms.Padding(6);
            this.lvwFileMatches.Name = "lvwFileMatches";
            this.lvwFileMatches.Size = new System.Drawing.Size(1379, 290);
            this.lvwFileMatches.TabIndex = 10;
            this.lvwFileMatches.UseCompatibleStateImageBehavior = false;
            this.lvwFileMatches.View = System.Windows.Forms.View.Details;
            this.lvwFileMatches.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwFileMatches_MouseClick);
            this.lvwFileMatches.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwFileMatches_MouseDoubleClick);
            // 
            // colFilePath
            // 
            this.colFilePath.DisplayIndex = 1;
            this.colFilePath.Text = "File Path";
            this.colFilePath.Width = 1058;
            // 
            // colMatchCount
            // 
            this.colMatchCount.DisplayIndex = 0;
            this.colMatchCount.Text = "Matches";
            this.colMatchCount.Width = 154;
            // 
            // lvwLineData
            // 
            this.lvwLineData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwLineData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lineNumber,
            this.lineData});
            this.lvwLineData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lvwLineData.FullRowSelect = true;
            this.lvwLineData.Location = new System.Drawing.Point(0, 6);
            this.lvwLineData.Margin = new System.Windows.Forms.Padding(6);
            this.lvwLineData.Name = "lvwLineData";
            this.lvwLineData.Size = new System.Drawing.Size(1379, 281);
            this.lvwLineData.TabIndex = 11;
            this.lvwLineData.UseCompatibleStateImageBehavior = false;
            this.lvwLineData.View = System.Windows.Forms.View.Details;
            this.lvwLineData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwLineData_MouseClick);
            // 
            // lineNumber
            // 
            this.lineNumber.Text = "Line #";
            this.lineNumber.Width = 102;
            // 
            // lineData
            // 
            this.lineData.Text = "Data";
            this.lineData.Width = 1115;
            // 
            // timeCounter
            // 
            this.timeCounter.Interval = 500;
            this.timeCounter.Tick += new System.EventHandler(this.timeCounter_Tick);
            // 
            // ddlFileExtensions
            // 
            this.ddlFileExtensions.AllowDrop = true;
            this.ddlFileExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFileExtensions.FormattingEnabled = true;
            this.ddlFileExtensions.Location = new System.Drawing.Point(220, 133);
            this.ddlFileExtensions.Margin = new System.Windows.Forms.Padding(6);
            this.ddlFileExtensions.Name = "ddlFileExtensions";
            this.ddlFileExtensions.Size = new System.Drawing.Size(705, 33);
            this.ddlFileExtensions.TabIndex = 3;
            // 
            // lnkExtensions
            // 
            this.lnkExtensions.AutoSize = true;
            this.lnkExtensions.BackColor = System.Drawing.Color.Transparent;
            this.lnkExtensions.Location = new System.Drawing.Point(20, 136);
            this.lnkExtensions.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lnkExtensions.Name = "lnkExtensions";
            this.lnkExtensions.Size = new System.Drawing.Size(165, 25);
            this.lnkExtensions.TabIndex = 21;
            this.lnkExtensions.TabStop = true;
            this.lnkExtensions.Text = "File Extensions:";
            this.lnkExtensions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExtensions_LinkClicked);
            // 
            // ddlSearchCriteria
            // 
            this.ddlSearchCriteria.AllowDrop = true;
            this.ddlSearchCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSearchCriteria.FormattingEnabled = true;
            this.ddlSearchCriteria.Location = new System.Drawing.Point(220, 25);
            this.ddlSearchCriteria.Margin = new System.Windows.Forms.Padding(6);
            this.ddlSearchCriteria.Name = "ddlSearchCriteria";
            this.ddlSearchCriteria.Size = new System.Drawing.Size(705, 33);
            this.ddlSearchCriteria.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.cbxMatchCase);
            this.panel1.Controls.Add(this.cbxRecursive);
            this.panel1.Controls.Add(this.cbxMatchPhrase);
            this.panel1.Controls.Add(this.rbLiteral);
            this.panel1.Controls.Add(this.rbRegular);
            this.panel1.Location = new System.Drawing.Point(937, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 184);
            this.panel1.TabIndex = 22;
            // 
            // cbxMatchCase
            // 
            this.cbxMatchCase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxMatchCase.AutoSize = true;
            this.cbxMatchCase.Location = new System.Drawing.Point(30, 17);
            this.cbxMatchCase.Margin = new System.Windows.Forms.Padding(6);
            this.cbxMatchCase.Name = "cbxMatchCase";
            this.cbxMatchCase.Size = new System.Drawing.Size(159, 29);
            this.cbxMatchCase.TabIndex = 4;
            this.cbxMatchCase.Text = "Match Case";
            this.cbxMatchCase.UseVisualStyleBackColor = true;
            // 
            // cbxRecursive
            // 
            this.cbxRecursive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRecursive.AutoSize = true;
            this.cbxRecursive.Location = new System.Drawing.Point(30, 71);
            this.cbxRecursive.Margin = new System.Windows.Forms.Padding(6);
            this.cbxRecursive.Name = "cbxRecursive";
            this.cbxRecursive.Size = new System.Drawing.Size(214, 29);
            this.cbxRecursive.TabIndex = 6;
            this.cbxRecursive.Text = "Recursive Search";
            this.cbxRecursive.UseVisualStyleBackColor = true;
            // 
            // cbxMatchPhrase
            // 
            this.cbxMatchPhrase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxMatchPhrase.AutoSize = true;
            this.cbxMatchPhrase.Location = new System.Drawing.Point(226, 17);
            this.cbxMatchPhrase.Margin = new System.Windows.Forms.Padding(6);
            this.cbxMatchPhrase.Name = "cbxMatchPhrase";
            this.cbxMatchPhrase.Size = new System.Drawing.Size(177, 29);
            this.cbxMatchPhrase.TabIndex = 5;
            this.cbxMatchPhrase.Text = "Match Phrase";
            this.cbxMatchPhrase.UseVisualStyleBackColor = true;
            // 
            // rbLiteral
            // 
            this.rbLiteral.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbLiteral.AutoSize = true;
            this.rbLiteral.Checked = true;
            this.rbLiteral.Location = new System.Drawing.Point(30, 124);
            this.rbLiteral.Margin = new System.Windows.Forms.Padding(6);
            this.rbLiteral.Name = "rbLiteral";
            this.rbLiteral.Size = new System.Drawing.Size(176, 29);
            this.rbLiteral.TabIndex = 7;
            this.rbLiteral.TabStop = true;
            this.rbLiteral.Text = "Literal Search";
            this.rbLiteral.UseVisualStyleBackColor = true;
            // 
            // rbRegular
            // 
            this.rbRegular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbRegular.AutoSize = true;
            this.rbRegular.Location = new System.Drawing.Point(226, 124);
            this.rbRegular.Margin = new System.Windows.Forms.Padding(6);
            this.rbRegular.Name = "rbRegular";
            this.rbRegular.Size = new System.Drawing.Size(231, 29);
            this.rbRegular.TabIndex = 8;
            this.rbRegular.Text = "Regular Expression";
            this.rbRegular.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(25, 805);
            this.progressBar.Margin = new System.Windows.Forms.Padding(6);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(184, 44);
            this.progressBar.TabIndex = 18;
            this.progressBar.Visible = false;
            // 
            // lblMessages
            // 
            this.lblMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessages.BackColor = System.Drawing.Color.Transparent;
            this.lblMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessages.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lblMessages.Location = new System.Drawing.Point(240, 805);
            this.lblMessages.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(990, 44);
            this.lblMessages.TabIndex = 13;
            this.lblMessages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(1260, 805);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(150, 44);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(25, 203);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwFileMatches);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvwLineData);
            this.splitContainer1.Size = new System.Drawing.Size(1385, 593);
            this.splitContainer1.SplitterDistance = 296;
            this.splitContainer1.TabIndex = 23;
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(1924, 1117);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ddlSearchCriteria);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lnkExtensions);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.ddlFileExtensions);
            this.Controls.Add(this.txtBaseSearchPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1200, 800);
            this.Name = "MainUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grepper";
            this.TransparencyKey = System.Drawing.SystemColors.InactiveBorder;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainUI_FormClosing);
            this.Resize += new System.EventHandler(this.MainUI_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBaseSearchPath;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView lvwFileMatches;
		private System.Windows.Forms.ColumnHeader colFilePath;
		private System.Windows.Forms.ColumnHeader colMatchCount;
		private System.Windows.Forms.ListView lvwLineData;
		private System.Windows.Forms.ColumnHeader lineNumber;
        private System.Windows.Forms.ColumnHeader lineData;
		private System.Windows.Forms.Timer timeCounter;
        private System.Windows.Forms.ToolTip grepperTip;
        private System.Windows.Forms.ComboBox ddlFileExtensions;
        private System.Windows.Forms.LinkLabel lnkExtensions;
        private System.Windows.Forms.ComboBox ddlSearchCriteria;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbxMatchCase;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox cbxRecursive;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.CheckBox cbxMatchPhrase;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton rbLiteral;
        private System.Windows.Forms.RadioButton rbRegular;
        private System.Windows.Forms.SplitContainer splitContainer1;
	}
}

