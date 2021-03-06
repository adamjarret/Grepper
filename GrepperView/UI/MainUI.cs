﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Grepper.ContextMenu;
using GrepperLib.Controller;
using GrepperLib.Model;

namespace GrepperView
{
    public partial class MainUI : Form
    {
        #region Private Members________

        private readonly FileController _fileController;
        private static BackgroundWorker workerThread;

        #endregion
        #region Constructor____________

        /// <summary>
        /// Sets folder path, tool tips, and loads settings.
        /// </summary>
        /// <param name="path"></param>
        public MainUI(string path)
        {
            InitializeComponent();
            timeCounter.Enabled = false;
            progressBar.Visible = false;
            txtBaseSearchPath.Text = path;
            _fileController = new FileController();

            // set tool tips
            SetToolTips();

            // load user settings
            LoadSettings();
        }

        #endregion
        #region Protected Methods______

        /// <summary>
        /// Custom gradient color background.
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Rectangle r = this.ClientRectangle;
            if (r.Width > 0 && r.Height > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(r, Color.White, Color.AliceBlue, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, r);
                }
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        /// <summary>
        /// Invalidates the form so that it will resize the controls correctly and repaint.
        /// </summary>
        protected void MainUI_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary>
        /// Save User Settings when window closes
        /// </summary>
        protected void MainUI_FormClosing(Object sender, FormClosingEventArgs e)
        {
            // save the current user option settings
            SaveCurrentSettings();
        }

        /// <summary>
        /// Doubleclick will attempt to open the file in its default associated application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvwFileMatches_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string itemSelected = lvwFileMatches.SelectedItems[0].Text;
            var fileMatch = from match in _fileController.FileDataList
                            where match.FilePath == itemSelected
                            select match;

            if (fileMatch == null || fileMatch.Count() < 1) return;
            using (Process proc = new Process() { StartInfo = new ProcessStartInfo(fileMatch.First().FilePath) })
            {
                proc.Start();
            }
        }

        /// <summary>
        /// Delegate method for timer object. Increments the progressBar by a pre-defined step for each tick of the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void timeCounter_Tick(object sender, EventArgs e)
        {
            // reset to 0 if max reached, only need to show user that program is doing something and not frozen
            if (progressBar.Step >= progressBar.Maximum) progressBar.Step = 0;
            progressBar.PerformStep();
        }

        /// <summary>
        /// Right-click will copy contents to clipboard.
        /// </summary>
        protected void lvwLineData_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string itemSelected = string.Empty;
                if ((lvwLineData.SelectedItems.Count > 0) && (lvwLineData.SelectedItems[0].SubItems.Count > 1))
                    itemSelected = lvwLineData.SelectedItems[0].SubItems[1].Text;

