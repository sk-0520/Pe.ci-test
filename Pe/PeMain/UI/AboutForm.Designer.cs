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
			this.imageIcon = new System.Windows.Forms.PictureBox();
			this.commandOk = new System.Windows.Forms.Button();
			this.labelAppName = new System.Windows.Forms.Label();
			this.labelAppVersion = new System.Windows.Forms.Label();
			this.linkWeb = new System.Windows.Forms.LinkLabel();
			this.linkRepository = new System.Windows.Forms.LinkLabel();
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
			this.commandOk.Location = new System.Drawing.Point(138, 150);
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
			this.linkWeb.Location = new System.Drawing.Point(12, 76);
			this.linkWeb.Name = "linkWeb";
			this.linkWeb.Size = new System.Drawing.Size(173, 15);
			this.linkWeb.TabIndex = 3;
			this.linkWeb.TabStop = true;
			this.linkWeb.Text = "http://content-type-text.net";
			this.linkWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// linkRepository
			// 
			this.linkRepository.AutoEllipsis = true;
			this.linkRepository.AutoSize = true;
			this.linkRepository.Location = new System.Drawing.Point(12, 101);
			this.linkRepository.Name = "linkRepository";
			this.linkRepository.Size = new System.Drawing.Size(202, 15);
			this.linkRepository.TabIndex = 3;
			this.linkRepository.TabStop = true;
			this.linkRepository.Text = "https://bitbucket.org/sk_0520/pe";
			this.linkRepository.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.commandOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(251, 193);
			this.Controls.Add(this.linkRepository);
			this.Controls.Add(this.linkWeb);
			this.Controls.Add(this.labelAppVersion);
			this.Controls.Add(this.labelAppName);
			this.Controls.Add(this.imageIcon);
			this.Controls.Add(this.commandOk);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = global::PeMain.Properties.Images.Pe;
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
		private System.Windows.Forms.LinkLabel linkRepository;
		private System.Windows.Forms.LinkLabel linkWeb;
		private System.Windows.Forms.Label labelAppVersion;
		private System.Windows.Forms.Label labelAppName;
		private System.Windows.Forms.Button commandOk;
		private System.Windows.Forms.PictureBox imageIcon;
	}
}
