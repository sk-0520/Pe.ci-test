namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[Serializable]
	public class WindowStatus: ItemModelBase
	{
		public WindowStatus()
			: base()
		{ }

		/// <summary>
		/// 論理X座標。
		/// </summary>
		public double X { get; set; }
		/// <summary>
		/// 論理Y座標。
		/// </summary>
		public double Y { get; set; }
		/// <summary>
		/// 論理横幅。
		/// </summary>
		public double Width { get; set; }
		/// <summary>
		/// 論理高さ。
		/// </summary>
		public double Height { get; set; }
	}
}
