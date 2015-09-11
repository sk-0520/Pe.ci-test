namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Define;

	[Serializable]
	public class ClipboardLimitSizeItemModel : ItemModelBase, IDeepClone
	{
		/// <summary>
		/// 取り込み制限対象。
		/// </summary>
		[DataMember]
		public ClipboardType LimitType { get; set; }
		[DataMember]
		public int Text { get; set; }
		[DataMember]
		public int Rtf { get; set; }
		[DataMember]
		public int Html { get; set; }
		[DataMember]
		public int ImageWidth { get; set; }
		[DataMember]
		public int ImageHeight { get; set; }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (ClipboardLimitSizeItemModel)target;

			obj.LimitType = LimitType;
			obj.Text = Text;
			obj.Rtf = Rtf;
			obj.Html = Html;
			obj.ImageWidth = ImageWidth;
			obj.ImageHeight = ImageHeight;
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
