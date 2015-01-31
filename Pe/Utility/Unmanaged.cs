namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	/// <summary>
	/// アンマネージドオブジェクトを管理してくれそうな人。
	/// </summary>
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
	/// オブジェクトハンドルを管理。
	/// </summary>
	public class UnmanagedHandle: UnmanagedBase
	{
		public UnmanagedHandle(IntPtr hHandle): base()
		{
			Handle = hHandle;
		}

		/// <summary>
		/// ハンドル。
		/// </summary>
		public IntPtr Handle {get; private set; }

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

	/// <summary>
	/// アイコンハンドルを管理。
	/// </summary>
	public class UnmanagedIcon: UnmanagedHandle
	{
		/// <summary>
		/// ビットマップからアンマネージドアイコンの生成。
		/// </summary>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		public static UnmanagedIcon FromBitmap(Bitmap bitmap)
		{
			if(bitmap == null) {
				throw new ArgumentNullException("bitmap");
			}

			var hIcon = bitmap.GetHicon();
			return new UnmanagedIcon(hIcon);
		}

		public UnmanagedIcon(IntPtr hIcon): base(hIcon)
		{
			if(hIcon == IntPtr.Zero) {
				throw new ArgumentNullException("hIcon");
			}
		}

		#region UnmanagedHandle

		protected override void ReleaseHandle()
		{
			NativeMethods.DestroyIcon(Handle);
			NativeMethods.SendMessage(Handle, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
		}

		#endregion

		/// <summary>
		/// アイコンハンドルを元にマネージドリソースであるアイコンオブジェクトを生成。
		/// </summary>
		/// <returns>アイコン。UnmanagedIconの管轄外になるので後始末が必要なことに注意。</returns>
		public Icon ToManagedIcon()
		{
			using(var icon = Icon.FromHandle(Handle)) {
				return (Icon)icon.Clone();
			}
		}
	}
}

