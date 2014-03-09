/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 12:43
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Linq;

namespace PeMain.Data
{
	/// <summary>
	/// Description of Font.
	/// </summary>
	[Serializable]
	public class FontSetting: Item, IDisposable
	{
		private Font _font = null;
		
		public FontSetting()
		{
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
						family = FontFamily.GenericMonospace;
					}
					var size = Height;
					if(float.IsNaN(size) || size == 0.0) {
						size = SystemFonts.DefaultFont.Size;
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
		
		public void Dispose()
		{
			if(this._font != null) {
				this._font.Dispose();
			}
		}
		
		public virtual void Include(FontSetting fs) 
		{
			Height = fs.Height;
			Family = fs.Family;
		}
	}
}
