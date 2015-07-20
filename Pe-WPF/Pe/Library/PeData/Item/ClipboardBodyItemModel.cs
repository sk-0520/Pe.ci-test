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
	using System.Windows;
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

		#region ClipboardBodyItemModel

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

					var bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					try {
						bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
						bitmapImage.StreamSource = new MemoryStream(value);
					} finally {
						bitmapImage.EndInit();
					}

					Image = bitmapImage;
				}
			}
		}

		[DataMember]
		public CollectionModel<string> Files { get; set; }

		#endregion

		#region IndexBodyItemModelBase

		public override IndexKind IndexKind { get { return IndexKind.Clipboard; } }

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				var bitmapImage = Image as BitmapImage;
				if(bitmapImage != null && Application.Current != null) {
					Application.Current.Dispatcher.Invoke(new Action(() => {
						if(bitmapImage.StreamSource != null) {
							bitmapImage.StreamSource.Dispose();
							bitmapImage.StreamSource = null;
						}
					}));
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
