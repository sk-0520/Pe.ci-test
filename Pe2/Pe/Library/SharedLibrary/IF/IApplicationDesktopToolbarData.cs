namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public interface IApplicationDesktopToolbarData : IWindowsViewExtendRestrictionViewModelMarker
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
		/// 表示状態。
		/// </summary>
		Visibility Visibility { get; set; }
		/// <summary>
		/// バーの論理サイズ
		/// </summary>
		[PixelKind(Px.Logical)]
		Size BarSize { get; set; }
		/// <summary>
		/// 表示中の論理バーサイズ。
		/// </summary>
		[PixelKind(Px.Logical)]
		Rect ShowLogicalBarArea { get; set; }
		/// <summary>
		/// 隠れた状態のバー論理サイズ。
		/// </summary>
		[PixelKind(Px.Logical)]
		double HideWidth { get; set; }
		/// <summary>
		/// 表示中の隠れたバーの論理領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		Rect HideLogicalBarArea { get; set; }
		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		TimeSpan HideWaitTime { get; set; }
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		TimeSpan HideAnimateTime { get; set; }
		/// <summary>
		/// ドッキングに使用するスクリーン。
		/// </summary>
		ScreenModel DockScreen { get; set; }

		/// <summary>
		/// 指定位置に合わせてデータ書き換え
		/// </summary>
		void ChangingWindowMode(DockType dockType);
	}
}
