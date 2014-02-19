/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using PI.Windows;

namespace PeUtility
{
	/// <summary>
	/// Description of BaseToolbarForm_function.
	/// </summary>
	public partial class AppbarForm
	{
		protected override CreateParams CreateParams {
			get {
				const int WS_EX_TOOLWINDOW = 0x80;

				var p = base.CreateParams;
				// AppBar として表示するには WS_EX_TOOLWINDOW スタイルが必要
				p.ExStyle = p.ExStyle | WS_EX_TOOLWINDOW;

				return p;
			}
		}
		
		protected override void OnResizeEnd(EventArgs e)
		{
			if(IsDocking) {
				// AppBar のサイズを更新します。
				switch (DesktopDockType) {
					case DesktopDockType.Left:
					case DesktopDockType.Right:
						BarSize = new Size(Width, BarSize.Height);
						break;
					case DesktopDockType.Top:
					case DesktopDockType.Bottom:
						BarSize = new Size(BarSize.Width, Height);
						break;
				}
				Docking(DesktopDockType);
			}
			base.OnResizeEnd(e);
		}
		
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(IsDocking) {
				UnResistAppBar();
			}
			
			base.OnFormClosed(e);
		}
		
		
		private bool ResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			this.callbackMessage = API.RegisterWindowMessage(MessageString);
			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = this.callbackMessage;
			
			var registResult = API.SHAppBarMessage(ABM.ABM_NEW, ref appBar);
			IsDocking = registResult.ToInt32() != 0;
			
			return IsDocking;
		}
		
		private bool UnResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			var appBar = new APPBARDATA(Handle);

			var unregistResult = API.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
			
			IsDocking = false;
			this.callbackMessage = 0;
			
			return unregistResult.ToInt32() != 0;
		}
		
		private RECT CalcWantBarArea(DesktopDockType dockType)
		{
			Debug.Assert(dockType != DesktopDockType.None);
			
			var desktopArea = DockScreen.Bounds;
			var barArea = new RECT();
			
			// 設定値からバー領域取得
			if(dockType == DesktopDockType.Left || dockType == DesktopDockType.Right) {
				barArea.Top = desktopArea.Top;
				barArea.Bottom = desktopArea.Bottom;
				if(dockType == DesktopDockType.Left) {
					barArea.Left = desktopArea.Left;
					barArea.Right = desktopArea.Left + BarSize.Width;
				} else {
					barArea.Left = desktopArea.Right - BarSize.Width;
					barArea.Right = desktopArea.Right;
				}
			} else {
				Debug.Assert(dockType == DesktopDockType.Top || dockType == DesktopDockType.Bottom);
				barArea.Left = desktopArea.Left;
				barArea.Right = desktopArea.Right;
				if(dockType == DesktopDockType.Top) {
					barArea.Top = desktopArea.Top;
					barArea.Bottom = desktopArea.Top + BarSize.Height;
				} else {
					barArea.Top = desktopArea.Bottom - BarSize.Height;
					barArea.Bottom = desktopArea.Bottom;
				}
			}
			
			return barArea;
		}
		private void TuneSystemBarArea(ref APPBARDATA appBar)
		{
			// 現在の希望するサイズから実際のサイズ要求する
			API.SHAppBarMessage(ABM.ABM_QUERYPOS, ref appBar);
			switch(appBar.uEdge) {
				case ABE.ABE_LEFT:
					appBar.rc.Right = appBar.rc.Left + BarSize.Width;
					break;
					
				case ABE.ABE_RIGHT:
					appBar.rc.Left = appBar.rc.Right - BarSize.Width;
					break;
					
				case ABE.ABE_TOP:
					appBar.rc.Bottom = appBar.rc.Top + BarSize.Height;
					break;
					
				case ABE.ABE_BOTTOM:
					appBar.rc.Top = appBar.rc.Bottom - BarSize.Height;
					break;
					
				default:
					Debug.Assert(false, appBar.uEdge.ToString());
					break;
			}
		}
		
		private void DockingFromParameter(DesktopDockType dockType)
		{
			var appBar = new APPBARDATA(Handle);
			appBar.uEdge = dockType.ToABE();
			appBar.rc = CalcWantBarArea(dockType);
			TuneSystemBarArea(ref appBar);
			
			var appbarResult = API.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
			
			this.Bounds = new Rectangle(
				appBar.rc.Left,
				appBar.rc.Top,
				appBar.rc.Right - appBar.rc.Left,
				appBar.rc.Bottom - appBar.rc.Top
			);
			
		}
		public void DockingFromProperty()
		{
			DockingFromParameter(DesktopDockType);
		}
		/// <summary>
		/// ドッキングの実行
		/// 
		/// すでにドッキングされている場合はドッキングを再度実行する
		/// </summary>
		private void Docking(DesktopDockType dockType)
		{
			if(DesignMode) {
				return;
			}
			
			// 得録済みであればいったん解除
			if(IsDocking) {
				UnResistAppBar();
			}
			
			if(dockType == DesktopDockType.None) {
				return;
			}
			
			// 登録
			Debug.Assert(!IsDocking);
			ResistAppBar();
			
			DockingFromParameter(dockType);
		}
	}
}
