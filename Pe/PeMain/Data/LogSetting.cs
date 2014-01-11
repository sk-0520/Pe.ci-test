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

namespace PeMain.Data
{
	/// <summary>
	/// Description of LogSetting.
	/// </summary>
	[Serializable]
	public class LogSetting: Item
	{
		public LogSetting()
		{
			Size = new Size(350, 400);
			Point = new Point(0, 0);
		}
		
		public bool Visible { get; set; }
		public Point Point { get; set; }
		public Size Size { get; set; }
		/// <summary>
		/// ログ追加時に画面表示
		/// </summary>
		public bool ShowAdd { get; set; }
	}
}
