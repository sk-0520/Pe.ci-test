/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 11:45
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;

using PeMain.Logic;
using PeUtility;

namespace PeMain.Data
{
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
	
	/// <summary>
	/// 
	/// </summary>
	public static class ToolbarPositionUtility
	{
		public static ToolbarPosition ToToolbarPosition(DesktopDockType value)
		{
			return new Dictionary<DesktopDockType, ToolbarPosition>() {
				{ DesktopDockType.Left,   ToolbarPosition.DesktopLeft },
				{ DesktopDockType.Top,    ToolbarPosition.DesktopTop },
				{ DesktopDockType.Right,  ToolbarPosition.DesktopRight },
				{ DesktopDockType.Bottom, ToolbarPosition.DesktopBottom },
			}[value];
		}
		public static DesktopDockType ToDockType(ToolbarPosition value)
		{
			return new Dictionary<ToolbarPosition, DesktopDockType>() {
				{ToolbarPosition.DesktopLeft,   DesktopDockType.Left },
				{ToolbarPosition.DesktopTop,    DesktopDockType.Top },
				{ToolbarPosition.DesktopRight,  DesktopDockType.Right },
				{ToolbarPosition.DesktopBottom, DesktopDockType.Bottom },
			}[value];
		}
		public static bool IsDockingMode(ToolbarPosition value)
		{
			return value.IsIn(ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopRight, ToolbarPosition.DesktopBottom);
		}
		public static bool IsHorizonMode(ToolbarPosition pos)
		{
			return pos.IsIn(
				ToolbarPosition.DesktopFloat,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopBottom,
				ToolbarPosition.WindowTop,
				ToolbarPosition.WindowBottom
			);
		}
		
	}
	
	/// <summary>
	/// ツールバーグループとして名前を管理。
	/// </summary>
	[Serializable]
	public class ToolbarGroupItem: NameItem
	{
		public ToolbarGroupItem()
		{
			ItemNames = new List<string>();
		}
		
		public List<string> ItemNames { get; set; }
	}

	/// <summary>
	/// ツールバーの各グループを統括。
	/// </summary>
	[Serializable]
	public class ToolbarGroup: Item
	{
		public ToolbarGroup()
		{
			Groups = new List<ToolbarGroupItem>();
		}
		
		public List<ToolbarGroupItem> Groups { get; set; }
	}
	
	/// <summary>
	/// ツールバーの設定。
	/// </summary>
	public class ToolbarItem: DisposableNameItem, IDisposable
	{
		#region Equals and GetHashCode implementation !![ operator == ]!!
		public override bool Equals(object obj)
		{
			LauncherItem item = obj as LauncherItem;
			if(item == null) {
				return false;
			}
			return IsNameEqual(item.Name);
		}
		
		public override int GetHashCode()
		{
			//if(this.Name == null) {
			//	return default(int);
			//}
			return Name.GetHashCode();
		}
		#endregion

		public ToolbarItem()
		{
			FontSetting = new FontSetting();
			ToolbarPosition = ToolbarPosition.DesktopFloat;
			IconScale = IconScale.Small;
			ShowText = false;
			Visible = true;
			Topmost = true;
			AutoHide = false;
			FloatSize = Literal.toolbarFloatSize;
			DesktopSize = Literal.toolbarDesktopSize;
			TextWidth = Literal.toolbarTextWidth;
		}
		/// <summary>
		/// 表示
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// 最前面表示
		/// </summary>
		public bool Topmost { get; set; }
		/// <summary>
		/// ツールバー位置
		/// </summary>
		public ToolbarPosition ToolbarPosition { get; set; }
		/// <summary>
		/// 自動的に隠す
		/// </summary>
		public bool AutoHide { get; set; }
		/// <summary>
		/// フロート状態サイズ
		/// </summary>
		public Size FloatSize { get; set; }
		/// <summary>
		/// フロート状態位置
		/// </summary>
		public Point FloatLocation { get; set; }
		/// <summary>
		/// デスクトップでのサイズ
		/// </summary>
		public Size DesktopSize { get; set; }
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconScale IconScale { get; set; }
		/// <summary>
		/// テキスト表示
		/// </summary>
		public bool ShowText { get; set; }
		/// <summary>
		/// テキスト表示欄の長さ(px)
		/// </summary>
		public int TextWidth { get; set; }
		/// <summary>
		/// フォント
		/// </summary>
		public FontSetting FontSetting { get; set; }
		/// <summary>
		/// 非表示までの時間
		/// </summary>
		public TimeSpan HiddenWaitTime { get; set; }
		/// <summary>
		/// 非表示のアニメーション時間
		/// </summary>
		public TimeSpan HiddenAnimateTime { get; set; }
		
		public bool IsNameEqual(string name)
		{
			return Name == name;
		}
		
		public override void Dispose()
		{
			base.Dispose();
			
			FontSetting.Dispose();
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class ToolbarSetting: Item
	{
		public ToolbarSetting()
		{
			ToolbarGroup = new ToolbarGroup();
			
			Items = new HashSet<ToolbarItem>();
		}
		
		public ToolbarGroup ToolbarGroup { get; set; }
		public HashSet<ToolbarItem> Items { get; set; }
	}
}
