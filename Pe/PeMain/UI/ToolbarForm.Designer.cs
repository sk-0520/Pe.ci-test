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
			this.menuGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolLauncher = new System.Windows.Forms.ToolBar();
			this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
			this.SuspendLayout();
			// 
			// menuGroup
			// 
			this.menuGroup.Name = "menuGroup";
			this.menuGroup.Size = new System.Drawing.Size(61, 4);
			// 
			// toolLauncher
			// 
			this.toolLauncher.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolLauncher.AutoSize = false;
			this.toolLauncher.Divider = false;
			this.toolLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolLauncher.DropDownArrows = true;
			this.toolLauncher.Location = new System.Drawing.Point(2, 2);
			this.toolLauncher.Margin = new System.Windows.Forms.Padding(0);
			this.toolLauncher.Name = "toolLauncher";
			this.toolLauncher.ShowToolTips = true;
			this.toolLauncher.Size = new System.Drawing.Size(176, 74);
			this.toolLauncher.TabIndex = 1;
			this.toolLauncher.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			// 
			// maskedTextBox1
			// 
			this.maskedTextBox1.Location = new System.Drawing.Point(0, 0);
			this.maskedTextBox1.Name = "maskedTextBox1";
			this.maskedTextBox1.Size = new System.Drawing.Size(100, 19);
			this.maskedTextBox1.TabIndex = 0;
			// 
			// ToolbarForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(180, 78);
			this.ContextMenuStrip = this.menuGroup;
			this.Controls.Add(this.toolLauncher);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ToolbarForm";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.Text = "ToolbarForm";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.MaskedTextBox maskedTextBox1;
		private System.Windows.Forms.ToolBar toolLauncher;
		private System.Windows.Forms.ContextMenuStrip menuGroup;
	}
}
