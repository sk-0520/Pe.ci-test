/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:15
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class ToolbarForm
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
			this.components = new System.ComponentModel.Container();
			this.toolLauncher = new PeMain.UI.ToolbarToolStrip();
			this.tipsLauncher = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// toolLauncher
			// 
			this.toolLauncher.AllowDrop = true;
			this.toolLauncher.AutoSize = false;
			this.toolLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolLauncher.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolLauncher.Location = new System.Drawing.Point(0, 0);
			this.toolLauncher.Name = "toolLauncher";
			this.toolLauncher.Padding = new System.Windows.Forms.Padding(0);
			this.toolLauncher.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolLauncher.ShowItemToolTips = false;
			this.toolLauncher.Size = new System.Drawing.Size(180, 19);
			this.toolLauncher.TabIndex = 1;
			this.toolLauncher.Text = "toolStrip1";
			this.toolLauncher.DragDrop += new System.Windows.Forms.DragEventHandler(this.ToolLauncherDragDrop);
			this.toolLauncher.DragEnter += new System.Windows.Forms.DragEventHandler(this.ToolLauncherDragEnter);
			this.toolLauncher.DragOver += new System.Windows.Forms.DragEventHandler(this.ToolLauncherDragOver);
			this.toolLauncher.MouseLeave += new System.EventHandler(this.ToolLauncher_MouseLeave);
			this.toolLauncher.MouseHover += new System.EventHandler(this.ToolLauncher_MouseHover);
			// 
			// ToolbarForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(180, 19);
			this.Controls.Add(this.toolLauncher);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ToolbarForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "ToolbarForm";
			this.Activated += new System.EventHandler(this.ToolbarForm_Activated);
			this.Deactivate += new System.EventHandler(this.ToolbarForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolbarFormFormClosing);
			this.Shown += new System.EventHandler(this.ToolbarFormShown);
			this.LocationChanged += new System.EventHandler(this.ToolbarForm_LocationChanged);
			this.SizeChanged += new System.EventHandler(this.ToolbarForm_SizeChanged);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolbarForm_Paint);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolTip tipsLauncher;
		private PeMain.UI.ToolbarToolStrip toolLauncher;
	}
}
