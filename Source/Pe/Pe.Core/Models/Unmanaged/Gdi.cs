using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// Windows 万歳な GDI 系オブジェクトを扱う。
    /// </summary>
    public abstract class Gdi: SafeHandle, IMakeBitmapSource
    {
        protected Gdi(IntPtr hHandle)
            : base(hHandle, true)
        { }

        #region property

        public IntPtr ResourceHandle => this.handle;

        public virtual bool CanMakeImageSource => false;

        #endregion

        #region function

        protected abstract BitmapSource MakeBitmapSourceCore();

        #endregion

        #region IMakeBitmapSource

        /// <summary>
        /// GDIオブジェクトから<see cref="BitmapSource"/>作成。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"><see cref="CanMakeImageSource"/></exception>
        public BitmapSource MakeBitmapSource()
        {
            if(!CanMakeImageSource) {
                throw new InvalidOperationException();
            }
            return MakeBitmapSourceCore();
        }

        #endregion

        #region SafeHandle

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            return NativeMethods.DeleteObject(this.handle);
        }

        #endregion
    }

    public class IconHandleWrapper: Gdi
    {
        public IconHandleWrapper(IntPtr hIcon)
            : base(hIcon)
        { }

        #region UnmanagedHandle

        protected override bool ReleaseHandle()
        {
            return NativeMethods.DestroyIcon(this.handle);
            /* なんだったかなぁこれ
            NativeMethods.SendMessage(Instance, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            */
        }

        #endregion

        #region GdiObjectModelBase

        public override bool CanMakeImageSource => true;

        protected override BitmapSource MakeBitmapSourceCore()
        {
            var result = Imaging.CreateBitmapSourceFromHIcon(
                this.handle,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return result;
        }

        #endregion
    }

    /// <summary>
    /// ビットマップハンドルを管理。
    /// </summary>
    public class BitmapHandleWrapper: Gdi
    {
        public BitmapHandleWrapper(IntPtr hBitmap)
            : base(hBitmap)
        { }

        #region GdiObjectModelBase

        public override bool CanMakeImageSource => true;

        protected override BitmapSource MakeBitmapSourceCore()
        {
            var result = Imaging.CreateBitmapSourceFromHBitmap(
                this.handle,
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return result;
        }

        #endregion
    }
}
