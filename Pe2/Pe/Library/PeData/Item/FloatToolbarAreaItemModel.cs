namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	[DataContract, Serializable]
	public class FloatToolbarAreaItemModel : ItemModelBase
	{
		/// <summary>
		/// 横に表示するアイテム数。
		/// </summary>
		public int Width { get; set; }
		/// <summary>
		/// 縦に表示するアイテム数。
		/// </summary>
		public int Height { get; set; }
		/// <summary>
		/// 論理XY座標。
		/// </summary>
		public Point Location { get; set; }
	}
}
