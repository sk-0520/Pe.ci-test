namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;

	[Serializable]
	public class FloatToolbarAreaItemModel: ItemModelBase
	{
		public FloatToolbarAreaItemModel()
			: base()
		{ }

		/// <summary>
		/// 横に表示するアイテム数。
		/// </summary>
		[DataMember]
		public int Width { get; set; }
		/// <summary>
		/// 縦に表示するアイテム数。
		/// </summary>
		[DataMember]
		public int Height { get; set; }
		/// <summary>
		/// 論理X座標。
		/// </summary>
		[DataMember]
		[PixelKind(Px.Logical)]
		public double X { get; set; }
		/// <summary>
		/// 論理Y座標。
		/// </summary>
		[DataMember]
		[PixelKind(Px.Logical)]
		public double Y { get; set; }
	}
}
