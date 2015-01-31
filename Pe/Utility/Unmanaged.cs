namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
using System.Collections.Generic;
using System.Drawing;
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

		public UnmanagedIcon(IntPtr hIcon)
		{
			if(hIcon == IntPtr.Zero) {
				throw new ArgumentNullException("hIcon");
			}

			IconHandle = hIcon;
		}

		public IntPtr IconHandle {get; private set; }

		protected override void Dispose(bool disposing)
		{
			NativeMethods.DestroyIcon(IconHandle);
			NativeMethods.SendMessage(IconHandle, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
			IconHandle = IntPtr.Zero;

			base.Dispose(disposing);
		}

		/// <summary>
		/// アイコンハンドルを元にマネージドリソースであるアイコンオブジェクトを生成。
		/// </summary>
		/// <returns>アイコン。UnmanagedIconの管轄外になるので後始末が必要なことに注意。</returns>
		public Icon ToManagedIcon()
		{
			using(var icon = Icon.FromHandle(IconHandle)) {
				return (Icon)icon.Clone();
			}
		}
	}
}
