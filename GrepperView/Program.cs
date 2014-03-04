using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Test.CommandLineParsing;
using Grepper.ContextMenu;
using GrepperLib.Model;

namespace GrepperView
{
    public class CommandLineArguments
    {
        public int? contextmenu { get; set; }
        public string path { get; set; }
    }
    
    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            string path;
            CommandLineArguments a = new CommandLineArguments();
            try
            {
                CommandLineParser.ParseArguments(a, args);
            }
            catch (Exception)
            {
                // Allow path to be passed as a string
                path = args[0];

                // Support legacy context menu param format
                if (path.StartsWith("-p"))
                {
                    path = path.Substring(2);
                }
            }

            // Enable/disable the context menu (requires Administrator permissions)
            //  Passing this param simply sets the registry value and exits
            if (a.contextmenu.HasValue)
            {
                if (a.contextmenu.Value == 1)
                {
                    // add context menu if it does not exist
                    RegistrySettings.AddContextMenu(Application.ExecutablePath);
                }
                else
                {
                    // remove context menu if it exists
                    RegistrySettings.RemoveContextMenu();
                }
                return;
            }

            // Set initial path using arg or default
            if (String.IsNullOrEmpty(a.path))
            {
                // set remembered or default search path if no args passed
                if (UserSettings.SearchOptions == null
                    || String.IsNullOrEmpty(UserSettings.SearchOptions.path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                else
                {
                    path = UserSettings.SearchOptions.path;
                }
            }
            else
            {
                path = a.path;
            }

            // right click on root of a drive causes a double-quote such as --> C:"
            Regex reg = new Regex("^[a-zA-Z][:]{1}");
            if (reg.Matches(path.Substring(0, 2)).Count > 0 && path.Substring(2, 1) == "\"")
            {
                path = string.Format("{0}\\", path.Substring(0, 2));
            }
            
            // if all else fails, use C:\ as path
            if (path == null) path = @"C:\";

            // set default user settings
            UserSettings.InitDefaults();

            // start the app
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainUI(path));
		}
	}
}
