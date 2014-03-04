using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GrepperLib.Controller;
using GrepperLib.Model;
using GrepperWPF.Converters;
using GrepperWPF.Helpers;
using GrepperWPF.Models;
using Ookii.Dialogs.Wpf;
using Timer = System.Timers.Timer;
using WindowState = System.Windows.WindowState;

namespace GrepperWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Timer _overlayTimer;
        private readonly FileController _fileController;
        private static BackgroundWorker _workerThread;

        public MainWindow()
        {
            _fileController = new FileController();

            InitializeComponent();

            // Overlay Timer
            _overlayTimer = new Timer();
            _overlayTimer.Elapsed += overlayTimer_Tick;
            _overlayTimer.Interval = 700;
 
            // Load Window Size/Postion/State
            // (do this here to avoid flicker -- after InitializeComponent but before Window_Loaded)
            if (UserSettings.RememberWindowSettings)
            {
                WindowState = UserSettings.MainWindowState;
                LoadWindowSettings();
            }
        }

        #region Event Handlers

        /// <summary>
        /// Load User Settings when window loads
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();

            ddlSearchCriteria.Focus();
        }
        
        /// <summary>
        /// Save User Settings when window is closed
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {            
            SaveCurrentSettings();
        }

        /// <summary>
        /// Save window size/position and Resize the columns of the listviews (the binding doesn't seem to change the width on resize)
        /// </summary>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeColumns();
            SaveWindowSettings();
        }

        /// <summary>
        /// Triggered when the "Copied to clipboard" overlay is shown/hidden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Overlay_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Overlay.IsVisible)
            {
                _overlayTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Triggered when overlayTimer fires after set interval
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void overlayTimer_Tick(object source, ElapsedEventArgs e)
        {
            // Make sure code that updates the UI runs on the main thread
            new Thread(HideOverlay).Start();
        }

        /// <summary>
        /// Doubleclick will attempt to open the file in its default associated application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvw_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (UserSettings.ShiftDoubleClickToReveal)
                {
                    RevealSelectedFile();
                }
            }
            else
            {
                if (UserSettings.DoubleClickToOpen)
                {
                    OpenSelectedFile();
                }
            }
        }

        /// <summary>
        /// Pressing Enter will attempt to open the file in its default associated application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return; // We only care about the return key

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (UserSettings.ShiftEnterToReveal)
                {
                    RevealSelectedFile();
                }
            }
            else
            {
                if (UserSettings.EnterToOpen)
                {
                    OpenSelectedFile();
                }
            }
        }

        /// <summary>
        /// Trigger search on Enter keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void field_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                // Cancel and restart search if already running
                if (_workerThread != null && _workerThread.IsBusy)
                {
                    // Add a listener that triggers when search is completed
                    //  (i.e. the worker has finished being Cancelled)
                    _workerThread.RunWorkerCompleted += workerThread_RestartSearch;
                    CancelSearch();
                }
                else
                {
                    ExecuteSearch();
                }
            }
        }

        /// <summary>
        /// Trigger search when Search button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_workerThread != null && _workerThread.IsBusy)
            {
                CancelSearch();
            }
            else
            {
                ExecuteSearch();
            }
        }

        /// <summary>
        /// Show Options window when Options button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOptions_Click(object sender, EventArgs e)
        {
            Options.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Show Options window when Options button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseOptions_Click(object sender, EventArgs e)
        {
            Options.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Clear remembered combo box items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearComboBox_Click(object sender, EventArgs e)
        {
            ComboBox cb = null;
            string name = ((Button)sender).Name;
            switch (name)
            {
                case "btnClearSearchCriteria":
                    cb = ddlSearchCriteria;
                    UserSettings.SearchTerms.Clear();
                    OverlayLabel.Content = "Cleared search criteria history";
                    break;
                case "btnClearSearchPaths":
                    cb = ddlBaseSearchPath;
                    UserSettings.SearchPaths.Clear();
                    OverlayLabel.Content = "Cleared search path history";
                    break;
                case "btnClearExtensions":
                    cb = ddlFileExtensions;
                    UserSettings.Extensions.Clear();
                    OverlayLabel.Content = "Cleared file filter history";
                    break;
            }
            if (cb != null)
            {
                string text = cb.Text;
                cb.Items.Clear();
                cb.Text = text;
                UserSettings.Save();
                Overlay.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Show Folder Chooser when Browse button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog();
            var so = GetSearchOptions();
            dlg.SelectedPath = so.Path;
            dlg.ShowNewFolderButton = true;
            if (dlg.ShowDialog() == true)
            {
                ddlBaseSearchPath.Text = dlg.SelectedPath;
            }
        }

        /// <summary>
        /// Display match details when selection changes in file list
        /// </summary>
        private void lvwFileMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowMatches();
        }

        /// <summary>
        /// Right-click will copy contents to clipboard.
        /// </summary>
        private void lvwFileMatches_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!UserSettings.RightClickToCopy || e.ChangedButton != MouseButton.Right)
            {
                return;
            }

            var selectedRow = SelectedFileRow();
            if (selectedRow == null)
            {
                return;
            }

            Clipboard.SetText(selectedRow.Path);

            OverlayLabel.Content = "Path copied to clipboard";
            Overlay.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Right-click will copy contents to clipboard.
        /// </summary>
        private void lvwLineData_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (UserSettings.RightClickToCopy && e.ChangedButton == MouseButton.Right)
            {
                var sb = new StringBuilder();
                var selectedRows = SelectedLineRows();
                var selectedRowsCount = selectedRows.Count;
                if (selectedRowsCount > 0)
                {
                    foreach (LineMatchRow row in selectedRows)
                    {
                        // Experimental feature (might be useful for people with clipboard managers)
                        if (UserSettings.CopyLinesInSequence)
                        {
                            Clipboard.SetText(row.Text);
                            Thread.Sleep(120); // 120 seems to be enough for Ditto
                        }
                        else
                        {
                            sb.AppendLine(row.Text);
                        }
                    }
                    if (!UserSettings.CopyLinesInSequence)
                    {
                        Clipboard.SetText(sb.ToString());
                    }

                    OverlayLabel.Content = String.Format("({0}) Line{1} copied to clipboard", selectedRowsCount, selectedRowsCount > 1 ? "s" : "");
                    Overlay.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Attempts to show the selected file in Explorer
        /// </summary>
        private FileData SelectedFile()
        {
            FileMatchRow selectedRow = SelectedFileRow();
            string selectedPath = selectedRow.Path;
            var fileMatch = (from match in _fileController.FileDataList
                             where match.FilePath == selectedPath
                             select match).ToList();
            if (fileMatch.Count < 1)
            {
                return null;
            }
            return fileMatch.First();
        }

        private FileMatchRow SelectedFileRow()
        {
            if (lvwFileMatches.SelectedItems.Count <= 0)
            {
                return null;
            }

            var selectedItem = (ListViewItem)lvwFileMatches.SelectedItems[0];
            return (FileMatchRow)selectedItem.Content;
        }

        private List<LineMatchRow> SelectedLineRows()
        {
            return (from ListViewItem selectedItem in lvwLineData.SelectedItems
                    select (LineMatchRow) selectedItem.Content).ToList();
        }

        private void HideOverlay()
        {
            _overlayTimer.Enabled = false;

            // Make sure this is run on the main thread
            if (Overlay.Dispatcher.CheckAccess())
            {
                Overlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                Overlay.Dispatcher.BeginInvoke(
                    (Action)HideOverlay);
            }
        }

        private void SetProgressBarValue(int value)
        {
            progressBar.Value = value;
        }

        private void ResizeColumns()
        {
            var lvwFileMatchesGridView = (GridView)lvwFileMatches.View;
            lvwFileMatchesGridView.Columns.ElementAt(1).Width = StretchableColumnWidthConverter.GetWidth(lvwFileMatches);

            var lvwLineDataGridView = (GridView)lvwLineData.View;
            lvwLineDataGridView.Columns.ElementAt(1).Width = StretchableColumnWidthConverter.GetWidth(lvwLineData);
        }

        private void LoadWindowSettings()
        {
            WindowSettings ws = UserSettings.MainWindowSettings;
            if (ws != null)
            {
                double screenWidth = SystemParameters.VirtualScreenWidth;
                double screenHeight = SystemParameters.VirtualScreenHeight;
                ws.SizeToFit(screenWidth, screenHeight);
                ws.MoveIntoView(screenWidth, screenHeight);
                Top = ws.Top;
                Left = ws.Left;
                Width = ws.Width;
                Height = ws.Height;
            }
        }

        private void SaveWindowSettings()
        {
            if (IsLoaded && WindowState == WindowState.Normal)
            {
                var ws = new WindowSettings
                {
                    Top = Top,
                    Left = Left,
                    Width = Width,
                    Height = Height
                };
                UserSettings.MainWindowSettings = ws;
            }
            // Do not call UserSettings.Save for performance reasons. Save is only called when the Window is closed or a search is performed.
        }

        /// <summary>
        /// Builds a SearchOptions object using the form data
        /// </summary>
        private SearchOptions GetSearchOptions()
        {
            return new SearchOptions
            {
                Literal = cbxRegEx != null && !cbxRegEx.IsChecked.GetValueOrDefault(),
                MatchCase = cbxMatchCase != null && cbxMatchCase.IsChecked.GetValueOrDefault(),
                MatchPhrase = cbxMatchPhrase != null && cbxMatchPhrase.IsChecked.GetValueOrDefault(),
                Recursive = cbxRecursive != null && cbxRecursive.IsChecked.GetValueOrDefault(),
                Search = ddlSearchCriteria.Text,
                Path = ddlBaseSearchPath.Text,
                Extensions = ddlFileExtensions.Text
            };
        }

        /// <summary>
        /// Loads user settings from the registry.
        /// </summary>
        private void LoadSettings()
        {
            ddlFileExtensions.Items.Clear();
            foreach (string item in UserSettings.Extensions)
            {
                ddlFileExtensions.Items.Add(item);
            }

            ddlSearchCriteria.Items.Clear();
            foreach (string item in UserSettings.SearchTerms)
            {
                ddlSearchCriteria.Items.Add(item);
            }

            ddlBaseSearchPath.Items.Clear();
            foreach (string item in UserSettings.SearchPaths)
            {
                ddlBaseSearchPath.Items.Add(item);
            }

            // set app version on titlebar
            Title = string.Format("GREPPER v{0}.{1}.{2}", Assembly.GetAssembly(_fileController.GetType()).GetName().Version.Major,
                                                                Assembly.GetAssembly(_fileController.GetType()).GetName().Version.Minor,
                                                                Assembly.GetAssembly(_fileController.GetType()).GetName().Version.Build);

            if (UserSettings.RememberLastSearch)
            {
                SearchOptions so = UserSettings.SearchOptions;
                cbxMatchCase.IsChecked = so.MatchCase;
                cbxMatchPhrase.IsChecked = so.MatchPhrase;
                cbxRecursive.IsChecked = so.Recursive;
                cbxRegEx.IsChecked = !so.Literal;
                ddlFileExtensions.Text = so.Extensions;
                ddlSearchCriteria.Text = so.Search;
            }

            ddlBaseSearchPath.Text = UserSettings.GetInitialPath();

        }

        /// <summary>
        /// Persist current search options and combobox items to disk
        /// (including any changes to the window size/position/state)
        /// </summary>
        private void SaveCurrentSettings()
        {
            // ComboBoxes
            if (UserSettings.RememberExtensions)
            {
                UserSettings.Extensions.Clear();
                foreach (string item in ddlFileExtensions.Items)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        UserSettings.Extensions.Add(item);
                    }
                }
            }

            if (UserSettings.RememberSearchCriteria)
            {
                UserSettings.SearchTerms.Clear();
                foreach (string item in ddlSearchCriteria.Items)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        UserSettings.SearchTerms.Add(item);
                    }
                }
            }

            if (UserSettings.RememberSearchPaths)
            {
                UserSettings.SearchPaths.Clear();
                foreach (string item in ddlBaseSearchPath.Items)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        UserSettings.SearchPaths.Add(item);
                    }
                }
            }

            // Save Window State (do this here so it happens after search in addition to Window close)
            if (WindowState != WindowState.Minimized)
            {
                UserSettings.MainWindowState = WindowState;
            }

            // Search Options
            UserSettings.SearchOptions = GetSearchOptions();
            
            // Save Settings
            UserSettings.Save();
        }

        /// <summary>
        /// Add the current Text of a ComboBox to it's Items.
        /// If the string exists, move it to the top. Restrict count to maxItems.
        /// </summary>
        private void RememberComboBoxItem(ComboBox cb, int maxItems)
        {
            string text = cb.Text;
            if (!String.IsNullOrEmpty(text))
            {
                // if item already exists, it is moved to first place
                int idx = cb.Items.IndexOf(text);
                if (idx >= 0)
                {
                    cb.Items.RemoveAt(idx);
                    cb.Text = text; // Text is cleared when the corresponsing item is removed, so re-set it here 
                }

                cb.Items.Insert(0, text);

                // reduce to maxItems if needed
                for (int i = maxItems; i < cb.Items.Count; i++)
                {
                    cb.Items.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Attempts to open the selected file using the default program
        /// </summary>
        private void OpenSelectedFile()
        {
            var fileData = SelectedFile();
            if (fileData == null) return;
            using (var proc = new Process { StartInfo = new ProcessStartInfo(fileData.FilePath) })
            {
                proc.Start();
            }
        }

        /// <summary>
        /// Attempts to show the selected file in Explorer
        /// </summary>
        private void RevealSelectedFile()
        {
            var fileData = SelectedFile();
            if (fileData == null) return;
            string args = string.Format("/Select, {0}", fileData.FilePath);
            using (var proc = new Process { StartInfo = new ProcessStartInfo("explorer.exe", args) })
            {
                proc.Start();
            }
        }

        /// <summary>
        /// Display file contents in lower view section.
        /// </summary>
        private void ShowMatches()
        {
            lvwLineData.Items.Clear();
            if (lvwFileMatches.SelectedItems.Count > 0)
            {
                var fileData = SelectedFile();
                if (fileData == null)
                {
                    return;
                }

                foreach (var item in (Dictionary<long, LineData>)fileData.LineDataList)
                {
                    var lvi = new ListViewItem
                    {
                        Content = new LineMatchRow(item.Value)
                    };
                    lvwLineData.Items.Add(lvi);
                }
            }
        }

        #endregion

        #region Search Functionality

        /// <summary>
        /// Starts a new thread to search the given folder with the given parameters.
        /// </summary>
        private void ExecuteSearch()
        {
            // clear out any messages
            lblMessages.Content = string.Empty;
            lvwFileMatches.Items.Clear();
            lvwLineData.Items.Clear();

            // add items to dropdowns
            RememberComboBoxItem(ddlFileExtensions, (int)UserSettings.RememberExtensionsCount);
            RememberComboBoxItem(ddlSearchCriteria, (int)UserSettings.RememberSearchCriteriaCount);
            RememberComboBoxItem(ddlBaseSearchPath, (int)UserSettings.RememberSearchPathsCount);

            // save the current user option settings
            SaveCurrentSettings();

            // pass form data to file controller
            _fileController.SetFormData(GetSearchOptions());

            // create background worker thread to perform search so that UI does not lock up
            _workerThread = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _workerThread.DoWork += workerThread_DoWork;
            _workerThread.ProgressChanged += workerThread_ProgressChanged;
            _workerThread.RunWorkerCompleted += workerThread_RunWorkerCompleted;

            // reset & show progressBar
            progressBar.Visibility = Visibility.Visible;
            SetProgressBarValue(0);

            // start the thread
            _workerThread.RunWorkerAsync(_fileController);
        }

        private void CancelSearch()
        {
            _workerThread.CancelAsync();
        }

        /// <summary>
        /// Delegate method called when worker thread starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            // set search button text
            btnSearch.Dispatcher.Invoke(() => { btnSearch.Content = "Cancel"; });

            // get instance of FileController
            var fc = (FileController)e.Argument;

            // keep reference to BackgroundWorker in fc for progress notification 
            fc.Worker = (BackgroundWorker)sender;

            // perform search
            fc.GenerateFileData();

            // set DoWorkEventArgs Result to result of fc operation
            e.Result = fc;
        }

        /// <summary>
        /// Delegate method called when worker thread reports progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetProgressBarValue(e.ProgressPercentage);           

            // if UserState is not null, a file returned results so update the results list
            if (e.UserState != null)
            {
                var fd = (FileData)e.UserState;
                var item = new ListViewItem
                {
                    Content = new FileMatchRow(fd.LineDataList.Count.ToString(CultureInfo.InvariantCulture), fd.FilePath)
                };
                lvwFileMatches.Items.Add(item);

                //ResizeColumns(); // Not currently needed b/c right now space is always alotted for scrollbar, whether visible or not
                
                // If this is the first result, select it and focus on the ListView
                if (lvwFileMatches.Items.Count == 1)
                {
                    // Set sected index
                    lvwFileMatches.SelectedIndex = 0;

                    // Focus on selected list item
                    lvwFileMatches.UpdateLayout(); // Pre-generates item containers (Thanks http://stackoverflow.com/a/10463162)
                    item.Focus();
                }
            }
        }

        /// <summary>
        /// Delegate method called when worker thread is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // obtain results from worker thread and update UI as necessary
            var fc = (FileController)e.Result;

            // display any errors
            if (fc.MessageList != null)
            {
                foreach (string error in fc.MessageList)
                {
                    var item = new ListViewItem
                    {
                        Content = new FileMatchRow("ERROR", error),
                        Foreground = Brushes.Crimson
                    };
                    lvwFileMatches.Items.Add(item);
                }
            }

            // set total results
            lblMessages.Visibility = Visibility.Visible;
            string matches = fc.TotalMatches == 1 ? "" : "es";

            // display message if no results found
            if (fc.FileDataList == null || fc.FileDataList.Count < 1)
            {
                lblMessages.Content = "No results found.";
            }
            else
            {
                string files = fc.FileDataList.Count == 1 ? "" : "s";
                lblMessages.Content = string.Format("{0} match{1} in {2} file{3}", fc.TotalMatches, matches, fc.FileDataList.Count, files);
            }

            // hide progressBar
            SetProgressBarValue(100);
            progressBar.Visibility = Visibility.Hidden;

            // set search button text
            btnSearch.Dispatcher.Invoke(() => { btnSearch.Content = "Search"; });
        }

        /// <summary>
        /// Delegate method called when worker thread is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workerThread_RestartSearch(object sender, RunWorkerCompletedEventArgs e)
        {
            ((BackgroundWorker)sender).RunWorkerCompleted -= workerThread_RestartSearch; // Unsubscribe so this only happens once
            ExecuteSearch();
        }

        #endregion

    }

}
