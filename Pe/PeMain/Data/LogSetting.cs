/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/11
 * 時刻: 23:54
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// ログ設定。
	/// </summary>
	[Serializable]
	public class LogSetting: Item
	{
		public LogSetting()
		{
			Size = new Size(
				Screen.PrimaryScreen.Bounds.Width / 4,
				Screen.PrimaryScreen.Bounds.Height / 2
			);
			var screenSize = Screen.PrimaryScreen.WorkingArea.Size;
			Point = new Point(screenSize.Width - Size.Width, screenSize.Height - Size.Height);
			AddShow = true;
			AddShowTrigger = LogType.Warning | LogType.Error;
		}
		
		/// <summary>
		/// ログダイアログの表示状態。
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// ログダイアログの位置。
		/// </summary>
		public Point Point { get; set; }
		/// <summary>
		/// ログダイアログのサイズ。
		/// </summary>
		public Size Size { get; set; }
		/// <summary>
		/// ログ追加時にログダイアログを表示するか。
		/// </summary>
		public bool AddShow { get; set; }
		/// <summary>
		/// ログダイアログを表示する際にどの種別で表示するか。
		/// </summary>
		public LogType AddShowTrigger { get; set; }
		
		/// <summary>
		/// 詳細部を全面表示。
		/// </summary>
		public bool FullDetail { get; set; }
	}
}
