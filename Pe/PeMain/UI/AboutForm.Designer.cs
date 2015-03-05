/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace ContentTypeTextNet.Pe.PeMain.UI
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
			this.labelConfiguration = new System.Windows.Forms.Label();
			this.linkMail = new System.Windows.Forms.LinkLabel();
			this.commandUpdate = new System.Windows.Forms.Button();
			this.gridComponents = new System.Windows.Forms.DataGridView();
			this.gridComponents_columnName = new System.Windows.Forms.DataGridViewLinkColumn();
			this.gridComponents_columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridComponents_columnLicense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.commandChangelog = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.linkDiscussion = new System.Windows.Forms.LinkLabel();
			this.panelWeb = new System.Windows.Forms.FlowLayoutPanel();
			this.linkFeedback = new System.Windows.Forms.LinkLabel();
			this.panelEnv = new System.Windows.Forms.FlowLayoutPanel();
			this.labelUserenv = new System.Windows.Forms.Label();
			this.linkCopyShort = new System.Windows.Forms.LinkLabel();
			this.linkCopyLong = new System.Windows.Forms.LinkLabel();
			this.tabAbout = new System.Windows.Forms.TabControl();
			this.tabAbout_pageApp = new System.Windows.Forms.TabPage();
			this.panelApp = new System.Windows.Forms.FlowLayoutPanel();
			this.tabAbout_pageComponent = new System.Windows.Forms.TabPage();
			this.panelCommand = new System.Windows.Forms.FlowLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.imageIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridComponents)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelWeb.SuspendLayout();
			this.panelEnv.SuspendLayout();
			this.tabAbout.SuspendLayout();
			this.tabAbout_pageApp.SuspendLayout();
			this.panelApp.SuspendLayout();
			this.tabAbout_pageComponent.SuspendLayout();
			this.panelCommand.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
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
			this.commandOk.Location = new System.Drawing.Point(532, 236);
			this.commandOk.Name = "commandOk";
			this.commandOk.Size = new System.Drawing.Size(101, 31);
			this.commandOk.TabIndex = 8;
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
			this.labelAppVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelAppVersion.AutoEllipsis = true;
			this.labelAppVersion.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.labelAppVersion, 2);
			this.labelAppVersion.Location = new System.Drawing.Point(3, 62);
			this.labelAppVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
			this.labelAppVersion.Name = "labelAppVersion";
			this.labelAppVersion.Size = new System.Drawing.Size(98, 15);
			this.labelAppVersion.TabIndex = 2;
			this.labelAppVersion.Text = "labelAppVersion";
			this.labelAppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// linkAbout
			// 
			this.linkAbout.AutoEllipsis = true;
			this.linkAbout.AutoSize = true;
			this.linkAbout.Location = new System.Drawing.Point(3, 0);
			this.linkAbout.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
			this.linkAbout.Name = "linkAbout";
			this.linkAbout.Size = new System.Drawing.Size(150, 15);
			this.linkAbout.TabIndex = 0;
			this.linkAbout.TabStop = true;
			this.linkAbout.Text = ":about/label/page-about";
			this.linkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// linkDevelopment
			// 
			this.linkDevelopment.AutoEllipsis = true;
			this.linkDevelopment.AutoSize = true;
			this.linkDevelopment.Location = new System.Drawing.Point(3, 38);
			this.linkDevelopment.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
			this.linkDevelopment.Name = "linkDevelopment";
			this.linkDevelopment.Size = new System.Drawing.Size(193, 15);
			this.linkDevelopment.TabIndex = 2;
			this.linkDevelopment.TabStop = true;
			this.linkDevelopment.Text = ":about/label/page-development";
			this.linkDevelopment.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// commandExecuteDir
			// 
			this.commandExecuteDir.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandExecuteDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandExecuteDir.Location = new System.Drawing.Point(3, 3);
			this.commandExecuteDir.Name = "commandExecuteDir";
			this.commandExecuteDir.Size = new System.Drawing.Size(97, 31);
			this.commandExecuteDir.TabIndex = 4;
			this.commandExecuteDir.Text = ":about/command/execute";
			this.commandExecuteDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandExecuteDir.UseVisualStyleBackColor = true;
			this.commandExecuteDir.Click += new System.EventHandler(this.CommandExecuteDir_Click);
			// 
			// commandDataDir
			// 
			this.commandDataDir.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandDataDir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandDataDir.Location = new System.Drawing.Point(106, 3);
			this.commandDataDir.Name = "commandDataDir";
			this.commandDataDir.Size = new System.Drawing.Size(97, 31);
			this.commandDataDir.TabIndex = 5;
			this.commandDataDir.Text = ":about/command/data";
			this.commandDataDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandDataDir.UseVisualStyleBackColor = true;
			this.commandDataDir.Click += new System.EventHandler(this.CommandDataDir_Click);
			// 
			// labelConfiguration
			// 
			this.labelConfiguration.AutoSize = true;
			this.labelConfiguration.Location = new System.Drawing.Point(63, 30);
			this.labelConfiguration.Name = "labelConfiguration";
			this.labelConfiguration.Size = new System.Drawing.Size(111, 15);
			this.labelConfiguration.TabIndex = 2;
			this.labelConfiguration.Text = "labelConfiguration";
			// 
			// linkMail
			// 
			this.linkMail.AutoEllipsis = true;
			this.linkMail.AutoSize = true;
			this.linkMail.Location = new System.Drawing.Point(3, 19);
			this.linkMail.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
			this.linkMail.Name = "linkMail";
			this.linkMail.Size = new System.Drawing.Size(159, 15);
			this.linkMail.TabIndex = 1;
			this.linkMail.TabStop = true;
			this.linkMail.Text = ":about/label/mail-address";
			this.linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// commandUpdate
			// 
			this.commandUpdate.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandUpdate.Location = new System.Drawing.Point(312, 3);
			this.commandUpdate.Name = "commandUpdate";
			this.commandUpdate.Size = new System.Drawing.Size(97, 31);
			this.commandUpdate.TabIndex = 7;
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
            this.gridComponents_columnType,
            this.gridComponents_columnLicense});
			this.gridComponents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridComponents.Location = new System.Drawing.Point(3, 3);
			this.gridComponents.MultiSelect = false;
			this.gridComponents.Name = "gridComponents";
			this.gridComponents.ReadOnly = true;
			this.gridComponents.RowTemplate.Height = 21;
			this.gridComponents.Size = new System.Drawing.Size(616, 187);
			this.gridComponents.TabIndex = 3;
			this.gridComponents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridComponents_CellContentClick);
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
			// gridComponents_columnLicense
			// 
			this.gridComponents_columnLicense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.gridComponents_columnLicense.HeaderText = ":about/column/license";
			this.gridComponents_columnLicense.Name = "gridComponents_columnLicense";
			this.gridComponents_columnLicense.ReadOnly = true;
			this.gridComponents_columnLicense.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// commandChangelog
			// 
			this.commandChangelog.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandChangelog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.commandChangelog.Location = new System.Drawing.Point(209, 3);
			this.commandChangelog.Name = "commandChangelog";
			this.commandChangelog.Size = new System.Drawing.Size(97, 31);
			this.commandChangelog.TabIndex = 6;
			this.commandChangelog.Text = ":about/command/changelog";
			this.commandChangelog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandChangelog.UseVisualStyleBackColor = true;
			this.commandChangelog.Click += new System.EventHandler(this.CommandChangelog_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.imageIcon, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelAppName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelAppVersion, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelConfiguration, 1, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(177, 77);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// linkDiscussion
			// 
			this.linkDiscussion.AutoEllipsis = true;
			this.linkDiscussion.AutoSize = true;
			this.linkDiscussion.Location = new System.Drawing.Point(3, 57);
			this.linkDiscussion.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
			this.linkDiscussion.Name = "linkDiscussion";
			this.linkDiscussion.Size = new System.Drawing.Size(175, 15);
			this.linkDiscussion.TabIndex = 3;
			this.linkDiscussion.TabStop = true;
			this.linkDiscussion.Text = ":about/label/page-discussion";
			this.linkDiscussion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// panelWeb
			// 
			this.panelWeb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelWeb.AutoScroll = true;
			this.panelWeb.Controls.Add(this.linkAbout);
			this.panelWeb.Controls.Add(this.linkMail);
			this.panelWeb.Controls.Add(this.linkDevelopment);
			this.panelWeb.Controls.Add(this.linkDiscussion);
			this.panelWeb.Controls.Add(this.linkFeedback);
			this.panelWeb.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.panelWeb.Location = new System.Drawing.Point(186, 3);
			this.panelWeb.Margin = new System.Windows.Forms.Padding(3, 3, 3, 4);
			this.panelWeb.Name = "panelWeb";
			this.panelWeb.Size = new System.Drawing.Size(320, 120);
			this.panelWeb.TabIndex = 1;
			// 
			// linkFeedback
			// 
			this.linkFeedback.AutoEllipsis = true;
			this.linkFeedback.AutoSize = true;
			this.linkFeedback.Location = new System.Drawing.Point(3, 76);
			this.linkFeedback.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
			this.linkFeedback.Name = "linkFeedback";
			this.linkFeedback.Size = new System.Drawing.Size(169, 15);
			this.linkFeedback.TabIndex = 4;
			this.linkFeedback.TabStop = true;
			this.linkFeedback.Text = ":about/label/page-feedback";
			this.linkFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// panelEnv
			// 
			this.panelEnv.AutoSize = true;
			this.panelEnv.Controls.Add(this.labelUserenv);
			this.panelEnv.Controls.Add(this.linkCopyShort);
			this.panelEnv.Controls.Add(this.linkCopyLong);
			this.panelEnv.Location = new System.Drawing.Point(3, 130);
			this.panelEnv.Name = "panelEnv";
			this.panelEnv.Size = new System.Drawing.Size(417, 15);
			this.panelEnv.TabIndex = 2;
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
			// tabAbout
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.tabAbout, 2);
			this.tabAbout.Controls.Add(this.tabAbout_pageApp);
			this.tabAbout.Controls.Add(this.tabAbout_pageComponent);
			this.tabAbout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabAbout.Location = new System.Drawing.Point(3, 3);
			this.tabAbout.Name = "tabAbout";
			this.tabAbout.SelectedIndex = 0;
			this.tabAbout.Size = new System.Drawing.Size(630, 221);
			this.tabAbout.TabIndex = 9;
			// 
			// tabAbout_pageApp
			// 
			this.tabAbout_pageApp.Controls.Add(this.panelApp);
			this.tabAbout_pageApp.Location = new System.Drawing.Point(4, 24);
			this.tabAbout_pageApp.Name = "tabAbout_pageApp";
			this.tabAbout_pageApp.Padding = new System.Windows.Forms.Padding(3);
			this.tabAbout_pageApp.Size = new System.Drawing.Size(622, 193);
			this.tabAbout_pageApp.TabIndex = 0;
			this.tabAbout_pageApp.Text = ":about/page/app";
			this.tabAbout_pageApp.UseVisualStyleBackColor = true;
			// 
			// panelApp
			// 
			this.panelApp.AutoScroll = true;
			this.panelApp.Controls.Add(this.tableLayoutPanel1);
			this.panelApp.Controls.Add(this.panelWeb);
			this.panelApp.Controls.Add(this.panelEnv);
			this.panelApp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelApp.Location = new System.Drawing.Point(3, 3);
			this.panelApp.Name = "panelApp";
			this.panelApp.Size = new System.Drawing.Size(616, 187);
			this.panelApp.TabIndex = 0;
			// 
			// tabAbout_pageComponent
			// 
			this.tabAbout_pageComponent.Controls.Add(this.gridComponents);
			this.tabAbout_pageComponent.Location = new System.Drawing.Point(4, 24);
			this.tabAbout_pageComponent.Name = "tabAbout_pageComponent";
			this.tabAbout_pageComponent.Padding = new System.Windows.Forms.Padding(3);
			this.tabAbout_pageComponent.Size = new System.Drawing.Size(622, 193);
			this.tabAbout_pageComponent.TabIndex = 1;
			this.tabAbout_pageComponent.Text = ":about/page/component";
			this.tabAbout_pageComponent.UseVisualStyleBackColor = true;
			// 
			// panelCommand
			// 
			this.panelCommand.AutoSize = true;
			this.panelCommand.Controls.Add(this.commandExecuteDir);
			this.panelCommand.Controls.Add(this.commandDataDir);
			this.panelCommand.Controls.Add(this.commandChangelog);
			this.panelCommand.Controls.Add(this.commandUpdate);
			this.panelCommand.Location = new System.Drawing.Point(3, 230);
			this.panelCommand.Name = "panelCommand";
			this.panelCommand.Size = new System.Drawing.Size(412, 37);
			this.panelCommand.TabIndex = 12;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.tabAbout, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.commandOk, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.panelCommand, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(636, 270);
			this.tableLayoutPanel2.TabIndex = 10;
			// 
			// AboutForm
			// 
			this.AcceptButton = this.commandOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(636, 270);
			this.Controls.Add(this.tableLayoutPanel2);
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
			this.tabAbout.ResumeLayout(false);
			this.tabAbout_pageApp.ResumeLayout(false);
			this.panelApp.ResumeLayout(false);
			this.panelApp.PerformLayout();
			this.tabAbout_pageComponent.ResumeLayout(false);
			this.panelCommand.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
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
		private System.Windows.Forms.DataGridViewTextBoxColumn gridComponents_columnLicense;
		private System.Windows.Forms.TabControl tabAbout;
		private System.Windows.Forms.TabPage tabAbout_pageApp;
		private System.Windows.Forms.TabPage tabAbout_pageComponent;
		private System.Windows.Forms.FlowLayoutPanel panelApp;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.FlowLayoutPanel panelCommand;
		private System.Windows.Forms.LinkLabel linkFeedback;
	}
}
