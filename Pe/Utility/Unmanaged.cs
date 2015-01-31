namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public abstract class UnmanagedBase: IDisposable
	{
		protected UnmanagedBase()
		{
			IsDisposed = false;
		}

		~UnmanagedBase()
		{
			Dispose(false);
		}

		/// <summary>
		/// 破棄されたか。
		/// </summary>
		public bool IsDisposed { get; protected set; }

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			IsDisposed = true;
		}

		/// <summary>
		/// 解放。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

	}

	/// <summary>
	/// アイコンハンドルを管理。
	/// </summary>
	public class UnmanagedIcon: UnmanagedBase
	{
		public UnmanagedIcon(IntPtr hIcon)
		{
			IconHandle = hIcon;
		}

		public IntPtr IconHandle {get; private set; }

		protected override void Dispose(bool disposing)
		{
			NativeMethods.DestroyIcon(IconHandle);
			IconHandle = IntPtr.Zero;

			base.Dispose(disposing);
		}
	}
}
