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
	/// <summary>
	/// ツールバー
	/// </summary>
	public class ToolbarItem: Item
	{
		public ToolbarItem()
		{
			FontSetting = new FontSetting();
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
		/// <summary>
		/// デスクトップ状態でのディスプレイ
		/// </summary>
		public string DesktopDisplay { get; set; }
	}
	
	public class ToolbarSetting: Item
	{
		public ToolbarSetting()
		{
			Items = new Dictionary<string, ToolbarItem>();
		}
		
		public Dictionary<string, ToolbarItem> Items { get; set; }
	}
}
