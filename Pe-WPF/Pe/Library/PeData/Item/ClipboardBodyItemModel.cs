namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	[DataContract, Serializable]
	public class ClipboardBodyItemModel: IndexBodyItemModelBase
	{
		public ClipboardBodyItemModel()
			: base()
		{
			Files = new CollectionModel<string>();
		}

		public string Text { get; set; }
		public string Rtf { get; set; }
		public string Html { get; set; }

		public BitmapSource Image { get; set; }
		public CollectionModel<string> Files {get;set; }

		#region IndexBodyItemModelBase

		public override IndexKind IndexKind { get { return IndexKind.Clipboard; } }

		#endregion
	}
}
