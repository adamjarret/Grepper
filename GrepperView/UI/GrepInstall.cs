using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace GrepperView
{
	[RunInstaller(true)]
	public partial class GrepInstall : Installer
	{
		public GrepInstall()
		{
			InitializeComponent();
			this.Committed += GrepInstall_Committed;
		}

		private void GrepInstall_Committed(object sender, InstallEventArgs e)
		{
			try
			{
                string parentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); //GetEntryAssembly
                Directory.SetCurrentDirectory(parentPath);
				string path = String.Format("{0}\\Grepper.exe", parentPath);
                ProcessStartInfo startInfo = new ProcessStartInfo(path, "/contextmenu=1");
                //startInfo.Verb = "runas"; //trigger a UAC prompt (if UAC is enabled) 
                Process.Start(startInfo);
			}
			catch { }
		}

		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);
		}

		protected override void OnCommitted(System.Collections.IDictionary savedState)
		{
			base.OnCommitted(savedState);
		}

		public override void Commit(System.Collections.IDictionary savedState)
		{
			base.Commit(savedState);
		}

		public override void Rollback(System.Collections.IDictionary savedState)
		{
			base.Rollback(savedState);
		}
	}
}
