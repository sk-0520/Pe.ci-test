/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/05/2013
 * 時刻: 23:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;

namespace Pe.Logic
{
	/// <summary>
	/// 
	/// </summary>
	public enum TextPosition
	{
		/// <summary>
		/// 表示しない。
		/// 
		/// アイコンは強制表示される。
		/// </summary>
		None,
		/// <summary>
		/// 右側
		/// </summary>
		Right,
		/// <summary>
		/// 下側
		/// </summary>
		Bottom
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class WindowToolItemData: ItemData
	{
		const string AttributeShowIcon = "showicon";
		const string AttributeIconSize = "iconsize";
		const string AttributeTextPosition = "textpos";
		
		/// <summary>
		/// 
		/// </summary>
		public WindowToolItemData()
		{
			LauncherItemIdList = new List<string>();
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ShowIcon { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public ShareLib.IconSize IconSize { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public TextPosition TextPosition { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public List<string> LauncherItemIdList { get; set; }
	}
}
