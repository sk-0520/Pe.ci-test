namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using System.Xml.Serialization;
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

		[DataMember]
		public string Text { get; set; }
		[DataMember]
		public string Rtf { get; set; }
		[DataMember]
		public string Html { get; set; }

		[IgnoreDataMember, XmlIgnore]
		public BitmapSource Image { get; set; }

		[DataMember(Name = "Image")]
		public byte[] Image_Impl
		{
			get {
				if (Image != null) {
					//var encoder = new PngBitmapEncoder();
					var encoder = new BmpBitmapEncoder();
					encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Image.Clone()));
					byte[] array;
					using (var stream = new MemoryStream()) {
						encoder.Save(stream);
						array = stream.ToArray();
					}
					return array;
				} else {
					return null;
				}
			}
			set
			{
				if (value == null) {
					Image = null;
				} else {
					if(Image != null) {
						return;
					}

					var image = new BitmapImage();
					image.BeginInit();
					try {
						image.CacheOption = BitmapCacheOption.OnLoad;
						image.StreamSource = new MemoryStream(value);
					} finally {
						image.EndInit();
					}

					Image = image;
				}
			}
		}

		[DataMember]
		public CollectionModel<string> Files { get; set; }

		#region IndexBodyItemModelBase

		public override IndexKind IndexKind { get { return IndexKind.Clipboard; } }

		#endregion
	}
}
