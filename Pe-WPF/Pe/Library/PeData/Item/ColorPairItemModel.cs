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

	public class ColorPairItemModel: ItemModelBase, IDeepClone, IColorPair
	{
		#region IColorPair
		
		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }

		#endregion

		#region 
	
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
