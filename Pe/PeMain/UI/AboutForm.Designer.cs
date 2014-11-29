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
			this.linkAbout = new System.Windows.Forms.LinkLabel();
			this.linkDevelopment = new System.Windows.Forms.LinkLabel();
			this.commandExecuteDir = new System.Windows.Forms.Button();
			this.commandDataDir = new System.Windows.Forms.Button();
			this.commandBackupDir = new System.Windows.Forms.Button();
			this.labelConfiguration = new System.Windows.Forms.Label();
			this.linkMail = new System.Windows.Forms.LinkLabel();
			this.commandUpdate = new System.Windows.Forms.Button();
			this.gridComponents = new System.Windows.Forms.DataGridView();
			this.commandChangelog = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.linkDiscussion = new System.Windows.Forms.LinkLabel();
			this.panelWeb = new System.Windows.Forms.FlowLayoutPanel();
			this.panelEnv = new System.Windows.Forms.FlowLayoutPanel();
			this.labelUserenv = new System.Windows.Forms.Label();
			this.linkCopyShort = new System.Windows.Forms.LinkLabel();
			this.linkCopyLong = new System.Windows.Forms.LinkLabel();
			this.gridComponents_columnName = new System.Windows.Forms.DataGridViewLinkColumn();
			this.gridComponents_columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.imageIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridComponents)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelWeb.SuspendLayout();
			this.panelEnv.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageIcon
			// 
			this.imageIcon.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageIcon.Location = new System.Drawing.Point(6, 6);
			this.imageIcon.Name = "imageIcon";
			this.tableLayoutPanel1.SetRowSpan(this.imageIcon, 2);
			this.imageIcon.Size = new System.Drawing.Size(48, 48);
			this.imageIcon.TabIndex = 0;
			this.imageIcon.TabStop = false;
			// 
			// commandOk
			// 
			this.commandOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.commandOk.Location = new System.Drawing.Point(523, 227);
			this.commandOk.Name = "commandOk";
			this.commandOk.Size = new System.Drawing.Size(101, 31);
			this.commandOk.TabIndex = 7;
			this.commandOk.Text = "{OK}";
			this.commandOk.UseVisualStyleBackColor = true;
			this.commandOk.Click += new System.EventHandler(this.CommandOk_Click);
			// 
			// labelAppName
			// 
			this.labelAppName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelAppName.AutoSize = true;
			this.labelAppName.Location = new System.Drawing.Point(63, 13);
			this.labelAppName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
			this.labelAppName.Name = "labelAppName";
			this.labelAppName.Size = new System.Drawing.Size(91, 15);
			this.labelAppName.TabIndex = 1;
			this.labelAppName.Text = "labelAppName";
			// 
			// labelAppVersion
			// 
			this.labelAppVersion.AutoSize = true;
			this.labelAppVersion.Location = new System.Drawing.Point(63, 32);
			this.labelAppVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
			this.labelAppVersion.Name = "labelAppVersion";
			this.labelAppVersion.Size = new System.Drawing.Size(98, 15);
			this.labelAppVersion.TabIndex = 2;
			this.labelAppVersion.Text = "labelAppVersion";
			// 
			// linkAbout
			// 
			this.linkAbout.AutoEllipsis = true;
			this.linkAbout.AutoSize = true;
			this.linkAbout.Location = new System.Drawing.Point(3, 0);
			this.linkAbout.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
			this.linkAbout.Name = "linkAbout";
			this.linkAbout.Size = new System.Drawing.Size(112, 15);
			this.linkAbout.TabIndex = 0;
			this.linkAbout.TabStop = true;
			this.linkAbout.Text = "http://page-about";
			this.linkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// linkDevelopment
			// 
			this.linkDevelopment.AutoEllipsis = true;
			this.linkDevelopment.AutoSize = true;
			this.linkDevelopment.Location = new System.Drawing.Point(3, 40);
			this.linkDevelopment.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
			this.linkDevelopment.Name = "linkDevelopment";
			this.linkDevelopment.Size = new System.Drawing.Size(155, 15);
			this.linkDevelopment.TabIndex = 2;
			this.linkDevelopment.TabStop = true;
			this.linkDevelopment.Text = "http://page-development";
			this.linkDevelopment.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// commandExecuteDir
			// 
			this.commandExecuteDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandExecuteDir.Image = global::PeMain.Properties.Images.Dir;
			this.commandExecuteDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandExecuteDir.Location = new System.Drawing.Point(523, 12);
			this.commandExecuteDir.Name = "commandExecuteDir";
			this.commandExecuteDir.Size = new System.Drawing.Size(101, 27);
			this.commandExecuteDir.TabIndex = 2;
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
			this.commandDataDir.Location = new System.Drawing.Point(523, 44);
			this.commandDataDir.Name = "commandDataDir";
			this.commandDataDir.Size = new System.Drawing.Size(101, 27);
			this.commandDataDir.TabIndex = 3;
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
			this.commandBackupDir.Location = new System.Drawing.Point(523, 76);
			this.commandBackupDir.Name = "commandBackupDir";
			this.commandBackupDir.Size = new System.Drawing.Size(101, 27);
			this.commandBackupDir.TabIndex = 4;
			this.commandBackupDir.Text = ":about/command/backup";
			this.commandBackupDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandBackupDir.UseVisualStyleBackColor = true;
			this.commandBackupDir.Click += new System.EventHandler(this.CommandBackupDir_Click);
			// 
			// labelConfiguration
			// 
			this.labelConfiguration.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.labelConfiguration, 2);
			this.labelConfiguration.Location = new System.Drawing.Point(3, 60);
			this.labelConfiguration.Name = "labelConfiguration";
			this.labelConfiguration.Size = new System.Drawing.Size(111, 15);
			this.labelConfiguration.TabIndex = 2;
			this.labelConfiguration.Text = "labelConfiguration";
			// 
			// linkMail
			// 
			this.linkMail.AutoEllipsis = true;
			this.linkMail.AutoSize = true;
			this.linkMail.Location = new System.Drawing.Point(3, 20);
			this.linkMail.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
			this.linkMail.Name = "linkMail";
			this.linkMail.Size = new System.Drawing.Size(124, 15);
			this.linkMail.TabIndex = 1;
			this.linkMail.TabStop = true;
			this.linkMail.Text = "mailto:mail-address";
			this.linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// commandUpdate
			// 
			this.commandUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandUpdate.Image = global::PeMain.Properties.Images.Update;
			this.commandUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandUpdate.Location = new System.Drawing.Point(523, 170);
			this.commandUpdate.Name = "commandUpdate";
			this.commandUpdate.Size = new System.Drawing.Size(101, 27);
			this.commandUpdate.TabIndex = 6;
			this.commandUpdate.Text = ":about/command/update";
			this.commandUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandUpdate.UseVisualStyleBackColor = true;
			this.commandUpdate.Click += new System.EventHandler(this.CommandUpdate_Click);
			// 
			// gridComponents
			// 
			this.gridComponents.AllowUserToAddRows = false;
			this.gridComponents.AllowUserToDeleteRows = false;
			this.gridComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridComponents_columnName,
            this.gridComponents_columnType});
			this.gridComponents.Location = new System.Drawing.Point(12, 137);
			this.gridComponents.MultiSelect = false;
			this.gridComponents.Name = "gridComponents";
			this.gridComponents.ReadOnly = true;
			this.gridComponents.RowTemplate.Height = 21;
			this.gridComponents.Size = new System.Drawing.Size(505, 121);
			this.gridComponents.TabIndex = 1;
			this.gridComponents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridComponents_CellContentClick);
			// 
			// commandChangelog
			// 
			this.commandChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandChangelog.Image = global::PeMain.Properties.Images.Changelog;
			this.commandChangelog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandChangelog.Location = new System.Drawing.Point(523, 137);
			this.commandChangelog.Name = "commandChangelog";
			this.commandChangelog.Size = new System.Drawing.Size(101, 27);
			this.commandChangelog.TabIndex = 5;
			this.commandChangelog.Text = ":about/command/changelog";
			this.commandChangelog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandChangelog.UseVisualStyleBackColor = true;
			this.commandChangelog.Click += new System.EventHandler(this.CommandChangelog_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.imageIcon, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelAppName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelAppVersion, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelConfiguration, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(161, 91);
			this.tableLayoutPanel1.TabIndex = 10;
			// 
			// linkDiscussion
			// 
			this.linkDiscussion.AutoEllipsis = true;
			this.linkDiscussion.AutoSize = true;
			this.linkDiscussion.Location = new System.Drawing.Point(3, 60);
			this.linkDiscussion.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
			this.linkDiscussion.Name = "linkDiscussion";
			this.linkDiscussion.Size = new System.Drawing.Size(137, 15);
			this.linkDiscussion.TabIndex = 3;
			this.linkDiscussion.TabStop = true;
			this.linkDiscussion.Text = "http://page-discussion";
			this.linkDiscussion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// panelWeb
			// 
			this.panelWeb.AutoScroll = true;
			this.panelWeb.Controls.Add(this.linkAbout);
			this.panelWeb.Controls.Add(this.linkMail);
			this.panelWeb.Controls.Add(this.linkDevelopment);
			this.panelWeb.Controls.Add(this.linkDiscussion);
			this.panelWeb.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.panelWeb.Location = new System.Drawing.Point(179, 12);
			this.panelWeb.Name = "panelWeb";
			this.panelWeb.Size = new System.Drawing.Size(338, 91);
			this.panelWeb.TabIndex = 0;
			// 
			// panelEnv
			// 
			this.panelEnv.Controls.Add(this.labelUserenv);
			this.panelEnv.Controls.Add(this.linkCopyShort);
			this.panelEnv.Controls.Add(this.linkCopyLong);
			this.panelEnv.Location = new System.Drawing.Point(12, 109);
			this.panelEnv.Name = "panelEnv";
			this.panelEnv.Size = new System.Drawing.Size(505, 22);
			this.panelEnv.TabIndex = 11;
			// 
			// labelUserenv
			// 
			this.labelUserenv.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelUserenv.AutoSize = true;
			this.labelUserenv.Location = new System.Drawing.Point(3, 0);
			this.labelUserenv.Name = "labelUserenv";
			this.labelUserenv.Size = new System.Drawing.Size(135, 15);
			this.labelUserenv.TabIndex = 1;
			this.labelUserenv.Text = ":about/label/user-env";
			// 
			// linkCopyShort
			// 
			this.linkCopyShort.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.linkCopyShort.AutoSize = true;
			this.linkCopyShort.Location = new System.Drawing.Point(144, 0);
			this.linkCopyShort.Name = "linkCopyShort";
			this.linkCopyShort.Size = new System.Drawing.Size(133, 15);
			this.linkCopyShort.TabIndex = 0;
			this.linkCopyShort.TabStop = true;
			this.linkCopyShort.Text = ":about/link/short-env";
			this.linkCopyShort.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCopyShort_LinkClicked);
			// 
			// linkCopyLong
			// 
			this.linkCopyLong.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.linkCopyLong.AutoSize = true;
			this.linkCopyLong.Location = new System.Drawing.Point(283, 0);
			this.linkCopyLong.Name = "linkCopyLong";
			this.linkCopyLong.Size = new System.Drawing.Size(131, 15);
			this.linkCopyLong.TabIndex = 0;
			this.linkCopyLong.TabStop = true;
			this.linkCopyLong.Text = ":about/long/long-env";
			this.linkCopyLong.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCopyLong_LinkClicked);
			// 
			// gridComponents_columnName
			// 
			this.gridComponents_columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.gridComponents_columnName.HeaderText = ":about/column/name";
			this.gridComponents_columnName.Name = "gridComponents_columnName";
			this.gridComponents_columnName.ReadOnly = true;
			this.gridComponents_columnName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridComponents_columnName.Width = 138;
			// 
			// gridComponents_columnType
			// 
			this.gridComponents_columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.gridComponents_columnType.HeaderText = ":about/column/type";
			this.gridComponents_columnType.Name = "gridComponents_columnType";
			this.gridComponents_columnType.ReadOnly = true;
			this.gridComponents_columnType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.gridComponents_columnType.Width = 131;
			// 
			// AboutForm
			// 
			this.AcceptButton = this.commandOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(636, 270);
			this.Controls.Add(this.panelEnv);
			this.Controls.Add(this.panelWeb);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.commandChangelog);
			this.Controls.Add(this.gridComponents);
			this.Controls.Add(this.commandUpdate);
			this.Controls.Add(this.commandBackupDir);
			this.Controls.Add(this.commandDataDir);
			this.Controls.Add(this.commandExecuteDir);
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
			((System.ComponentModel.ISupportInitialize)(this.gridComponents)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panelWeb.ResumeLayout(false);
			this.panelWeb.PerformLayout();
			this.panelEnv.ResumeLayout(false);
			this.panelEnv.PerformLayout();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.FlowLayoutPanel panelWeb;
		private System.Windows.Forms.LinkLabel linkDiscussion;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button commandChangelog;
		private System.Windows.Forms.DataGridView gridComponents;
		private System.Windows.Forms.Button commandUpdate;
		private System.Windows.Forms.LinkLabel linkMail;
		private System.Windows.Forms.Label labelConfiguration;
		private System.Windows.Forms.Button commandBackupDir;
		private System.Windows.Forms.Button commandDataDir;
		private System.Windows.Forms.Button commandExecuteDir;
		private System.Windows.Forms.LinkLabel linkDevelopment;
		private System.Windows.Forms.LinkLabel linkAbout;
		private System.Windows.Forms.Label labelAppVersion;
		private System.Windows.Forms.Label labelAppName;
		private System.Windows.Forms.Button commandOk;
		private System.Windows.Forms.PictureBox imageIcon;
		private System.Windows.Forms.FlowLayoutPanel panelEnv;
		private System.Windows.Forms.Label labelUserenv;
		private System.Windows.Forms.LinkLabel linkCopyShort;
		private System.Windows.Forms.LinkLabel linkCopyLong;
		private System.Windows.Forms.DataGridViewLinkColumn gridComponents_columnName;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridComponents_columnType;
	}
}
