using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Grepper.ContextMenu;

namespace GrepperView
{
    public partial class ExtensionsUI : Form
    {
        public ExtensionsUI()
        {
            InitializeComponent();
            this.LoadExtensions();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbExtensions.SelectedItem != null)
            {
                UserSettings.Extensions.Remove(lbExtensions.SelectedItem.ToString());
                UserSettings.Save();

                LoadExtensions(); // update UI
            }
        }

        /// <summary>
        /// Loads extensions saved in the registry.
        /// </summary>
        private void LoadExtensions()
        {
            lbExtensions.Items.Clear();
            foreach (string item in UserSettings.Extensions)
            {
                lbExtensions.Items.Add(item);
            }
        }
    }
}
