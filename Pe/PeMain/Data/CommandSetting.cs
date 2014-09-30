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
using System.Windows.Forms;
using System.Windows.Input;

using PeUtility;

namespace PeMain.Data
{
	/// <summary>
	/// コマンドランチャー設定
	/// </summary>
	[Serializable]
	public class CommandSetting: DisposableItem, IDisposable
	{
		//private Font _font = null;
		
		public CommandSetting()
		{
			Width = 200;
			Height = 200;
			IconScale = IconScale.Small;
			HiddenTime = new TimeSpan(0, 0, 0, 1, 500);
			FontSetting = new FontSetting();
			HotKey = new HotKeySetting();
		}
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconScale IconScale { get; set; }
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
		
		public HotKeySetting HotKey { get; set; }
		
		public override void Dispose()
		{
			if(FontSetting != null) {
				FontSetting.Dispose();
			}
		}
	}
}
