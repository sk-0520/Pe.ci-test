namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Drawing;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	/// <summary>
	/// アンマネージドオブジェクトを管理してくれそうな人。
	/// </summary>
	public abstract class UnmanagedBase: DisposeFinalizer
	{ }

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
		/// <summary>
		/// デバイスコンテキスト割り当てオブジェクト戻し用。
		/// 
		/// Dispose or Rollbackで戻しを実行する。
		/// Dispose は解放ではなく Rollback を発火するためだけに存在する。
		/// ファイナライザは実装しないのでGC回収時に戻しが行われることはない。
		/// 
		/// 戻しが不要ならそのまま破棄するか IsRollbackTarget を偽に設定する。
		/// </summary>
		public class SelectedObject: IDisposable
		{
			protected internal SelectedObject(UnmanagedDeviceContextBase hDC, IntPtr handle)
			{
				DC = hDC;
				SelectedHandle = handle;
				IsRollbackTarget = true;
			}

			public UnmanagedDeviceContextBase DC { get; private set; }
			public IntPtr SelectedHandle { get; private set; }

			public bool IsRollbackTarget { get; set; }

			#region IDisposable

			protected void Dispose(bool disposing)
			{
				Rollback();
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			#endregion

			public void Rollback()
			{
				if(IsRollbackTarget) {
					NativeMethods.SelectObject(DC.Handle, SelectedHandle);
					IsRollbackTarget = false;
				}
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

		public Graphics CreateGraphics()
		{
			return Graphics.FromHdc(Handle);
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

	/// <summary>
	/// Control紐付けデバイスコンテキスト。
	/// </summary>
	public class UnmanagedControlDeviceContext: UnmanagedDeviceContextBase
	{
		public UnmanagedControlDeviceContext(Control control)
			: base(NativeMethods.GetWindowDC(control.Handle))
		{
			Control = control;
		}

		public Control Control { get; private set; }

		protected override void ReleaseHandle()
		{
			if(Control != null) {
				NativeMethods.ReleaseDC(Control.Handle, Handle);
				Control = null;
			}
		}
	}

	/// <summary>
	/// フォントハンドルを管理。
	/// </summary>
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
	/// <summary>
	/// 生のCOMを管理。
	/// </summary>
	public abstract class UnmanagedRawComWrapper: UnmanagedBase
	{
		public UnmanagedRawComWrapper(object rawComObject)
			: base()
		{
			if(rawComObject == null) {
				throw new ArgumentNullException("rawComObject");
			}

			RawComObject = rawComObject;
		}

		/// <summary>
		/// COMオブジェクト。
		/// </summary>
		public object RawComObject { get; private set; }


		#region UnmanagedBase

		protected override void Dispose(bool disposing)
		{
			Marshal.ReleaseComObject(RawComObject);
			RawComObject = null;

			base.Dispose(disposing);
		}

		#endregion
	}

	/// <summary>
	/// 何かしらのCOMを管理。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UnmanagedComWrapper<T>: UnmanagedRawComWrapper
	{
		public UnmanagedComWrapper(T com)
			: base(com)
		{
			Com = com;
		}

		public T Com { get; private set; }
	}
}

