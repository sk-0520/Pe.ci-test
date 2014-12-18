/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class StreamForm
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
			this.toolStream = new System.Windows.Forms.ToolStrip();
			this.toolStream_itemTopmost = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStream_itemSave = new System.Windows.Forms.ToolStripButton();
			this.toolStream_itemClear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStream_itemRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStream_itemKill = new System.Windows.Forms.ToolStripButton();
			this.tabStream = new System.Windows.Forms.TabControl();
			this.tabStream_pageStream = new System.Windows.Forms.TabPage();
			this.inputOutput = new System.Windows.Forms.TextBox();
			this.tabStream_pageProcess = new System.Windows.Forms.TabPage();
			this.propertyProcess = new System.Windows.Forms.PropertyGrid();
			this.tabStream_pageProperty = new System.Windows.Forms.TabPage();
			this.propertyProperty = new System.Windows.Forms.PropertyGrid();
			this.toolStream.SuspendLayout();
			this.tabStream.SuspendLayout();
			this.tabStream_pageStream.SuspendLayout();
			this.tabStream_pageProcess.SuspendLayout();
			this.tabStream_pageProperty.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStream
			// 
			this.toolStream.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStream.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStream_itemTopmost,
									this.toolStripSeparator2,
									this.toolStream_itemSave,
									this.toolStream_itemClear,
									this.toolStripSeparator1,
									this.toolStream_itemRefresh,
									this.toolStream_itemKill});
			this.toolStream.Location = new System.Drawing.Point(0, 0);
			this.toolStream.Name = "toolStream";
			this.toolStream.Size = new System.Drawing.Size(471, 25);
			this.toolStream.TabIndex = 1;
			this.toolStream.Text = "toolStrip1";
			// 
			// toolStream_itemTopmost
			// 
			this.toolStream_itemTopmost.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_itemTopmost.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Pin;
			this.toolStream_itemTopmost.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_itemTopmost.Name = "toolStream_itemTopmost";
			this.toolStream_itemTopmost.Size = new System.Drawing.Size(23, 22);
			this.toolStream_itemTopmost.Text = ":stream/command/topmost";
			this.toolStream_itemTopmost.Click += new System.EventHandler(this.ToolStream_itemTopmost_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStream_itemSave
			// 
			this.toolStream_itemSave.Enabled = false;
			this.toolStream_itemSave.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Save;
			this.toolStream_itemSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_itemSave.Name = "toolStream_itemSave";
			this.toolStream_itemSave.Size = new System.Drawing.Size(172, 22);
			this.toolStream_itemSave.Text = ":stream/command/save";
			this.toolStream_itemSave.ToolTipText = ":stream/tips/save";
			this.toolStream_itemSave.Click += new System.EventHandler(this.ToolStream_save_Click);
			// 
			// toolStream_itemClear
			// 
			this.toolStream_itemClear.Enabled = false;
			this.toolStream_itemClear.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Clear;
			this.toolStream_itemClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_itemClear.Name = "toolStream_itemClear";
			this.toolStream_itemClear.Size = new System.Drawing.Size(173, 22);
			this.toolStream_itemClear.Text = ":stream/command/clear";
			this.toolStream_itemClear.ToolTipText = ":stream/tips/clear";
			this.toolStream_itemClear.Click += new System.EventHandler(this.ToolStream_clear_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStream_itemRefresh
			// 
			this.toolStream_itemRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_itemRefresh.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Refresh;
			this.toolStream_itemRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_itemRefresh.Name = "toolStream_itemRefresh";
			this.toolStream_itemRefresh.Size = new System.Drawing.Size(23, 22);
			this.toolStream_itemRefresh.ToolTipText = ":stream/tips/refresh";
			this.toolStream_itemRefresh.Click += new System.EventHandler(this.ToolStream_refresh_Click);
			// 
			// toolStream_itemKill
			// 
			this.toolStream_itemKill.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStream_itemKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_itemKill.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Kill;
			this.toolStream_itemKill.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_itemKill.Name = "toolStream_itemKill";
			this.toolStream_itemKill.Size = new System.Drawing.Size(23, 22);
			this.toolStream_itemKill.ToolTipText = ":stream/tips/kill";
			this.toolStream_itemKill.Click += new System.EventHandler(this.ToolStream_kill_Click);
			// 
			// tabStream
			// 
			this.tabStream.Controls.Add(this.tabStream_pageStream);
			this.tabStream.Controls.Add(this.tabStream_pageProcess);
			this.tabStream.Controls.Add(this.tabStream_pageProperty);
			this.tabStream.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabStream.Location = new System.Drawing.Point(0, 25);
			this.tabStream.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabStream.Name = "tabStream";
			this.tabStream.SelectedIndex = 0;
			this.tabStream.Size = new System.Drawing.Size(471, 255);
			this.tabStream.TabIndex = 2;
			// 
			// tabStream_pageStream
			// 
			this.tabStream_pageStream.Controls.Add(this.inputOutput);
			this.tabStream_pageStream.Location = new System.Drawing.Point(4, 24);
			this.tabStream_pageStream.Name = "tabStream_pageStream";
			this.tabStream_pageStream.Size = new System.Drawing.Size(463, 227);
			this.tabStream_pageStream.TabIndex = 0;
			this.tabStream_pageStream.Text = ":stream/page/stream";
			this.tabStream_pageStream.UseVisualStyleBackColor = true;
			// 
			// inputOutput
			// 
			this.inputOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.inputOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputOutput.Location = new System.Drawing.Point(0, 0);
			this.inputOutput.Multiline = true;
			this.inputOutput.Name = "inputOutput";
			this.inputOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.inputOutput.Size = new System.Drawing.Size(463, 227);
			this.inputOutput.TabIndex = 0;
			this.inputOutput.WordWrap = false;
			this.inputOutput.TextChanged += new System.EventHandler(this.ViewOutput_TextChanged);
			this.inputOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ViewOutput_KeyPress);
			// 
			// tabStream_pageProcess
			// 
			this.tabStream_pageProcess.Controls.Add(this.propertyProcess);
			this.tabStream_pageProcess.Location = new System.Drawing.Point(4, 24);
			this.tabStream_pageProcess.Name = "tabStream_pageProcess";
			this.tabStream_pageProcess.Size = new System.Drawing.Size(463, 227);
			this.tabStream_pageProcess.TabIndex = 1;
			this.tabStream_pageProcess.Text = ":stream/page/process";
			this.tabStream_pageProcess.UseVisualStyleBackColor = true;
			// 
			// propertyProcess
			// 
			this.propertyProcess.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProcess.Location = new System.Drawing.Point(0, 0);
			this.propertyProcess.Name = "propertyProcess";
			this.propertyProcess.Size = new System.Drawing.Size(463, 227);
			this.propertyProcess.TabIndex = 0;
			// 
			// tabStream_pageProperty
			// 
			this.tabStream_pageProperty.Controls.Add(this.propertyProperty);
			this.tabStream_pageProperty.Location = new System.Drawing.Point(4, 24);
			this.tabStream_pageProperty.Name = "tabStream_pageProperty";
			this.tabStream_pageProperty.Size = new System.Drawing.Size(463, 227);
			this.tabStream_pageProperty.TabIndex = 2;
			this.tabStream_pageProperty.Text = ":stream/page/property";
			this.tabStream_pageProperty.UseVisualStyleBackColor = true;
			// 
			// propertyProperty
			// 
			this.propertyProperty.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProperty.Location = new System.Drawing.Point(0, 0);
			this.propertyProperty.Name = "propertyProperty";
			this.propertyProperty.Size = new System.Drawing.Size(463, 227);
			this.propertyProperty.TabIndex = 1;
			// 
			// StreamForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(471, 280);
			this.Controls.Add(this.tabStream);
			this.Controls.Add(this.toolStream);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "StreamForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/stream";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StreamForm_FormClosing);
			this.Shown += new System.EventHandler(this.StreamForm_Shown);
			this.toolStream.ResumeLayout(false);
			this.toolStream.PerformLayout();
			this.tabStream.ResumeLayout(false);
			this.tabStream_pageStream.ResumeLayout(false);
			this.tabStream_pageStream.PerformLayout();
			this.tabStream_pageProcess.ResumeLayout(false);
			this.tabStream_pageProperty.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStream_itemTopmost;
		private System.Windows.Forms.ToolStripButton toolStream_itemKill;
		private System.Windows.Forms.ToolStripButton toolStream_itemRefresh;
		private System.Windows.Forms.PropertyGrid propertyProperty;
		private System.Windows.Forms.TabPage tabStream_pageProperty;
		private System.Windows.Forms.PropertyGrid propertyProcess;
		private System.Windows.Forms.TextBox inputOutput;
		private System.Windows.Forms.ToolStripButton toolStream_itemClear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.TabPage tabStream_pageProcess;
		private System.Windows.Forms.TabPage tabStream_pageStream;
		private System.Windows.Forms.TabControl tabStream;
		private System.Windows.Forms.ToolStripButton toolStream_itemSave;
		private System.Windows.Forms.ToolStrip toolStream;
	}
}
