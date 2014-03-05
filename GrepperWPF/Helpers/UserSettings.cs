using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows;
using GrepperLib.Model;
using GrepperWPF.Properties;

namespace GrepperWPF.Helpers
{
    public class UserSettings
    {
        #region Properties
        // ReSharper disable MemberCanBePrivate.Global

        public static StringCollection SearchTerms
        {
            get { return Settings.Default.SearchTerms; }
            set { Settings.Default.SearchTerms = value; }
        }

        public static StringCollection Extensions
        {
            get { return Settings.Default.Extensions; }
            set { Settings.Default.Extensions = value; }
        }

        public static StringCollection SearchPaths
        {
            get { return Settings.Default.SearchPaths; }
            set { Settings.Default.SearchPaths = value; }
        }

        public static SearchOptions SearchOptions
        {
            get { return Settings.Default.SearchOptions; }
            set { Settings.Default.SearchOptions = value; }
        }

        public static WindowSettings MainWindowSettings
        {
            get { return Settings.Default.MainWindowSettings; }
            set { Settings.Default.MainWindowSettings = value; }
        }

        public static WindowState MainWindowState
        {
            get { return Settings.Default.MainWindowState; }
            set { Settings.Default.MainWindowState = value; }
        }

        public static bool CopyLinesInSequence
        {
            get { return Settings.Default.CopyLinesInSequence; }
            set { Settings.Default.CopyLinesInSequence = value; }
        }

        public static bool RememberWindowSettings
        {
            get { return Settings.Default.RememberWindowSettings; }
            set { Settings.Default.RememberWindowSettings = value; }
        }

        public static bool RememberLastSearch
        {
            get { return Settings.Default.RememberLastSearch; }
            set { Settings.Default.RememberLastSearch = value; }
        }

        public static bool RememberSearchCriteria
        {
            get { return Settings.Default.RememberSearchCriteria; }
            set { Settings.Default.RememberSearchCriteria = value; }
        }

        public static bool RememberSearchPaths
        {
            get { return Settings.Default.RememberSearchPaths; }
            set { Settings.Default.RememberSearchPaths = value; }
        }

        public static bool RememberExtensions
        {
            get { return Settings.Default.RememberExtensions; }
            set { Settings.Default.RememberExtensions = value; }
        }

        public static uint RememberSearchCriteriaCount
        {
            get { return Settings.Default.RememberSearchCriteriaCount; }
            set { Settings.Default.RememberSearchCriteriaCount = value; }
        }

        public static uint RememberSearchPathsCount
        {
            get { return Settings.Default.RememberSearchPathsCount; }
            set { Settings.Default.RememberSearchPathsCount = value; }
        }

        public static uint RememberExtensionsCount
        {
            get { return Settings.Default.RememberExtensionsCount; }
            set { Settings.Default.RememberExtensionsCount = value; }
        }

        public static bool DoubleClickToOpen
        {
            get { return Settings.Default.DoubleClickToOpen; }
            set { Settings.Default.DoubleClickToOpen = value; }
        }

        public static bool ShiftDoubleClickToReveal
        {
            get { return Settings.Default.ShiftDoubleClickToReveal; }
            set { Settings.Default.ShiftDoubleClickToReveal = value; }
        }

        public static bool EnterToOpen
        {
            get { return Settings.Default.EnterToOpen; }
            set { Settings.Default.EnterToOpen = value; }
        }

        public static bool ShiftEnterToReveal
        {
            get { return Settings.Default.ShiftEnterToReveal; }
            set { Settings.Default.ShiftEnterToReveal = value; }
        }

        public static bool RightClickToCopy
        {
            get { return Settings.Default.RightClickToCopy; }
            set { Settings.Default.RightClickToCopy = value; }
        }

        public static bool TreatExcludePatternsAsRegex
        {
            get { return Settings.Default.TreatExcludePatternsAsRegex; }
            set { Settings.Default.TreatExcludePatternsAsRegex = value; }
        }
        
        // ReSharper restore MemberCanBePrivate.Global
        #endregion

        public static void InitDefaults()
        {
            // set default user settings
            if (Extensions == null)
            {
                Extensions = new StringCollection();
            }
            if (SearchTerms == null)
            {
                SearchTerms = new StringCollection();
            }
            if (SearchPaths == null)
            {
                SearchPaths = new StringCollection();
            }
            if (SearchOptions == null)
            {
                SearchOptions = new SearchOptions();
            }
        }

        public static string GetInitialPath()
        {
            string path;

            // Get initial path using arg or default
            CommandLineArguments a = CommandLineArguments.Args();
            if (!String.IsNullOrEmpty(a.path))
            {
                path = a.path;
            }
            else
            {
                if (!RememberLastSearch || String.IsNullOrEmpty(SearchOptions.Path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                else
                {
                    path = SearchOptions.Path;
                }
            }

            // right click on root of a drive causes a double-quote such as --> C:"
            var reg = new Regex("^[a-zA-Z][:]{1}");
            if (reg.Matches(path.Substring(0, 2)).Count > 0 && path.Substring(2, 1) == "\"")
            {
                path = string.Format("{0}\\", path.Substring(0, 2));
            }

            // if all else fails, use C:\ as path
            if (String.IsNullOrEmpty(path.Trim())) path = @"C:\";

            return path;
        }

        public static void Save()
        {
            Settings.Default.Save();
        }
    }
}
