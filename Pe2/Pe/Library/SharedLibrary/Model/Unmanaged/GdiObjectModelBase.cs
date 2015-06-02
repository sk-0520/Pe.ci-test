namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;

	/// <summary>
	/// アンマネージドなGDIオブジェクトを管理。
	/// </summary>
	public abstract class GdiObjectModelBase: UnmanagedHandleModelBase
	{
		public GdiObjectModelBase(IntPtr hHandle)
			: base(hHandle)
		{ }

		#region UnmanagedHandleModelBase

		protected override void ReleaseHandle()
		{
			NativeMethods.DeleteObject(Handle);
		}

		#endregion
	}
}
