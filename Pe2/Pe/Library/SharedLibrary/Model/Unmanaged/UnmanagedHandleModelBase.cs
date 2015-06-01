namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged
{
	using System;
	using ContentTypeTextNet.Library.PInvoke.Windows;

	/// <summary>
	/// アンマネージドオブジェクトハンドルを管理。
	/// </summary>
	public abstract class UnmanagedHandleModelBase: UnmanagedModelBase
	{
		public UnmanagedHandleModelBase(IntPtr hHandle)
			: base()
		{
			if(hHandle == IntPtr.Zero) {
				throw new ArgumentNullException("hHandle");
			}

			Handle = hHandle;
		}

		/// <summary>
		/// ハンドル。
		/// </summary>
		public IntPtr Handle { get; private set; }

		/// <summary>
		/// 解放処理。
		/// 
		/// ハンドルにより処理色々なんでオーバーライドしてごちゃごちゃする。
		/// </summary>
		protected virtual void ReleaseHandle()
		{
			NativeMethods.DeleteObject(Handle);
		}

		#region UnmanagedBase

		protected override void Dispose(bool disposing)
		{
			ReleaseHandle();
			Handle = IntPtr.Zero;

			base.Dispose(disposing);
		}

		#endregion
	}
}
