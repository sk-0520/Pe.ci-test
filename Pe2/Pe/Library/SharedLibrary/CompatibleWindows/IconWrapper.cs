namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows
{
	using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Gdi;
using Drawing = System.Drawing;

	public class IconWrapper: DisposeFinalizeBase, IMakeBitmapSource
	{
		IconWrapper(Drawing.Icon icon)
		{
			Icon = icon;
		}

		public IconWrapper(Stream stream, IconScale iconScale)
			: this(new Drawing.Icon(stream, DrawingUtility.Convert(iconScale.ToSize())))
		{ }

		public IconWrapper(StreamResourceInfo streamInfo, IconScale iconScale)
			: this(streamInfo.Stream, iconScale)
		{ }

		public IconWrapper(string applicationResourcePath, IconScale iconScale)
			: this(Application.GetResourceStream(SharedConstants.GetPackUri(applicationResourcePath)), iconScale)
		{
			var uri = SharedConstants.GetPackUri(applicationResourcePath);
		}

		#region property

		Drawing.Icon Icon { get; set; }

		#endregion

		#region IMakeBitmapSource

		public BitmapSource MakeBitmapSource()
		{
			using(var hIcon = new IconHandleModel(Icon.Handle)) {
				System.Diagnostics.Debug.WriteLine(hIcon.MakeBitmapSource());
				var im = hIcon.MakeBitmapSource();
				return hIcon.MakeBitmapSource();
			}
		}

		#endregion
	}
}
