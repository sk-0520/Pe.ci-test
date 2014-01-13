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
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStream_clear = new System.Windows.Forms.ToolStripButton();
			this.tabStream = new System.Windows.Forms.TabControl();
			this.pageStream = new System.Windows.Forms.TabPage();
			this.viewOutput = new System.Windows.Forms.TextBox();
			this.pageProperty = new System.Windows.Forms.TabPage();
			this.propertyProcess = new System.Windows.Forms.PropertyGrid();
			this.toolStream.SuspendLayout();
			this.tabStream.SuspendLayout();
			this.pageStream.SuspendLayout();
			this.pageProperty.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStream
			// 
			this.toolStream.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStream.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStream_save,
									this.toolStripSeparator1,
									this.toolStream_clear});
			this.toolStream.Location = new System.Drawing.Point(0, 0);
			this.toolStream.Name = "toolStream";
			this.toolStream.Size = new System.Drawing.Size(331, 25);
			this.toolStream.TabIndex = 1;
			this.toolStream.Text = "toolStrip1";
			// 
			// toolStream_save
			// 
			this.toolStream_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_save.Image = global::PeMain.Properties.Images.Save;
			this.toolStream_save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_save.Name = "toolStream_save";
			this.toolStream_save.Size = new System.Drawing.Size(23, 22);
			this.toolStream_save.Text = "toolStripButton1";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStream_clear
			// 
			this.toolStream_clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStream_clear.Image = ((System.Drawing.Image)(resources.GetObject("toolStream_clear.Image")));
			this.toolStream_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStream_clear.Name = "toolStream_clear";
			this.toolStream_clear.Size = new System.Drawing.Size(23, 22);
			this.toolStream_clear.Text = "toolStripButton1";
			// 
			// tabStream
			// 
			this.tabStream.Controls.Add(this.pageStream);
			this.tabStream.Controls.Add(this.pageProperty);
			this.tabStream.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabStream.Location = new System.Drawing.Point(0, 25);
			this.tabStream.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabStream.Name = "tabStream";
			this.tabStream.SelectedIndex = 0;
			this.tabStream.Size = new System.Drawing.Size(331, 239);
			this.tabStream.TabIndex = 2;
			// 
			// pageStream
			// 
			this.pageStream.Controls.Add(this.viewOutput);
			this.pageStream.Location = new System.Drawing.Point(4, 24);
			this.pageStream.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageStream.Name = "pageStream";
			this.pageStream.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageStream.Size = new System.Drawing.Size(323, 211);
			this.pageStream.TabIndex = 0;
			this.pageStream.Text = "{STREAM}";
			this.pageStream.UseVisualStyleBackColor = true;
			// 
			// viewOutput
			// 
			this.viewOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.viewOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewOutput.Location = new System.Drawing.Point(3, 4);
			this.viewOutput.Multiline = true;
			this.viewOutput.Name = "viewOutput";
			this.viewOutput.ReadOnly = true;
			this.viewOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.viewOutput.Size = new System.Drawing.Size(317, 203);
			this.viewOutput.TabIndex = 0;
			this.viewOutput.WordWrap = false;
			// 
			// pageProperty
			// 
			this.pageProperty.Controls.Add(this.propertyProcess);
			this.pageProperty.Location = new System.Drawing.Point(4, 24);
			this.pageProperty.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageProperty.Name = "pageProperty";
			this.pageProperty.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageProperty.Size = new System.Drawing.Size(323, 211);
			this.pageProperty.TabIndex = 1;
			this.pageProperty.Text = "{PROPERTY}";
			this.pageProperty.UseVisualStyleBackColor = true;
			// 
			// propertyProcess
			// 
			this.propertyProcess.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyProcess.Location = new System.Drawing.Point(3, 4);
			this.propertyProcess.Name = "propertyProcess";
			this.propertyProcess.Size = new System.Drawing.Size(317, 203);
			this.propertyProcess.TabIndex = 0;
			// 
			// StreamForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 264);
			this.Controls.Add(this.tabStream);
			this.Controls.Add(this.toolStream);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "StreamForm";
			this.Text = "StreamForm";
			this.toolStream.ResumeLayout(false);
			this.toolStream.PerformLayout();
			this.tabStream.ResumeLayout(false);
			this.pageStream.ResumeLayout(false);
			this.pageStream.PerformLayout();
			this.pageProperty.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.PropertyGrid propertyProcess;
		private System.Windows.Forms.TextBox viewOutput;
		private System.Windows.Forms.ToolStripButton toolStream_clear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.TabPage pageProperty;
		private System.Windows.Forms.TabPage pageStream;
		private System.Windows.Forms.TabControl tabStream;
		private System.Windows.Forms.ToolStripButton toolStream_save;
		private System.Windows.Forms.ToolStrip toolStream;
	}
}
