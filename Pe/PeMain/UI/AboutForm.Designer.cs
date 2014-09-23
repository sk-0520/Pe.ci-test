/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class AboutForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.imageIcon = new System.Windows.Forms.PictureBox();
			this.commandOk = new System.Windows.Forms.Button();
			this.labelAppName = new System.Windows.Forms.Label();
			this.labelAppVersion = new System.Windows.Forms.Label();
			this.linkWeb = new System.Windows.Forms.LinkLabel();
			this.linkDev = new System.Windows.Forms.LinkLabel();
			this.commandExecuteDir = new System.Windows.Forms.Button();
			this.commandDataDir = new System.Windows.Forms.Button();
			this.commandBackupDir = new System.Windows.Forms.Button();
			this.labelConfiguration = new System.Windows.Forms.Label();
			this.linkMail = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.imageIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// imageIcon
			// 
			this.imageIcon.Location = new System.Drawing.Point(12, 12);
			this.imageIcon.Name = "imageIcon";
			this.imageIcon.Size = new System.Drawing.Size(48, 48);
			this.imageIcon.TabIndex = 0;
			this.imageIcon.TabStop = false;
			// 
			// commandOk
			// 
			this.commandOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.commandOk.Location = new System.Drawing.Point(252, 150);
			this.commandOk.Name = "commandOk";
			this.commandOk.Size = new System.Drawing.Size(101, 31);
			this.commandOk.TabIndex = 0;
			this.commandOk.Text = "{OK}";
			this.commandOk.UseVisualStyleBackColor = true;
			this.commandOk.Click += new System.EventHandler(this.CommandOk_Click);
			// 
			// labelAppName
			// 
			this.labelAppName.AutoSize = true;
			this.labelAppName.Location = new System.Drawing.Point(66, 12);
			this.labelAppName.Name = "labelAppName";
			this.labelAppName.Size = new System.Drawing.Size(91, 15);
			this.labelAppName.TabIndex = 1;
			this.labelAppName.Text = "labelAppName";
			// 
			// labelAppVersion
			// 
			this.labelAppVersion.AutoSize = true;
			this.labelAppVersion.Location = new System.Drawing.Point(66, 36);
			this.labelAppVersion.Name = "labelAppVersion";
			this.labelAppVersion.Size = new System.Drawing.Size(98, 15);
			this.labelAppVersion.TabIndex = 2;
			this.labelAppVersion.Text = "labelAppVersion";
			// 
			// linkWeb
			// 
			this.linkWeb.AutoEllipsis = true;
			this.linkWeb.AutoSize = true;
			this.linkWeb.Location = new System.Drawing.Point(12, 96);
			this.linkWeb.Name = "linkWeb";
			this.linkWeb.Size = new System.Drawing.Size(103, 15);
			this.linkWeb.TabIndex = 1;
			this.linkWeb.TabStop = true;
			this.linkWeb.Text = "http://web-page";
			this.linkWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// linkDev
			// 
			this.linkDev.AutoEllipsis = true;
			this.linkDev.AutoSize = true;
			this.linkDev.Location = new System.Drawing.Point(12, 150);
			this.linkDev.Name = "linkDev";
			this.linkDev.Size = new System.Drawing.Size(100, 15);
			this.linkDev.TabIndex = 3;
			this.linkDev.TabStop = true;
			this.linkDev.Text = "http://dev-page";
			this.linkDev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// commandExecuteDir
			// 
			this.commandExecuteDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandExecuteDir.Image = global::PeMain.Properties.Images.Dir;
			this.commandExecuteDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandExecuteDir.Location = new System.Drawing.Point(252, 12);
			this.commandExecuteDir.Name = "commandExecuteDir";
			this.commandExecuteDir.Size = new System.Drawing.Size(101, 27);
			this.commandExecuteDir.TabIndex = 4;
			this.commandExecuteDir.Text = ":about/command/execute";
			this.commandExecuteDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandExecuteDir.UseVisualStyleBackColor = true;
			this.commandExecuteDir.Click += new System.EventHandler(this.CommandExecuteDir_Click);
			// 
			// commandDataDir
			// 
			this.commandDataDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandDataDir.Image = global::PeMain.Properties.Images.Dir;
			this.commandDataDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandDataDir.Location = new System.Drawing.Point(252, 44);
			this.commandDataDir.Name = "commandDataDir";
			this.commandDataDir.Size = new System.Drawing.Size(101, 27);
			this.commandDataDir.TabIndex = 5;
			this.commandDataDir.Text = ":about/command/data";
			this.commandDataDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandDataDir.UseVisualStyleBackColor = true;
			this.commandDataDir.Click += new System.EventHandler(this.CommandDataDir_Click);
			// 
			// commandBackupDir
			// 
			this.commandBackupDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandBackupDir.Image = global::PeMain.Properties.Images.Dir;
			this.commandBackupDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandBackupDir.Location = new System.Drawing.Point(252, 76);
			this.commandBackupDir.Name = "commandBackupDir";
			this.commandBackupDir.Size = new System.Drawing.Size(101, 27);
			this.commandBackupDir.TabIndex = 6;
			this.commandBackupDir.Text = ":about/command/backup";
			this.commandBackupDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandBackupDir.UseVisualStyleBackColor = true;
			this.commandBackupDir.Click += new System.EventHandler(this.CommandBackupDir_Click);
			// 
			// labelConfiguration
			// 
			this.labelConfiguration.AutoSize = true;
			this.labelConfiguration.Location = new System.Drawing.Point(66, 60);
			this.labelConfiguration.Name = "labelConfiguration";
			this.labelConfiguration.Size = new System.Drawing.Size(111, 15);
			this.labelConfiguration.TabIndex = 2;
			this.labelConfiguration.Text = "labelConfiguration";
			// 
			// linkMail
			// 
			this.linkMail.AutoEllipsis = true;
			this.linkMail.AutoSize = true;
			this.linkMail.Location = new System.Drawing.Point(12, 123);
			this.linkMail.Name = "linkMail";
			this.linkMail.Size = new System.Drawing.Size(124, 15);
			this.linkMail.TabIndex = 2;
			this.linkMail.TabStop = true;
			this.linkMail.Text = "mailto:mail-address";
			this.linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.commandOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(365, 193);
			this.Controls.Add(this.linkMail);
			this.Controls.Add(this.commandBackupDir);
			this.Controls.Add(this.commandDataDir);
			this.Controls.Add(this.commandExecuteDir);
			this.Controls.Add(this.linkDev);
			this.Controls.Add(this.linkWeb);
			this.Controls.Add(this.labelConfiguration);
			this.Controls.Add(this.labelAppVersion);
			this.Controls.Add(this.labelAppName);
			this.Controls.Add(this.imageIcon);
			this.Controls.Add(this.commandOk);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/about";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.imageIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.LinkLabel linkMail;
		private System.Windows.Forms.Label labelConfiguration;
		private System.Windows.Forms.Button commandBackupDir;
		private System.Windows.Forms.Button commandDataDir;
		private System.Windows.Forms.Button commandExecuteDir;
		private System.Windows.Forms.LinkLabel linkDev;
		private System.Windows.Forms.LinkLabel linkWeb;
		private System.Windows.Forms.Label labelAppVersion;
		private System.Windows.Forms.Label labelAppName;
		private System.Windows.Forms.Button commandOk;
		private System.Windows.Forms.PictureBox imageIcon;
	}
}
