﻿namespace C8UpdateService {
	partial class ProjectInstaller {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ZitiUpdateProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.C8UpdateServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// ZitiUpdateProcessInstaller
			// 
			this.ZitiUpdateProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.ZitiUpdateProcessInstaller.Password = null;
			this.ZitiUpdateProcessInstaller.Username = null;
			// 
			// C8UpdateServiceInstaller
			// 
			this.C8UpdateServiceInstaller.Description = "Keep Ziti Service Software Up To Date";
			this.C8UpdateServiceInstaller.DisplayName = "Ziti Update Service";
			this.C8UpdateServiceInstaller.ServiceName = "C8UpdateService";
			this.C8UpdateServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			this.C8UpdateServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.C8UpdateServiceInstaller_AfterInstall);
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ZitiUpdateProcessInstaller,
            this.C8UpdateServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller ZitiUpdateProcessInstaller;
		private System.ServiceProcess.ServiceInstaller C8UpdateServiceInstaller;
	}
}