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
	/// アイコンハンドルを管理。
	/// </summary>
	public class IconHandleModel : GdiObjectModelBase
	{
		public IconHandleModel(IntPtr hIcon)
			: base(hIcon)
		{ }

		#region UnmanagedHandle

		protected override void ReleaseHandle()
		{
			NativeMethods.DestroyIcon(Handle);
			NativeMethods.SendMessage(Handle, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
		}

		#endregion

		#region GdiObjectModelBase

		public override bool CanMakeImageSource { get { return true; } }

		protected override BitmapSource MakeBitmapSourceImpl()
		{
			var result = Imaging.CreateBitmapSourceFromHIcon(
				Handle,
				System.Windows.Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions()
			);

			return result;
		}

		#endregion

	}

}
