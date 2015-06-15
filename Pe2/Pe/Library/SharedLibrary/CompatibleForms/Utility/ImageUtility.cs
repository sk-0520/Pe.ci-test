namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Drawing = System.Drawing;

	public static class ImageUtility
	{
		public static ImageSource ImageSourceFromIcon(Drawing.Icon icon)
		{
			var result = Imaging.CreateBitmapSourceFromHIcon(
				icon.Handle,
				Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions()
			);
			return result;
		}

		public static ImageSource ImageSourceFromBinaryIcon(byte[] binayIcon, Size iconSize)
		{
			using (var ms = new MemoryStream(binayIcon)) {
				var size = new Drawing.Size((int)iconSize.Width, (int)iconSize.Height);
				using (var icon = new Drawing.Icon(ms, size)) {
					return ImageSourceFromIcon(icon);
				}
			}
		}
	}
}
