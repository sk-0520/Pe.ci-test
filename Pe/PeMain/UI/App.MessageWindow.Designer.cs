/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/29
 * 時刻: 22:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class App
	{
		partial class MessageWindow
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
				NativeMethods.ChangeClipboardChain(Handle, NextWndHandle);
				base.Dispose(disposing);
			}
			
			/// <summary>
			/// This method is required for Windows Forms designer support.
			/// Do not change the method contents inside the source code editor. The Forms designer might
			/// not be able to load this method if it was changed manually.
			/// </summary>
			private void InitializeComponent()
			{
				// 
				// Pe_MessageWindow
				// 
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.Name = "Pe_MessageWindow";
			}
		}
	}
}
