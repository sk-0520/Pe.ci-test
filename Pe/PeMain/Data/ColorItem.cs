namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
using System.Drawing;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// 色保存用。
	/// </summary>
	[Serializable]
	public class ColorItem: Item, ICloneable
	{
		/// <summary>
		/// 色情報。
		/// </summary>
		[XmlIgnore]
		public Color Color { get; set; }

		[XmlAttribute("Color")]
		public string _Color
		{
			get { return PropertyUtility.MixinColorGetter(Color); }
			set { Color = PropertyUtility.MixinColorSetter(value); }
		}

		#region ICloneable

		public object Clone()
		{
			return new ColorItem() {
				Color = this.Color,
			};
		}
		#endregion
	}

	/// <summary>
	/// 前景色・背景色を保持する色情報。
	/// </summary>
	[Serializable]
	public class ColorPairItem: Item, IDeepClone
	{
		/// <summary>
		/// 
		/// </summary>
		public ColorPairItem()
		{
			Fore = new ColorItem();
			Back = new ColorItem();
		}

		/// <summary>
		/// 前景色。
		/// </summary>
		public ColorItem Fore { get; set; }
		/// <summary>
		/// 背景色。
		/// </summary>
		public ColorItem Back { get; set; }

		#region Item

		public void CorrectionColor(Color fore, Color back)
		{
			if(Fore.Color.IsEmpty) {
				Fore.Color = fore;
			}

			if(Back.Color.IsEmpty) {
				Back.Color = back;
			}
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			return new ColorPairItem() {
				Fore = (ColorItem)this.Fore.Clone(),
				Back = (ColorItem)this.Back.Clone(),
			};
		}

		#endregion
	}
}
