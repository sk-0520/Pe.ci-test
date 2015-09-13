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
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
	public class FloatToolbarItemModel: ItemModelBase, IDeepClone
	{
		public FloatToolbarItemModel()
			: base()
		{ }

		/// <summary>
		/// 横に表示するアイテム数。
		/// </summary>
		[DataMember]
		public int WidthButtonCount { get; set; }
		/// <summary>
		/// 縦に表示するアイテム数。
		/// </summary>
		[DataMember]
		public int HeightButtonCount { get; set; }
		/// <summary>
		/// 論理X座標。
		/// </summary>
		[DataMember]
		[PixelKind(Px.Logical)]
		public double Left { get; set; }
		/// <summary>
		/// 論理Y座標。
		/// </summary>
		[DataMember]
		[PixelKind(Px.Logical)]
		public double Top { get; set; }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (FloatToolbarItemModel)target;

			obj.WidthButtonCount = WidthButtonCount;
			obj.HeightButtonCount = HeightButtonCount;
			obj.Left = Left;
			obj.Top = Top;
		}

		public IDeepClone DeepClone()
		{
			var result = new FloatToolbarItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
