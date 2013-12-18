/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:06
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.UI
{
	public partial class AppbarForm
	{
		/// <summary>
		/// ドッキングするディスプレイ
		/// </summary>
		public Screen DockScreen { get; set; }
		/// <summary>
		/// ドッキングタイプ
		/// </summary>
		public DockType DockType { get; set; }
		/// <summary>
		/// ドッキングしているか
		/// </summary>
		public bool IsDocking { get; private set; }
		/// <summary>
		/// ドッキング状態でのバーサイズ
		/// 
		/// 左右: Width
		/// 上下: Height
		/// </summary>
		public Size BarSize { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string MessageString { get; set; }
	}
}
