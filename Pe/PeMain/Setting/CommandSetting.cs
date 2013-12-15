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
	public class CommandSetting: Item, IDisposable
	{
		private Font _font = null;
		
		public CommandSetting()
		{
			Width = 200;
			Height = 200;
		}
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconSize IconSize { get; set; }
		/// <summary>
		/// フォント名。
		/// </summary>
		public string FontName { get; set; }
		/// <summary>
		/// フォントサイズ。
		/// </summary>
		public float FontSize { get; set; }
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
		
		/// <summary>
		/// フォント。
		/// </summary>
		public Font Font
		{
			get
			{
				if(this._font == null) {
					FontFamily family = null;
					if(!string.IsNullOrWhiteSpace(FontName)) {
						family = FontFamily.Families.SingleOrDefault(f => f.Name == FontName);
					}
					if(family == null) {
						family = FontFamily.GenericMonospace;
					}
					var size = FontSize;
					if(float.IsNaN(size) || size == 0.0) {
						size = SystemFonts.DefaultFont.Size;
					}
					this._font = new Font(family, size);
				}
				
				return this._font;
			}
		}
		
		public void Dispose()
		{
			if(Font != null) {
				Font.Dispose();
			}
		}
	}
}
