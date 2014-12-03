/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 13:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace ContentTypeTextNet.Pe.Library.Utility
{
	partial class AppbarForm
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
			this.timerAutoHidden = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// timerAutoHidden
			// 
			this.timerAutoHidden.Interval = 1000;
			this.timerAutoHidden.Tick += new System.EventHandler(this.TimerAutoHide_Tick);
			// 
			// AppbarForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.ControlBox = false;
			this.Name = "AppbarForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "AppbarForm";
			this.VisibleChanged += new System.EventHandler(this.AppbarFormVisibleChanged);
			this.MouseEnter += new System.EventHandler(this.AppbarForm_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.AppbarForm_MouseLeave);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Timer timerAutoHidden;
	}
}
