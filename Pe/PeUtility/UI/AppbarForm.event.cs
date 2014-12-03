/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 17:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeUtility
{
	/// <summary>
	/// フルスクリーンイベント。
	/// </summary>
	public class AppbarFullScreenEvent: EventArgs
	{
		/// <summary>
		/// 検知したフルスクリーン状態。
		/// 
		/// 真でフルスクリーン。
		/// </summary>
		public bool FullScreen { get; set; }
	}
	
	public partial class AppbarForm
	{
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

			if (AppbarPosChanged != null) {
				AppbarPosChanged(this, e);
			}
		}
		
		/// <summary>
		/// ステータス変更。
		/// </summary>
		public event EventHandler<EventArgs> AppbarStateChange;

		protected virtual void OnAppbarStateChange(EventArgs e)
		{
			if (AppbarStateChange != null) {
				AppbarStateChange(this, e);
			}
		}
	}
}
