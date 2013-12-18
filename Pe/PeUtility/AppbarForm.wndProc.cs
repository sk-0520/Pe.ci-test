/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:09
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using Windows;

namespace PeMain.UI
{
	public partial class AppbarForm
	{
		protected override void WndProc(ref Message m) {
			if(IsDocking) {
				if(m.Msg == (int)WM.WM_ACTIVATE) {
					var appBar = new APPBARDATA(Handle);
					Windows.API.SHAppBarMessage(ABM.ABM_ACTIVATE, ref appBar);
				} else if(m.Msg == (int)WM.WM_WINDOWPOSCHANGED) {
				var appBar = new APPBARDATA(Handle);
					Windows.API.SHAppBarMessage(ABM.ABM_WINDOWPOSCHANGED, ref appBar);
				}
				
				if(this.callbackMessage != 0 && m.Msg == this.callbackMessage) {
					//
					switch (m.WParam.ToInt32()) {
						case (int)ABN.ABN_FULLSCREENAPP:
							// フルスクリーン
							OnAppbarFullScreen(Convert.ToBoolean(m.LParam.ToInt32()));
							break;
							
						case (int)ABN.ABN_POSCHANGED:
							// 他のバーの位置が変更されたので再設定
							OnAppbarPosChanged(EventArgs.Empty);
							break;
							
						case (int)ABN.ABN_STATECHANGE:
							// タスクバーの [常に手前に表示] または [自動的に隠す] が変化したとき
							// 特に何もする必要なし
							OnAppbarStateChange(EventArgs.Empty);
							break;
					}
				}
			}
			
			base.WndProc(ref m);
		}
	}
}
