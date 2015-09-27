namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// 前景色・背景色データ。
	/// </summary>
	public class ColorPairItemModel: ItemModelBase, IDeepClone, IColorPair
	{
		public ColorPairItemModel()
			:base()
		{ }

		public ColorPairItemModel(Color fore, Color back)
			:this()
		{
			ForeColor = fore;
			BackColor = back;
		}

		#region IColorPair
		
		/// <summary>
		/// 前景色。
		/// </summary>
		public Color ForeColor { get; set; }
		/// <summary>
		/// 背景色。
		/// </summary>
		public Color BackColor { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (ColorPairItemModel)target;

			obj.ForeColor = ForeColor;
			obj.BackColor = BackColor;
		}

		public IDeepClone DeepClone()
		{
			var result = new ColorPairItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
