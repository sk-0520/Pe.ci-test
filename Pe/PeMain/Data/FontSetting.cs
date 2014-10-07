/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 12:43
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using PeUtility;

namespace PeMain.Data
{
	/// <summary>
	/// フォント設定。
	/// </summary>
	[Serializable]
	public class FontSetting: DisposableItem, IDisposable
	{
		/// <summary>
		/// 保持用フォント。
		/// </summary>
		private Font _font = null;
		/// <summary>
		/// デフォルト値としてのフォント。
		/// </summary>
		private readonly Font _defaultFont;
		
		public FontSetting()
		{
			this._defaultFont = SystemFonts.MessageBoxFont;
		}
		
		public FontSetting(Font defaultFont)
		{
			this._defaultFont = defaultFont;
		}
		
		/// <summary>
		/// 高さ
		/// </summary>
		public float Height { get; set; }
		/// <summary>
		/// フォント名
		/// </summary>
		public string Family { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Italic { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Bold { get; set; }
		
		public Font Font
		{
			get
			{
				if(this._font == null) {
					FontFamily family = null;
					if(!string.IsNullOrWhiteSpace(Family)) {
						family = FontFamily.Families.SingleOrDefault(f => f.Name == Family);
					}
					if(family == null) {
						family = this._defaultFont.FontFamily;
					}
					var size = Height;
					if(float.IsNaN(size) || size == 0.0) {
						size = this._defaultFont.SizeInPoints;
					}
					
					this._font = new Font(family, size);
				}
				
				return this._font;
			}
		}
		
		public bool IsDefault
		{
			get { return string.IsNullOrWhiteSpace(this.Family); }
		}
		
		public override void Dispose()
		{
			this._font.ToDispose();
			this._font = null;
		}
		
		public virtual void Include(FontSetting fs)
		{
			Dispose();
			
			Height = fs.Height;
			Family = fs.Family;
			Bold = fs.Bold;
			Italic = fs.Italic;
			
			IsDisposed = false;
		}
		public virtual void Include(Font f)
		{
			Dispose();
			
			Height = f.SizeInPoints;
			Family = f.FontFamily.Name;
			Bold = f.Bold;
			Italic = f.Italic;
			
			IsDisposed = false;
		}
		
		public string ToViewText(Language language)
		{
			string viewText = language["common/command/default-font"];
			if(!IsDefault) {
				viewText = string.Format("{0} {1}", Family, Height);
			}
			
			return viewText;
		}

	}
}
