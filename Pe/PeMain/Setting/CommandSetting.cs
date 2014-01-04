/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 20:46
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Linq;
using PeUtility;

namespace PeMain.Setting
{
	/// <summary>
	/// コマンドランチャー設定
	/// </summary>
	[Serializable]
	public class CommandSetting: Item, IDisposable
	{
		//private Font _font = null;
		
		public CommandSetting()
		{
			Width = 200;
			Height = 200;
			IconSize = IconSize.Small;
			HiddenTime = new TimeSpan(0, 0, 0, 1, 500);
			FontSetting = new FontSetting();
		}
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconSize IconSize { get; set; }
		/// <summary>
		/// フォント
		/// </summary>
		public FontSetting FontSetting { get; set; }
		/// <summary>
		/// 入力欄の横幅。
		/// </summary>
		public int Width { get; set; }
		/// <summary>
		/// 補助リストの高さ。
		/// </summary>
		public int Height { get; set; }
		/// <summary>
		/// 非アクティブからの非表示猶予。
		/// </summary>
		public TimeSpan HiddenTime { get; set; }
		/// <summary>
		/// 最前面表示。
		/// </summary>
		public bool TopMost { get; set; }
		
		public void Dispose()
		{
			if(FontSetting != null) {
				FontSetting.Dispose();
			}
		}
	}
}
