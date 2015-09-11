namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public class ClipboardLimitSizeItemModel : ItemModelBase, IDeepClone
	{
		public uint Text { get; set; }
		public uint Rtf { get; set; }
		public uint Html { get; set; }
		public Size Image { get; set; }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (ClipboardLimitSizeItemModel)target;

			obj.Text = Text;
			obj.Rtf = Rtf;
			obj.Html = Html;
			obj.Image = Image;
		}

		public IDeepClone DeepClone()
		{
			var result = new ClipboardLimitSizeItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion


	}
}
