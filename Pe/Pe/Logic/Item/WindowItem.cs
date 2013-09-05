/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/05
 * 時刻: 23:10
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
	public enum WindowType
	{
		/// <summary>
		/// アイテムが定間隔で並ぶツールバーチックなもの
		/// </summary>
		ToolWindow,
		/// <summary>
		/// ダ、ダイアログ…
		/// </summary>
		Dialog,
	}
	
		
	/// <summary>
	/// 
	/// </summary>
	public class WindowItem: TitleItem
	{
		const string AttributeWindowType = "type";
		
		
		/// <summary>
		/// 
		/// </summary>
		public WindowItem WindowType { get; set; }
		
		
	}
}
