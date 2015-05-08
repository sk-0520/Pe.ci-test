namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using System.Linq;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Utility;

	/// <summary>
	/// フォント設定。
	/// </summary>
	[Serializable]
	public class FontSetting: DisposableItem, ICloneable
	{
		/// <summary>
		/// 保持用フォント。
		/// </summary>
		private Font _font = null;
		/// <summary>
		/// デフォルト値としてのフォント。
		/// </summary>
		[XmlIgnore]
		public Font DefaultFont { get; private set; }
		
		/// <summary>
		/// 
		/// </summary>
		public FontSetting()
		{
			this.DefaultFont = (Font)SystemFonts.MessageBoxFont.Clone();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="defaultFont">削除はFontSettingで処理する。</param>
		public FontSetting(Font defaultFont)
		{
			this.DefaultFont = defaultFont;
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
						family = this.DefaultFont.FontFamily;
					}
					var size = Height;
					if(float.IsNaN(size) || size == 0.0) {
						size = this.DefaultFont.SizeInPoints;
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
			this.DefaultFont.ToDispose();

			ClearFont();

			base.Dispose(disposing);
		}

		#endregion

		#region ICloneable

		public object Clone()
		{
			return new FontSetting() {
				Height = this.Height,
				Family = this.Family,
				Italic = this.Italic,
				Bold = this.Bold,
			};
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
