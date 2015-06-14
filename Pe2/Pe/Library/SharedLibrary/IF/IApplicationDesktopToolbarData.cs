namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public interface IApplicationDesktopToolbarData
	{
		uint CallbackMessage { get; set; }
		/// <summary>
		/// 他ウィンドウがフルスクリーン表示。
		/// </summary>
		bool NowFullScreen { get; set; }
		/// <summary>
		/// ドッキング中か。
		/// </summary>
		bool IsDocking { get; set; }
		/// <summary>
		/// ドッキング種別。
		/// </summary>
		DockType DockType { get; set; }
		/// <summary>
		/// 自動的に隠す。
		/// </summary>
		bool AutoHide { get; set; }
		/// <summary>
		/// 隠れているか。
		/// </summary>
		bool IsHidden { get; set; }
		/// <summary>
		/// バーの論理サイズ
		/// </summary>
		[PixelKind(Px.Logical)]
		Size BarSize { get; set; }
		/// <summary>
		/// 表示中の物理バーサイズ。
		/// </summary>
		[PixelKind(Px.Device)]
		Rect ShowDeviceBarArea { get; set; }
		/// <summary>
		/// 隠れた状態のバー論理サイズ。
		/// <para>横: Widthを使用</para>
		/// <para>縦: Heightを使用</para>
		/// </summary>
		[PixelKind(Px.Logical)]
		Size HideSize { get; set; }
		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		TimeSpan HiddenWaitTime { get; set; }
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		TimeSpan HiddenAnimateTime { get; set; }
		/// <summary>
		/// ドッキングに使用するスクリーン。
		/// </summary>
		ScreenModel DockScreen { get; set; }
	}
}
