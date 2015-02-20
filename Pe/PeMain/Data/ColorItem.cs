namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
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
			Foreground = new ColorItem();
			Background = new ColorItem();
		}

		/// <summary>
		/// 前景色。
		/// </summary>
		public ColorItem Foreground { get; set; }
		/// <summary>
		/// 背景色。
		/// </summary>
		public ColorItem Background { get; set; }

		public void CorrectionColor(Color fore, Color back)
		{
			if(Foreground.Color.IsEmpty) {
				Foreground.Color = fore;
			}

			if(Background.Color.IsEmpty) {
				Background.Color = back;
			}
		}
	}
}
