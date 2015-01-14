using System;
using System.Drawing;
using System.Linq;

using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// フォント設定。
	/// </summary>
	[Serializable]
	public class FontSetting: DisposableItem
	{
		/// <summary>
		/// 保持用フォント。
		/// </summary>
		private Font _font = null;
		/// <summary>
		/// デフォルト値としてのフォント。
		/// </summary>
		private readonly Font _defaultFont;
		
		/// <summary>
		/// 
		/// </summary>
		public FontSetting()
		{
			this._defaultFont = (Font)SystemFonts.MessageBoxFont.Clone();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="defaultFont">削除はFontSettingで処理する。</param>
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

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			this._defaultFont.ToDispose();

			ClearFont();

			base.Dispose(disposing);
		}

		#endregion

		protected void ClearFont()
		{
			this._font.ToDispose();
			this._font = null;
		}

		public virtual void Import(FontSetting fs)
		{
			ClearFont();

			Height = fs.Height;
			Family = fs.Family;
			Bold = fs.Bold;
			Italic = fs.Italic;
		}
		public virtual void Import(Font f)
		{
			ClearFont();
			
			Height = f.SizeInPoints;
			Family = f.FontFamily.Name;
			Bold = f.Bold;
			Italic = f.Italic;
		}
	}
}
