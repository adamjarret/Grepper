using System;
using System.Linq;
using System.Reflection;
using GrepperWPF.Helpers;
using Microsoft.Test.CommandLineParsing;
using Grepper.ContextMenu;

namespace GrepperWPF
{
    public class CommandLineArguments
    {
        // ReSharper disable InconsistentNaming, UnusedAutoPropertyAccessor.Global, MemberCanBePrivate.Global
        public int? contextmenu { get; set; }
        public string path { get; set; }
        public string exePath { get; set; }
        // ReSharper restore InconsistentNaming, UnusedAutoPropertyAccessor.Global, MemberCanBePrivate.Global

        public static CommandLineArguments Args()
        {
            var args = Environment.GetCommandLineArgs();
            var exePath = args[0];
            args = args.Skip(1).ToArray();
            var a = new CommandLineArguments();
            a.ParseArguments(args);
            a.exePath = exePath;
            return a;
        }
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            // Enable/disable the context menu if /contextmenu argument is passed
            //  Passing this param simply sets the registry value and exits
            //  ** requires Administrator permissions **
            var a = CommandLineArguments.Args();
            if (a.contextmenu.HasValue)
            {
                if (a.contextmenu.Value == 1)
                {
                    // add context menu if it does not exist
                    RegistrySettings.AddContextMenu(Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    // remove context menu if it exists
                    RegistrySettings.RemoveContextMenu();
                }
                Current.Shutdown();
            }

            // set default user settings
            UserSettings.InitDefaults();
        }
    }
}
