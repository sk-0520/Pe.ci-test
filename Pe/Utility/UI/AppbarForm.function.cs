/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// Description of BaseToolbarForm_function.
	/// </summary>
	public partial class AppbarForm
	{
		private bool ResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			this.callbackMessage = NativeMethods.RegisterWindowMessage(MessageString);
			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = this.callbackMessage;
			
			var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);
			IsDocking = registResult.ToInt32() != 0;
			
			return IsDocking;
		}
		
		private bool UnResistAppBar()
		{
			Debug.Assert(!this.DesignMode);

			var appBar = new APPBARDATA(Handle);

			var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
			
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
			NativeMethods.SHAppBarMessage(ABM.ABM_QUERYPOS, ref appBar);
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
		
		public IntPtr ExistsHideWindow(DesktopDockType desktopDockType)
		{
			Debug.Assert(desktopDockType != DesktopDockType.None);
			
			var appBar = new APPBARDATA(Handle);
			appBar.uEdge = desktopDockType.ToABE();
			var nowWnd = NativeMethods.SHAppBarMessage(ABM.ABM_GETAUTOHIDEBAR, ref appBar);
			
			return nowWnd;
		}
		
		private void DockingFromParameter(DesktopDockType dockType, bool autoHide)
		{
			Debug.Assert(dockType != DesktopDockType.None);
			
			var appBar = new APPBARDATA(Handle);
			appBar.uEdge = dockType.ToABE();
			appBar.rc = CalcWantBarArea(dockType);
			TuneSystemBarArea(ref appBar);
			
			bool autoHideResult = false;
			if(autoHide) {
				var hideWnd = ExistsHideWindow(dockType);
				if(hideWnd.ToInt32() == 0 || hideWnd == Handle) {
				//if(hideWnd == null || hideWnd == Handle) {
					// 自動的に隠す
					var result = NativeMethods.SHAppBarMessage(ABM.ABM_SETAUTOHIDEBAR, ref appBar);
					autoHideResult = result.ToInt32() != 0;
				}
			}
			if(!autoHideResult) {
				var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
			}
			
			AutoHide = autoHideResult;
			
			Bounds = new Rectangle(
				appBar.rc.Left,
				appBar.rc.Top,
				appBar.rc.Right - appBar.rc.Left,
				appBar.rc.Bottom - appBar.rc.Top
			);
			ShowBarSize = Size;
			
			if(AutoHide) {
				WaitHidden();
			}
		}
		
		public void DockingFromProperty()
		{
			DockingFromParameter(DesktopDockType, AutoHide);
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
			if(this.timerAutoHidden.Enabled) {
				this.timerAutoHidden.Stop();
			}
			
			// 登録済みであればいったん解除
			var needResist = true;
			if(IsDocking) {
				if(DesktopDockType != dockType || AutoHide) {
					UnResistAppBar();
					needResist = true;
				} else {
					needResist = false;
				}
			}
			
			if(dockType == DesktopDockType.None) {
				// NOTE: もっかしフルスクリーン通知拾えるかもなんで登録すべきかも。
				return;
			}
			
			// 登録
			if(needResist) {
				ResistAppBar();
			}
			
			DockingFromParameter(dockType, AutoHide);
		}
		
		protected void SwitchHidden()
		{
			if(AutoHide) {
				WaitHidden();
			}
		}
		
		void StopHidden()
		{
			//Debug.WriteLine("StopHidden");
			Debug.Assert(AutoHide);
			if(this.timerAutoHidden.Enabled) {
				this.timerAutoHidden.Stop();
			}
			ToShow();
		}
		
		void WaitHidden()
		{
			Debug.Assert(AutoHide);
			if(!this.timerAutoHidden.Enabled) {
				this.timerAutoHidden.Start();
			}
		}
		
		public void Hidden()
		{
			Debug.Assert(DesktopDockType != DesktopDockType.None);
			Debug.Assert(AutoHide);

			ToHidden(true);
		}

		protected void ToHidden(bool force)
		{
			Debug.Assert(DesktopDockType != DesktopDockType.None);
			Debug.Assert(AutoHide);
			
			this.timerAutoHidden.Stop();
			
			if(!force && ClientRectangle.Contains(this.PointToClient(Control.MousePosition))) {
				return;
			}
			
			var screeanPos = DockScreen.Bounds.Location;
			var screeanSize = DockScreen.Bounds.Size;
			var size = Size;
			var pos = Location;
			switch(DesktopDockType) {
				case DesktopDockType.Top:
					size.Width = screeanSize.Width;
					size.Height = HiddenSize.Top;
					pos.X = screeanPos.X;
					pos.Y = screeanPos.Y;
					break;
					
				case DesktopDockType.Bottom:
					size.Width = screeanSize.Width;
					size.Height = HiddenSize.Bottom;
					pos.X = screeanPos.X;
					pos.Y = screeanPos.Y + screeanSize.Height - size.Height;
					break;
					
				case DesktopDockType.Left:
					size.Width = HiddenSize.Left;
					size.Height = screeanSize.Height;
					pos.X = screeanPos.X;
					pos.Y = screeanPos.Y;
					break;
					
				case DesktopDockType.Right:
					size.Width = HiddenSize.Right;
					size.Height = screeanSize.Height;
					pos.X = screeanPos.X + screeanSize.Width - HiddenSize.Right;
					pos.Y = screeanPos.Y;
					break;
					
				default:
					Debug.Assert(false, DesktopDockType.ToString());
					break;
			}
			
			HiddenView(!force, new Rectangle(pos, size));
		}
		
		protected virtual void ToShow()
		{
			Debug.Assert(DesktopDockType != DesktopDockType.None);
			Debug.Assert(AutoHide);
			
			var screeanPos = DockScreen.WorkingArea.Location;
			var screeanSize = DockScreen.WorkingArea.Size;
			var size = ShowBarSize;
			var pos = Location;
			switch(DesktopDockType) {
				case DesktopDockType.Top:
					//size.Width = screeanSize.Width;
					//size.Height = HiddenSize.Top;
					pos.X = screeanPos.X;
					pos.Y = screeanPos.Y;
					break;
					
				case DesktopDockType.Bottom:
					//size.Width = screeanSize.Width;
					//size.Height = HiddenSize.Bottom;
					pos.X = screeanPos.X;
					pos.Y = screeanPos.Y + screeanSize.Height - size.Height;
					break;
					
				case DesktopDockType.Left:
					//size.Width = HiddenSize.Left;
					//size.Height = screeanSize.Height;
					pos.X = screeanPos.X;
					pos.Y = screeanPos.Y;
					break;
					
				case DesktopDockType.Right:
					//size.Width = HiddenSize.Right;
					//size.Height = screeanSize.Height;
					pos.X = screeanPos.X + screeanSize.Width - size.Width;
					pos.Y = screeanPos.Y;
					break;
					
				default:
					Debug.Assert(false, DesktopDockType.ToString());
					break;
			}
			
			Bounds = new Rectangle(pos, size);
		}
		
		static AW ToAW(DesktopDockType type, bool show)
		{
			var result = new Dictionary<DesktopDockType, AW>() {
				{ DesktopDockType.Top,    show ? AW.AW_VER_POSITIVE: AW.AW_VER_NEGATIVE },
				{ DesktopDockType.Bottom, show ? AW.AW_VER_NEGATIVE: AW.AW_VER_POSITIVE },
				{ DesktopDockType.Left,   show ? AW.AW_HOR_POSITIVE: AW.AW_HOR_NEGATIVE },
				{ DesktopDockType.Right,  show ? AW.AW_HOR_NEGATIVE: AW.AW_HOR_POSITIVE },
			}[type];
			
			if(!show) {
				result |= AW.AW_HIDE;
			}
			
			return result;
		}

		protected virtual void HiddenView(bool animation, Rectangle area)
		{
			var prevVisible = Visible;
			if(Visible) {
				if(animation) {
					NativeMethods.AnimateWindow(Handle, (int)HiddenAnimateTime.TotalMilliseconds, ToAW(DesktopDockType, false));
				}
				Bounds = area;
				Visible = prevVisible;
			}
		}
	}
}
