/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/05/2013
 * 時刻: 23:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows;

namespace Pe.Logic
{
	/// <summary>
	/// 
	/// </summary>
	public enum WindowShow
	{
		/// <summary>
		/// 通常
		/// </summary>
		Normal,
		/// <summary>
		/// 最大化
		/// </summary>
		Maxim,
		/// <summary>
		/// ←
		/// </summary>
		Left,
		/// <summary>
		/// ↑
		/// </summary>
		Top,
		/// <summary>
		/// →
		/// </summary>
		Right,
		/// <summary>
		/// ↓
		/// </summary>
		Bottom,
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class WindowViewItemData: ItemData
	{
		const string AttributePoint = "point";
		const string AttributeSize = "size";
		const string AttributeWindowShow = "show";
		const string AttributeTopmost = "topmost";
		const string AttributeAutoHide = "autohide";
		const string AttributeTasskbar = "taskbar";
		
		/// <summary>
		/// 
		/// </summary>
		public override string Name { get { return "view"; } }
		
		/// <summary>
		/// 
		/// </summary>
		public Point Point { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Size Size { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public WindowShow WindowShow { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool TopMost { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool AutoHide { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Taskbar { get; set; }
	}
}
