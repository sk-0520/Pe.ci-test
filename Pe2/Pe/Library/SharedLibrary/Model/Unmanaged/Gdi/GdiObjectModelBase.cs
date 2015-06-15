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
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// アンマネージドなGDIオブジェクトを管理。
	/// </summary>
	public abstract class GdiObjectModelBase: UnmanagedHandleModelBase
	{
		public GdiObjectModelBase(IntPtr hHandle)
			: base(hHandle)
		{ }

		#region property

		public virtual bool CanMakeImageSource { get { return false; } }

		#endregion

		#region UnmanagedHandleModelBase

		protected override void ReleaseHandle()
		{
			NativeMethods.DeleteObject(Handle);
		}

		#endregion

		#region function

		protected virtual BitmapSource MakeImageSourceImpl()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// GDIオブジェクトからImageSource作成。
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException">CanMakeImageSource</exception>
		public BitmapSource MakeImageSource()
		{
			CheckUtility.Enforce<NotImplementedException>(CanMakeImageSource);
			return MakeImageSourceImpl();
		}

		#endregion
	}
}
