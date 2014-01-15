/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 22:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class EnvRemoveControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
			this.viewEnv = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// viewEnv
			// 
			this.viewEnv.AcceptsReturn = true;
			this.viewEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewEnv.Location = new System.Drawing.Point(0, 0);
			this.viewEnv.Multiline = true;
			this.viewEnv.Name = "viewEnv";
			this.viewEnv.Size = new System.Drawing.Size(150, 150);
			this.viewEnv.TabIndex = 0;
			// 
			// EnvRemoveControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.viewEnv);
			this.Name = "EnvRemoveControl";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox viewEnv;
	}
}
