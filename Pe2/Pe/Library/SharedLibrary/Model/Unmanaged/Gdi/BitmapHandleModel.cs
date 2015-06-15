namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Gdi
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Interop;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.PInvoke.Windows;

	/// <summary>
	/// ビットマップハンドルを管理。
	/// </summary>
	public class BitmapHandleModel: GdiObjectModelBase
	{
		public BitmapHandleModel(IntPtr hBitmap)
			: base(hBitmap)
		{ }

		#region GdiObjectModelBase

		public override bool CanMakeImageSource { get { return true; } }

		protected override ImageSource MakeImageSourceImpl()
		{
			var result = Imaging.CreateBitmapSourceFromHBitmap(
				Handle,
				IntPtr.Zero,
				System.Windows.Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions()
			);

			return result;
		}

		#endregion
	}
}