                if (itemSelected != null) Clipboard.SetText(itemSelected);
                lblMessages.Text = string.Format("Copied line {0} to clipboard", lvwLineData.SelectedItems[0].Text);
            }
        }

        /// <summary>
        /// Clicking the search path pulls up a custom directory dialog box.
        /// </summary>
        protected void txtBaseSearchPath_Click(object sender, EventArgs e)
        {
            DirectoryBrowser db = new DirectoryBrowser();
            db.StartDirectory = txtBaseSearchPath.Text.Trim();
            if (db.ShowDialog() == DialogResult.OK)
            {
                txtBaseSearchPath.Text = db.NodeSelected;
            }
        }

        /// <summary>
        /// Display file contents in lower view section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvwFileMatches_MouseClick(object sender, MouseEventArgs e)
        {
            lvwLineData.Items.Clear();
            string itemSelected = lvwFileMatches.SelectedItems[0].Text;
            var fileMatch = from match in _fileController.FileDataList
                            where match.FilePath == itemSelected
                            select match;

            if (fileMatch == null || fileMatch.Count() < 1) return;
            foreach (KeyValuePair<long, LineData> item in (Dictionary<long, LineData>)fileMatch.First().LineDataList)
            {
                ListViewItem lvi = new ListViewItem(new string[] { item.Key.ToString(), item.Value.Text });
                lvwLineData.Items.Add(lvi);
            }

            SetAlternateRowColor(lvwLineData);
        }

        protected void lnkExtensions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExtensionsUI extensions = new ExtensionsUI();
            extensions.ShowDialog();
            LoadSettings();
        }

        #endregion
        #region Private Methods________

        /// <summary>
        /// Sets the popup tips for given controls when mouse hovers over them.
        /// This is meant to aid in the understanding of each feature.
        /// </summary>
        private void SetToolTips()
        {
            grepperTip.SetToolTip(cbxRecursive, "Include all subfolders in search.");
            grepperTip.SetToolTip(cbxMatchPhrase, "Match exact phrase on natural boundaries.");
            grepperTip.SetToolTip(cbxMatchCase, "Match exact case of search pattern (only for literal searches).");
            grepperTip.SetToolTip(ddlFileExtensions, "Enter files to search separated by spaces. Use asterisk for all matches: *.txt *.sql somefile.cs");
            grepperTip.SetToolTip(rbLiteral, "Search for the input pattern as typed in.");
            grepperTip.SetToolTip(rbRegular, "Treat input pattern as a regular expression.");
            grepperTip.SetToolTip(lnkExtensions, "Manage extensions list.");
        }

        /// <summary>
        /// Starts a new thread to search the given folder with the given parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // clear out any messages
            lblMessages.Text = string.Empty;
            lvwFileMatches.Items.Clear();
            lvwLineData.Items.Clear();

            // save the current user option settings
            SaveCurrentSettings();

            _fileController.SearchCriteria = ddlSearchCriteria.Text.Trim();
            _fileController.MatchCase = cbxMatchCase.Checked;
            _fileController.MatchPhrase = cbxMatchPhrase.Checked;
            _fileController.FileExtensions = ddlFileExtensions.Text;
            _fileController.RecursiveSearch = cbxRecursive.Checked;
            _fileController.BaseSearchPath = txtBaseSearchPath.Text;
            _fileController.LiteralSearch = rbLiteral.Checked;

            // create background worker thread to perform search so that UI does not lock up
            workerThread = new BackgroundWorker();
            workerThread.DoWork += workerThread_DoWork;
            workerThread.RunWorkerCompleted += workerThread_RunWorkerCompleted;

            // set & enable progressBar & timer
            progressBar.Visible = true;
            progressBar.Value = 0;
            timeCounter.Enabled = true;

            // start the thread
            workerThread.RunWorkerAsync(_fileController);
        }

        /// <summary>
        /// Delegate method called when worker thread is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // obtain results from worker thread and update UI as necessary
            FileController fc = (FileController)e.Result;

            // display any errors
            if (fc.MessageList != null)
            {
                foreach (string error in fc.MessageList)
                {
                    ListViewItem item = new ListViewItem(new string[] { error, "Error" });
                    item.ForeColor = Color.Red;
                    lvwFileMatches.Items.Add(item);
                }
            }
            // set total results
            lblMessages.Visible = true;
            string matches = fc.TotalMatches == 1 ? "" : "es";
            

            // display message if no results found
            if (fc.FileDataList == null || fc.FileDataList.Count < 1)
            {
                lblMessages.Text = "No results found.";
            }
            else
            {
                string files = fc.FileDataList.Count == 1 ? "" : "s";
                lblMessages.Text = string.Format("{0} match{1} in {2} file{3}", fc.TotalMatches, matches, fc.FileDataList.Count, files);
            }

            if (fc.FileDataList != null)
            {
                // display results if any
                foreach (FileData fd in fc.FileDataList)
                {
                    lvwFileMatches.Items.Add(new ListViewItem(new string[] { fd.FilePath, fd.LineDataList.Count.ToString() }));
                }
            }

            // set row colors and disable progressBar & timer
            SetAlternateRowColor(lvwFileMatches);
            progressBar.Visible = false;
            timeCounter.Enabled = false;
        }

        /// <summary>
        /// Delegate method to perform FileController actions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            FileController fc = (FileController)e.Argument;
            fc.GenerateFileData();

            e.Result = fc;
        }

        /// <summary>
        /// Saves current state of settings, including file extensions to search, but not the base path
        /// since that depends on where the user right-clicks in Explorer.
        /// </summary>
        private void SaveCurrentSettings()
        {
            #region Manage Extension List

            if (ddlFileExtensions.Text.Length > 0 && !ddlFileExtensions.Items.Contains(ddlFileExtensions.Text))
                ddlFileExtensions.Items.Add(ddlFileExtensions.Text);

            UserSettings.Extensions.Clear();
            foreach (string item in ddlFileExtensions.Items)
            {
                UserSettings.Extensions.Add(item);
            }

            #endregion
            #region Manage Search List

            if (ddlSearchCriteria.Text.Length > 0)
            {
                // if item already exists, it must be moved to first place
                if (ddlSearchCriteria.Items.Contains(ddlSearchCriteria.Text))
                {
                    List<string> itemList = new List<string>();
                    foreach (string text in ddlSearchCriteria.Items)
                    {
                        itemList.Add(text);
                    }
                    ddlSearchCriteria.Items.Clear();
                    foreach (string text in itemList)
                    {
                        if (text != ddlSearchCriteria.Text) ddlSearchCriteria.Items.Add(text);
                    }
                }

                // new item is always first, and trim anything past 5 "remembered" items
                ddlSearchCriteria.Items.Insert(0, ddlSearchCriteria.Text);
                int total = ddlSearchCriteria.Items.Count;
                if (total > 5)
                {
                    for (int i = 5; i < total; i++)
                    {
                        ddlSearchCriteria.Items.RemoveAt(i);
                    }
                }
            }
            
            UserSettings.SearchTerms.Clear();
            foreach (string item in ddlSearchCriteria.Items)
            {
                UserSettings.SearchTerms.Add(item);
            }

            #endregion
            #region Save Settings

            SearchOptions so = new SearchOptions();
            so.Literal = rbLiteral.Checked;
            so.MatchCase = cbxMatchCase.Checked;
            so.MatchPhrase = cbxMatchPhrase.Checked;
            so.Recursive = cbxRecursive.Checked;
            so.Search = ddlSearchCriteria.Text;
            so.Path = txtBaseSearchPath.Text;
            UserSettings.SearchOptions = so;

            UserSettings.Save();

            #endregion
        }

        /// <summary>
        /// Loads user settings from the registry.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                ddlFileExtensions.Items.Clear();
                StringCollection extns = UserSettings.Extensions;
                if(extns.Count > 0)
                {
                    foreach (string item in extns)
                    {
                        ddlFileExtensions.Items.Add(item);
                    }
                    ddlFileExtensions.Text = extns[0];
                }

                ddlSearchCriteria.Items.Clear();
                foreach (string item in UserSettings.SearchTerms)
                {
                    ddlSearchCriteria.Items.Add(item);
                }

                SearchOptions so = UserSettings.SearchOptions;
                ddlSearchCriteria.Text = so.Search;
                cbxMatchCase.Checked = so.MatchCase;
                cbxMatchPhrase.Checked = so.MatchPhrase;
                cbxRecursive.Checked = so.Recursive;
                rbLiteral.Checked = so.Literal;
                rbRegular.Checked = !rbLiteral.Checked;

                // set app version on titlebar
                this.Text = string.Format("GREPPER v{0}.{1}.{2}", Assembly.GetAssembly(_fileController.GetType()).GetName().Version.Major,
                                                                  Assembly.GetAssembly(_fileController.GetType()).GetName().Version.Minor,
                                                                  Assembly.GetAssembly(_fileController.GetType()).GetName().Version.Build);
            }
            catch (InvalidCastException ice)
            {
                _fileController.MessageList.Add(ice.Message);
            }
            catch (UnauthorizedAccessException uae)
            {
                _fileController.MessageList.Add(uae.Message);
            }
        }

        /// <summary>
        /// Alternate white & gray to make rows easier to discern.
        /// </summary>
        /// <param name="lv"></param>
        private static void SetAlternateRowColor(ListView lv)
        {
            foreach (ListViewItem item in lv.Items)
            {
                item.BackColor = (item.Index % 2 == 0) ? Color.White : Color.Gainsboro;
            }
        }
        #endregion
    }
}