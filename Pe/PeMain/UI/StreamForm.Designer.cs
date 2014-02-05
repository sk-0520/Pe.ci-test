/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamForm));
			this.toolStream = new System.Windows.Forms.ToolStrip();
			this.toolStream_save = new System.Windows.Forms.ToolStripButton();
			this.toolStream_clear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStream_refresh = new System.Windows.Forms.ToolStripButton();
			this.toolStream_kill = new System.Windows.Forms.ToolStripButton();
			this.tabStream = new System.Windows.Forms.TabControl();
			this.pageStream = new System.Windows.Forms.TabPage();
			this.viewOutput = new System.Windows.Forms.TextBox();
			this.pageProcess = new System.Windows.Forms.TabPage();
			this.propertyProcess = new System.Windows.Forms.PropertyGrid();
			this.pageProperty = new System.Windows.Forms.TabPage();
			this.propertyProperty = new System.Windows.Forms.PropertyGrid();
			this.toolStream.SuspendLayout();
			this.tabStream.SuspendLayout();
			this.pageStream.SuspendLayout();
			this.pageProcess.SuspendLayout();
			this.pageProperty.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStream
			// 
			this.toolStream.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStream.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStream_save,
									this.toolStream_clear,
									this.toolStripSeparator1,
									this.toolStream_refresh,
									this.toolStream_kill});
			this.toolStream.Location = new System.Drawing.Point(0, 0);
			this.toolStream.Name = "toolStream";
			this.toolStream.Size = new System.Drawing.Size(362, 25);
			this.toolStream.TabIndex = 1;
			this.toolStream.Text = "toolStrip1";
			// 
			// toolStream_save
			// 
			this.toolStream_save.Enabled = false;
			this.toolStream_save.Image = global::PeMain.Properties.Images.Save;
			this.toolStream_save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_save.Name = "toolStream_save";
			this.toolStream_save.Size = new System.Drawing.Size(129, 22);
			this.toolStream_save.Text = "{STREAM_SAVE}";
			// 
			// toolStream_clear
			// 
			this.toolStream_clear.Enabled = false;
			this.toolStream_clear.Image = ((System.Drawing.Image)(resources.GetObject("toolStream_clear.Image")));
			this.toolStream_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_clear.Name = "toolStream_clear";
			this.toolStream_clear.Size = new System.Drawing.Size(136, 22);
			this.toolStream_clear.Text = "{STREAM_CLEAR}";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStream_refresh
			// 
			this.toolStream_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_refresh.Image = global::PeMain.Properties.Images.Refresh;
			this.toolStream_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_refresh.Name = "toolStream_refresh";
			this.toolStream_refresh.Size = new System.Drawing.Size(23, 22);
			this.toolStream_refresh.Text = "{REFRESH}";
			this.toolStream_refresh.Click += new System.EventHandler(this.ToolStream_refresh_Click);
			// 
			// toolStream_kill
			// 
			this.toolStream_kill.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStream_kill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_kill.Image = global::PeMain.Properties.Images.Kill;
			this.toolStream_kill.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_kill.Name = "toolStream_kill";
			this.toolStream_kill.Size = new System.Drawing.Size(23, 22);
			this.toolStream_kill.Text = "{KILL}";
			this.toolStream_kill.Click += new System.EventHandler(this.ToolStream_kill_Click);
			// 
			// tabStream
			// 
			this.tabStream.Controls.Add(this.pageStream);
			this.tabStream.Controls.Add(this.pageProcess);
			this.tabStream.Controls.Add(this.pageProperty);
			this.tabStream.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabStream.Location = new System.Drawing.Point(0, 25);
			this.tabStream.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabStream.Name = "tabStream";
			this.tabStream.SelectedIndex = 0;
			this.tabStream.Size = new System.Drawing.Size(362, 239);
			this.tabStream.TabIndex = 2;
			// 
			// pageStream
			// 
			this.pageStream.Controls.Add(this.viewOutput);
			this.pageStream.Location = new System.Drawing.Point(4, 24);
			this.pageStream.Name = "pageStream";
			this.pageStream.Size = new System.Drawing.Size(354, 211);
			this.pageStream.TabIndex = 0;
			this.pageStream.Text = "{STREAM}";
			this.pageStream.UseVisualStyleBackColor = true;
			// 
			// viewOutput
			// 
			this.viewOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.viewOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewOutput.Location = new System.Drawing.Point(0, 0);
			this.viewOutput.Multiline = true;
			this.viewOutput.Name = "viewOutput";
			this.viewOutput.ReadOnly = true;
			this.viewOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.viewOutput.Size = new System.Drawing.Size(354, 211);
			this.viewOutput.TabIndex = 0;
			this.viewOutput.WordWrap = false;
			this.viewOutput.TextChanged += new System.EventHandler(this.ViewOutput_TextChanged);
			// 
			// pageProcess
			// 
			this.pageProcess.Controls.Add(this.propertyProcess);
			this.pageProcess.Location = new System.Drawing.Point(4, 24);
			this.pageProcess.Name = "pageProcess";
			this.pageProcess.Size = new System.Drawing.Size(354, 211);
			this.pageProcess.TabIndex = 1;
			this.pageProcess.Text = "{PROCESS}";
			this.pageProcess.UseVisualStyleBackColor = true;
			// 
			// propertyProcess
			// 
			this.propertyProcess.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProcess.Location = new System.Drawing.Point(0, 0);
			this.propertyProcess.Name = "propertyProcess";
			this.propertyProcess.Size = new System.Drawing.Size(354, 211);
			this.propertyProcess.TabIndex = 0;
			// 
			// pageProperty
			// 
			this.pageProperty.Controls.Add(this.propertyProperty);
			this.pageProperty.Location = new System.Drawing.Point(4, 24);
			this.pageProperty.Name = "pageProperty";
			this.pageProperty.Size = new System.Drawing.Size(354, 211);
			this.pageProperty.TabIndex = 2;
			this.pageProperty.Text = "{PROPERTY}";
			this.pageProperty.UseVisualStyleBackColor = true;
			// 
			// propertyProperty
			// 
			this.propertyProperty.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProperty.Location = new System.Drawing.Point(0, 0);
			this.propertyProperty.Name = "propertyProperty";
			this.propertyProperty.Size = new System.Drawing.Size(354, 211);
			this.propertyProperty.TabIndex = 1;
			// 
			// StreamForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(362, 264);
			this.Controls.Add(this.tabStream);
			this.Controls.Add(this.toolStream);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "StreamForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "{ITEM@STREAM}";
			this.toolStream.ResumeLayout(false);
			this.toolStream.PerformLayout();
			this.tabStream.ResumeLayout(false);
			this.pageStream.ResumeLayout(false);
			this.pageStream.PerformLayout();
			this.pageProcess.ResumeLayout(false);
			this.pageProperty.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripButton toolStream_kill;
		private System.Windows.Forms.ToolStripButton toolStream_refresh;
		private System.Windows.Forms.PropertyGrid propertyProperty;
		private System.Windows.Forms.TabPage pageProperty;
		private System.Windows.Forms.PropertyGrid propertyProcess;
		private System.Windows.Forms.TextBox viewOutput;
		private System.Windows.Forms.ToolStripButton toolStream_clear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.TabPage pageProcess;
		private System.Windows.Forms.TabPage pageStream;
		private System.Windows.Forms.TabControl tabStream;
		private System.Windows.Forms.ToolStripButton toolStream_save;
		private System.Windows.Forms.ToolStrip toolStream;
	}
}
