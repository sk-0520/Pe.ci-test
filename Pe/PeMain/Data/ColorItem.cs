namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// 色保存用。
	/// </summary>
	[Serializable]
	public class ColorItem: Item
	{
		[XmlIgnore]
		public Color Color { get; set; }

		[XmlAttribute("Color")]
		public string _Color
		{
			get 
			{
				return PropertyUtility.MixinColorGetter(Color);
			}
			set 
			{
				Color = PropertyUtility.MixinColorSetter(value);
			}
		}
	}

	[Serializable]
	public class ColorPairItem: Item
	{
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

		public void CorrectionColor(Color fore, Color back)
		{
			if(Fore.Color.IsEmpty) {
				Fore.Color = fore;
			}

			if(Back.Color.IsEmpty) {
				Back.Color = back;
			}
		}
	}
}
