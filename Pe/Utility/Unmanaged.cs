namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Drawing;
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
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	/// <summary>
	/// アンマネージドオブジェクトハンドルを管理。
	/// </summary>
	public abstract class UnmanagedHandle: UnmanagedBase
	{
		public UnmanagedHandle(IntPtr hHandle)
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
	/// アンマネージドGDIオブジェクトを管理。
	/// </summary>
	public abstract class UnmanagedGDIObject: UnmanagedHandle
	{
		public UnmanagedGDIObject(IntPtr hGdiobj)
			: base(hGdiobj)
		{ }
	}

	/// <summary>
	/// DCの管理用基底クラス。
	/// </summary>
	public class UnmanagedDeviceContextBase: UnmanagedHandle
	{
		public class SelectedObject: IDisposable
		{
			protected internal SelectedObject(UnmanagedDeviceContextBase hDC, IntPtr handle)
			{
				DC = hDC;
				SelectedHandle = handle;
			}

			public UnmanagedDeviceContextBase DC { get; private set; }
			public IntPtr SelectedHandle { get; private set; }

			#region IDisposable

			public void Dispose()
			{
				Rollback();
			}

			#endregion

			public void Rollback()
			{
				NativeMethods.SelectObject(DC.Handle, SelectedHandle);
			}
		}

		public UnmanagedDeviceContextBase(IntPtr hDC)
			: base(hDC)
		{ }

		public SelectedObject SelectObject(UnmanagedGDIObject gdiObject)
		{
			var selectedObject = NativeMethods.SelectObject(Handle, gdiObject.Handle);

			return new SelectedObject(this, selectedObject);
		}
	}

	/// <summary>
	/// メモリデバイスコンテキスト。
	/// </summary>
	public class UnmanagedCompatibleDeviceContext: UnmanagedDeviceContextBase
	{
		/// <summary>
		/// デバイスコンテキスト互換のメモリデバイスコンテキストを生成。
		/// </summary>
		/// <param name="hDC"></param>
		public UnmanagedCompatibleDeviceContext(IntPtr hDC)
			: base(NativeMethods.CreateCompatibleDC(hDC))
		{ }

		/// <summary>
		/// アンマネージドデバイスコンテキスト互換のメモリデバイスコンテキストを生成。
		/// </summary>
		/// <param name="hDC"></param>
		public UnmanagedCompatibleDeviceContext(UnmanagedDeviceContextBase hDC)
			: base(NativeMethods.CreateCompatibleDC(hDC.Handle))
		{ }

		protected override void ReleaseHandle()
		{
			NativeMethods.DeleteDC(Handle);
		}
	}

	/// <summary>
	/// Graphics紐付けデバイスコンテキスト。
	/// </summary>
	public class UnmanagedGraphicsDeviceContext: UnmanagedDeviceContextBase
	{
		public UnmanagedGraphicsDeviceContext(Graphics g)
			: base(g.GetHdc())
		{
			Graphics = g;
		}

		protected Graphics Graphics { get; set; }

		protected override void ReleaseHandle()
		{
			if(Graphics != null) {
				Graphics.ReleaseHdc(Handle);
				Graphics = null;
			}
		}
	}


	public class UnmanagedFont: UnmanagedGDIObject
	{
		public UnmanagedFont(IntPtr hFont)
			: base(hFont)
		{ }
	}

	/// <summary>
	/// ビットマップハンドルを管理。
	/// </summary>
	public class UnmanagedBitmap: UnmanagedGDIObject
	{
		public UnmanagedBitmap(IntPtr hBitmap)
			: base(hBitmap)
		{ }

		/// <summary>
		/// ビットマップハンドルを元にマネージドリソースであるビットマップオブジェクトを生成。
		/// </summary>
		/// <returns>ビットマップ。UnmanagedIconの管轄外になるので後始末が必要なことに注意。</returns>
		public Bitmap ToManagedBitmap()
		{
			return Image.FromHbitmap(Handle);
		}
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

		public UnmanagedIcon(IntPtr hIcon)
			: base(hIcon)
		{ }

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

