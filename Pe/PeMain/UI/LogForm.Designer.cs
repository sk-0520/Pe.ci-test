/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/10/2014
 * 時刻: 23:49
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class LogForm
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
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.statusLog = new System.Windows.Forms.StatusStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listLog = new System.Windows.Forms.ListView();
			this.listLog_columnTimestamp = new System.Windows.Forms.ColumnHeader();
			this.listLog_columnTitle = new System.Windows.Forms.ColumnHeader();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.viewDetail = new System.Windows.Forms.TextBox();
			this.listStack = new System.Windows.Forms.ListView();
			this.listStack_columnFunction = new System.Windows.Forms.ColumnHeader();
			this.listStack_columnLine = new System.Windows.Forms.ColumnHeader();
			this.listStack_columnFile = new System.Windows.Forms.ColumnHeader();
			this.toolLog = new System.Windows.Forms.ToolStrip();
			this.toolLog_save = new System.Windows.Forms.ToolStripButton();
			this.toolLog_clear = new System.Windows.Forms.ToolStripButton();
			this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.BottomToolStripPanel
			// 
			this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusLog);
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
			this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(436, 229);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(436, 276);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolLog);
			// 
			// statusLog
			// 
			this.statusLog.Dock = System.Windows.Forms.DockStyle.None;
			this.statusLog.Location = new System.Drawing.Point(0, 0);
			this.statusLog.Name = "statusLog";
			this.statusLog.Size = new System.Drawing.Size(436, 22);
			this.statusLog.TabIndex = 0;
			this.statusLog.Text = "statusStrip1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listLog);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(436, 229);
			this.splitContainer1.SplitterDistance = 135;
			this.splitContainer1.TabIndex = 0;
			// 
			// listLog
			// 
			this.listLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.listLog_columnTimestamp,
									this.listLog_columnTitle});
			this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listLog.FullRowSelect = true;
			this.listLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listLog.Location = new System.Drawing.Point(0, 0);
			this.listLog.MultiSelect = false;
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(436, 135);
			this.listLog.TabIndex = 0;
			this.listLog.UseCompatibleStateImageBehavior = false;
			this.listLog.View = System.Windows.Forms.View.Details;
			this.listLog.VirtualMode = true;
			this.listLog.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListLog_RetrieveVirtualItem);
			this.listLog.SelectedIndexChanged += new System.EventHandler(this.ListLog_SelectedIndexChanged);
			// 
			// listLog_columnTimestamp
			// 
			this.listLog_columnTimestamp.Text = ":log/header/timestamp";
			// 
			// listLog_columnTitle
			// 
			this.listLog_columnTitle.Text = ":log/header/title";
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.viewDetail);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.listStack);
			this.splitContainer2.Size = new System.Drawing.Size(436, 90);
			this.splitContainer2.SplitterDistance = 196;
			this.splitContainer2.TabIndex = 0;
			// 
			// viewDetail
			// 
			this.viewDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewDetail.Location = new System.Drawing.Point(0, 0);
			this.viewDetail.Multiline = true;
			this.viewDetail.Name = "viewDetail";
			this.viewDetail.ReadOnly = true;
			this.viewDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.viewDetail.Size = new System.Drawing.Size(196, 90);
			this.viewDetail.TabIndex = 1;
			this.viewDetail.WordWrap = false;
			// 
			// listStack
			// 
			this.listStack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.listStack_columnFunction,
									this.listStack_columnLine,
									this.listStack_columnFile});
			this.listStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listStack.FullRowSelect = true;
			this.listStack.GridLines = true;
			this.listStack.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listStack.Location = new System.Drawing.Point(0, 0);
			this.listStack.MultiSelect = false;
			this.listStack.Name = "listStack";
			this.listStack.ShowGroups = false;
			this.listStack.Size = new System.Drawing.Size(236, 90);
			this.listStack.TabIndex = 0;
			this.listStack.UseCompatibleStateImageBehavior = false;
			this.listStack.View = System.Windows.Forms.View.Details;
			// 
			// listStack_columnFunction
			// 
			this.listStack_columnFunction.Text = ":log/header/method";
			// 
			// listStack_columnLine
			// 
			this.listStack_columnLine.Text = ":log/header/file";
			// 
			// listStack_columnFile
			// 
			this.listStack_columnFile.Text = ":log/header/title";
			// 
			// toolLog
			// 
			this.toolLog.Dock = System.Windows.Forms.DockStyle.None;
			this.toolLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolLog_save,
									this.toolLog_clear});
			this.toolLog.Location = new System.Drawing.Point(0, 0);
			this.toolLog.Name = "toolLog";
			this.toolLog.Size = new System.Drawing.Size(436, 25);
			this.toolLog.Stretch = true;
			this.toolLog.TabIndex = 0;
			// 
			// toolLog_save
			// 
			this.toolLog_save.Image = global::PeMain.Properties.Images.Save;
			this.toolLog_save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLog_save.Name = "toolLog_save";
			this.toolLog_save.Size = new System.Drawing.Size(147, 22);
			this.toolLog_save.Text = ":log/command/save";
			this.toolLog_save.ToolTipText = ":log/tips/save";
			// 
			// toolLog_clear
			// 
			this.toolLog_clear.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLog_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLog_clear.Name = "toolLog_clear";
			this.toolLog_clear.Size = new System.Drawing.Size(148, 22);
			this.toolLog_clear.Text = ":log/command/clear";
			this.toolLog_clear.ToolTipText = ":log/tips/clear";
			// 
			// LogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 276);
			this.Controls.Add(this.toolStripContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LogForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = ":window/log";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
			this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.toolLog.ResumeLayout(false);
			this.toolLog.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TextBox viewDetail;
		private System.Windows.Forms.ColumnHeader listStack_columnFunction;
		private System.Windows.Forms.ColumnHeader listStack_columnLine;
		private System.Windows.Forms.ColumnHeader listStack_columnFile;
		private System.Windows.Forms.ListView listStack;
		private System.Windows.Forms.ColumnHeader listLog_columnTitle;
		private System.Windows.Forms.ColumnHeader listLog_columnTimestamp;
		private System.Windows.Forms.ListView listLog;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripButton toolLog_clear;
		private System.Windows.Forms.ToolStripButton toolLog_save;
		private System.Windows.Forms.ToolStrip toolLog;
		private System.Windows.Forms.StatusStrip statusLog;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
	}
}
