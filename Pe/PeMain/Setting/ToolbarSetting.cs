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

namespace PeMain.Setting
{
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
	
	public class ToolbarPositionItem: UseLanguageItemData<ToolbarPosition>
	{
		public ToolbarPositionItem(ToolbarPosition value): base(value) { }
		public ToolbarPositionItem(ToolbarPosition value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
	/// <summary>
	/// 
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
	/// 
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
	/// 
	/// </summary>
	public class ToolbarSetting: Item
	{
		public ToolbarSetting()
		{
			FontSetting = new FontSetting();
			ToolbarGroup = new ToolbarGroup();
			ToolbarPosition = ToolbarPosition.DesktopFloat;
			IconSize = IconSize.Small;
			Visible = false;
			Topmost = false;
			AutoHide = false;
			FloatSize = Literal.toolbarFloatSize;
			DesktopSize = Literal.toolbarDesktopSize;
		}
		
		public ToolbarGroup ToolbarGroup { get; set; }
		
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
		/// デスクトップでのサイズ
		/// </summary>
		public Size DesktopSize { get; set; }
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconSize IconSize { get; set; }
		/// <summary>
		/// フォント
		/// </summary>
		public FontSetting FontSetting { get; set; }
	}
}
