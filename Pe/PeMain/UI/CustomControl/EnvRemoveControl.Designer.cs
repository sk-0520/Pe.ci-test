/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 22:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace ContentTypeTextNet.Pe.PeMain.UI.CustomControl
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
			this.inputEnv = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// inputEnv
			// 
			this.inputEnv.AcceptsReturn = true;
			this.inputEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputEnv.Location = new System.Drawing.Point(0, 0);
			this.inputEnv.Multiline = true;
			this.inputEnv.Name = "inputEnv";
			this.inputEnv.Size = new System.Drawing.Size(150, 150);
			this.inputEnv.TabIndex = 0;
			this.inputEnv.TextChanged += new System.EventHandler(this.InputEnv_TextChanged);
			// 
			// EnvRemoveControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.inputEnv);
			this.Name = "EnvRemoveControl";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox inputEnv;
	}
}
