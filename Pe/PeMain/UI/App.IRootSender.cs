/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/16
 * 時刻: 20:56
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.PeMain.Logic;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class App
	{
		public void ShowBalloon(ToolTipIcon icon, string title, string message)
		{
			this._notifyIcon.ShowBalloonTip(0, title, message, icon);
		}
		
		public void ChangeLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem)
		{
			foreach(var toolbar in this._toolbarForms.Values.Where(t => t.UseToolbarItem != toolbarItem)) {
				toolbar.ReceiveChangedLauncherItems(toolbarItem, toolbarGroupItem);
			}
		}

		public void AppendWindow(Form window)
		{
			this._otherWindows.Add(window);
			window.FormClosed += window_FormClosed;
		}

		public void ReceiveDeviceChanged(ChangeDevice changeDevice)
		{
			//this._commonData.Logger.Puts(LogType.Warning, "ReceiveDeviceChanged", changeDevice);
			// デバイス状態が変更されたか
			if(changeDevice.DBT == DBT.DBT_DEVNODES_CHANGED && Initialized && !this._pause) {
				// デバイス変更前のスクリーン数が異なっていればディスプレイの抜き差しが行われたと判定する
				// 現在生成されているツールバーの数が前回ディスプレイ数となる
				
				// 変更通知から現在数をAPIでまともに取得する
				var rawScreenCount = NativeMethods.GetSystemMetrics(SM.SM_CMONITORS);
				bool changedScreenCount = this._toolbarForms.Count != rawScreenCount;
				//bool isTimeout = false;
				Task.Factory.StartNew(
					() => {
						const int waitMax = Literal.waitCountForGetScreenCount;
						int waitCount = 0;
						
						var managedScreenCount = Screen.AllScreens.Count();
						while(rawScreenCount != managedScreenCount) {
							//Debug.WriteLine("waitCount" + waitCount);
							if(waitMax < ++waitCount) {
								// タイムアウト
								//isTimeout = true;
								break;
							}
							Thread.Sleep(Literal.screenCountWaitTime);
							managedScreenCount = Screen.AllScreens.Count();
						}
					}
				).ContinueWith(
					t => {
						if(changedScreenCount) {
							ChangedScreenCount();
						}
					},
					TaskScheduler.FromCurrentSynchronizationContext()
				);
			}
		}

		public void ChangeClipboard()
		{
			if(!this._commonData.MainSetting.Clipboard.Enabled) {
				return;
			}

			var seq = NativeMethods.GetClipboardSequenceNumber();
			//Debug.WriteLine("{0} -> {1} - {2}", this._commonData.MainSetting.Clipboard.DisabledCopy, seq, _clipboardPrevSeq);
			if(this._commonData.MainSetting.Clipboard.DisabledCopy || seq == this._clipboardPrevSeq) {
				return;
			}
			this._clipboardPrevSeq = seq;

			var nowTime = DateTime.Now;
			if(nowTime - this._clipboardPrevTime <= Literal.clipboardWaitTime.median) {
				var map = new Dictionary<string,string>() {
					{ AppLanguageName.clipboardPrevTime, this._clipboardPrevTime.ToString() },
				};
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["clipboard/wait/title"], this._commonData.Language["clipboard/wait/message", map]);
				return;
			}

			var clipboardItem = new ClipboardItem();
			if(!this._commonData.MainSetting.Clipboard.DisabledCopy && clipboardItem.SetClipboardData()) {
				this._clipboardPrevTime = nowTime;
				var displayText = LanguageUtility.ClipboardItemToDisplayText(this._commonData.Language, clipboardItem);
				clipboardItem.Name = displayText;

				this._commonData.MainSetting.Clipboard.Items.Insert(0, clipboardItem);
			}
		}
	}
}
