using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using GrepperLib.Model;

namespace GrepperView
{
    public class UserSettings
    {
        public static StringCollection SearchTerms
        {
            get { return Properties.Settings.Default.SearchTerms; }
            set { Properties.Settings.Default.SearchTerms = value; } 
        }

        public static StringCollection Extensions
        {
            get { return Properties.Settings.Default.Extensions; }
            set { Properties.Settings.Default.Extensions = value; }
        }

        public static SearchOptions SearchOptions
        {
            get { return Properties.Settings.Default.SearchOptions; }
            set { Properties.Settings.Default.SearchOptions = value; }
        }

        public static void InitDefaults()
        {
            // set default user settings
            if (UserSettings.Extensions == null)
            {
                UserSettings.Extensions = new StringCollection();
            }
            if (UserSettings.SearchTerms == null)
            {
                UserSettings.SearchTerms = new StringCollection();
            }
            if (UserSettings.SearchOptions == null)
            {
                UserSettings.SearchOptions = new SearchOptions();
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
