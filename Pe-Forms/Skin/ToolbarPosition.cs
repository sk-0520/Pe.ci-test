namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System.Linq;

	/// <summary>
	/// ツールバーの位置。
	/// </summary>
	public enum ToolbarPosition
	{
		/// <summary>
		/// フロート
		/// </summary>
		DesktopFloat,
		/// <summary>
		/// デスクトップ 左側
		/// </summary>
		DesktopLeft,
		/// <summary>
		/// デスクトップ 上側
		/// </summary>
		DesktopTop,
		/// <summary>
		/// デスクトップ 右側
		/// </summary>
		DesktopRight,
		/// <summary>
		/// デスクトップ 下側
		/// </summary>
		DesktopBottom,
		/// <summary>
		/// アクティブウィンドウ 左側
		/// </summary>
		WindowLeft,
		/// <summary>
		/// アクティブウィンドウ 上側
		/// </summary>
		WindowTop,
		/// <summary>
		/// アクティブウィンドウ 右側
		/// </summary>
		WindowRight,
		/// <summary>
		/// アクティブウィンドウ 下側
		/// </summary>
		WindowBottom,
	}

	public static class ToolbarPositionUtility
	{
		public static bool IsDockingMode(ToolbarPosition value)
		{
			var targetList = new[] { 
				ToolbarPosition.DesktopLeft,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopRight,
				ToolbarPosition.DesktopBottom
			};

			return targetList.Any(tp => tp == value);
		}
		public static bool IsHorizonMode(ToolbarPosition pos)
		{
			var targetList = new [] {
				ToolbarPosition.DesktopFloat,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopBottom,
				ToolbarPosition.WindowTop,
				ToolbarPosition.WindowBottom
			};

			return targetList.Any(tp => tp == pos);
		}
	}
}
