using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// アプリケーションデスクトップツールバー。
	/// </summary>
	public partial class AppbarForm : Form
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		uint callbackMessage;
		DesktopDockType _desktopDockType;
		DesktopDockType _prevDesktopDockType;
		TimeSpan _hiddenTime;
		#endregion ////////////////////////////////////

		#region event
		/// <summary>
		/// フルスクリーンイベント。
		/// </summary>
		public event EventHandler<AppbarFullScreenEvent> AppbarFullScreen;

		protected void OnAppbarFullScreen(bool fullScreen)
		{
			if(AppbarFullScreen != null) {
				var e = new AppbarFullScreenEvent();
				e.FullScreen = fullScreen;
				AppbarFullScreen(this, e);
			}
		}

		/// <summary>
		/// 位置変更時に発生。
		/// </summary>
		public event EventHandler<EventArgs> AppbarPosChanged;

		protected virtual void OnAppbarPosChanged(EventArgs e)
		{
			//Docking(DesktopDockType);
			DockingFromProperty();

			if(AppbarPosChanged != null) {
				AppbarPosChanged(this, e);
			}
		}

		/// <summary>
		/// ステータス変更。
		/// </summary>
		public event EventHandler<EventArgs> AppbarStateChange;

		protected virtual void OnAppbarStateChange(EventArgs e)
		{
			if(AppbarStateChange != null) {
				AppbarStateChange(this, e);
			}
		}
		#endregion ////////////////////////////////////

		public AppbarForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		/// <summary>
		/// ドッキングするディスプレイ
		/// </summary>
		public Screen DockScreen { get; set; }
		/// <summary>
		/// ドッキングタイプ
		/// </summary>
		public virtual DesktopDockType DesktopDockType
		{
			get
			{
				return this._desktopDockType;
			}
			set
			{
				Docking(value);
				this._desktopDockType = value;
			}
		}

		public bool AutoHide { get; set; }

		/// <summary>
		/// ドッキングしているか
		/// </summary>
		public bool IsDocking { get; private set; }
		/// <summary>
		/// ドッキング状態でのバーサイズ
		/// 
		/// 左右: Width
		/// 上下: Height
		/// </summary>
		public Size BarSize { get; set; }
		/// <summary>
		/// 自動的に隠れた状態から復帰する際に使用する領域
		/// </summary>
		public Size ShowBarSize { get; private set; }
		/// <summary>
		/// 自動的に隠れた際のサイズ。
		/// 各片がドッキング位置に対応。
		/// </summary>
		public Padding HiddenSize { get; set; }
		public TimeSpan HiddenWaitTime
		{
			get { return this._hiddenTime; }
			set
			{
				this._hiddenTime = value;
				this.timerAutoHidden.Interval = (int)this._hiddenTime.TotalMilliseconds;
			}
		}
		public TimeSpan HiddenAnimateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string MessageString { get; set; }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		#endregion ////////////////////////////////////

		#region override
		protected override CreateParams CreateParams
		{
			get
			{
				var createParams = base.CreateParams;
				// AppBar として表示するには WS_EX_TOOLWINDOW スタイルが必要
				createParams.ExStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;;

				return createParams;
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
		
		protected override void OnMouseLeave(EventArgs e)
		{
			if(AutoHide && ClientRectangle.Contains(this.PointToClient(Control.MousePosition))) {
				return;
			} else {
				base.OnMouseLeave(e);
			}
		}
		
		protected override void OnControlAdded(ControlEventArgs e)
		{
			e.Control.MouseLeave += new EventHandler(e_Control_MouseLeave);
			base.OnControlAdded(e);
		}
		
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			e.Control.MouseLeave -= e_Control_MouseLeave;
			base.OnControlRemoved(e);
		}

		protected override void WndProc(ref Message m)
		{
			if(IsDocking) {
				if(m.Msg == (int)WM.WM_ACTIVATE) {
					var appBar = new APPBARDATA(Handle);
					NativeMethods.SHAppBarMessage(ABM.ABM_ACTIVATE, ref appBar);
				} else if(m.Msg == (int)WM.WM_WINDOWPOSCHANGED) {
					var appBar = new APPBARDATA(Handle);
					NativeMethods.SHAppBarMessage(ABM.ABM_WINDOWPOSCHANGED, ref appBar);
				}

				if(this.callbackMessage != 0 && m.Msg == this.callbackMessage) {
					//
					switch(m.WParam.ToInt32()) {
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

		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			AutoHide = false;
			BarSize = Size;
			DockScreen = Screen.PrimaryScreen;
			DesktopDockType = DesktopDockType.None;
			IsDocking = false;
			MessageString = "AppDesktopToolbar";
			HiddenSize = new Padding(SystemInformation.SizingBorderWidth);
			HiddenWaitTime = TimeSpan.FromSeconds(3);
			HiddenAnimateTime = TimeSpan.FromMilliseconds(500);

		}
		#endregion ////////////////////////////////////

		#region language
		#endregion ////////////////////////////////////

		#region function
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
		#endregion ////////////////////////////////////


		void e_Control_MouseLeave(object sender, EventArgs e)
		{
			SwitchHidden();
		}
		
		void AppbarFormVisibleChanged(object sender, EventArgs e)
		{
			var visible = Visible;
			if(visible) {
				DesktopDockType = this._prevDesktopDockType;
			} else {
				_prevDesktopDockType = DesktopDockType;
				DesktopDockType = DesktopDockType.None;
			}
		}
		
		void TimerAutoHide_Tick(object sender, EventArgs e)
		{
			if(IsDocking) {
				ToHidden(false);
			} else {
				this.timerAutoHidden.Stop();
			}
		}
		
		
		void AppbarForm_MouseEnter(object sender, EventArgs e)
		{
			if(AutoHide) {
				StopHidden();
			}
		}
		
		void AppbarForm_MouseLeave(object sender, EventArgs e)
		{
			if(AutoHide) {
				WaitHidden();
			}
		}
	}
}
