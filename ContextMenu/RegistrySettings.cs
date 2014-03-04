using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;

namespace Grepper.ContextMenu
{
    public static class RegistrySettings
    {
        private static string _grepperPath          = "Folder\\Shell\\Grepper\\";
        private static string _commandPath          = _grepperPath + "Command";

        /// <summary>
        /// Adds the right-click context menu item 'Grepper' 
        /// </summary>
        /// <param name="path">File system path to grepper.exe</param>
        public static void AddContextMenu(string path)
        {
            // Key Exists?
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(_grepperPath, true);
            if (key == null) key = Registry.ClassesRoot.CreateSubKey(_grepperPath);
            key = Registry.ClassesRoot.OpenSubKey(_commandPath, true);
            if (key == null) key = Registry.ClassesRoot.CreateSubKey(_commandPath);
            object keyData = key.GetValue("");
            if ((keyData == null) || (keyData.ToString().Length < 1))
            {
                path = string.Format(@"""{0}"" /path=""%1""", path);
                key.SetValue("", path);
            }
        }

        /// <summary>
        /// Removes the right-click context menu item 'Grepper' 
        /// </summary>
        public static void RemoveContextMenu()
        {
            // Key Exists?
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(_grepperPath, true);
            if (key == null) return;

            Registry.ClassesRoot.DeleteSubKey(_grepperPath);
        }

    }
}