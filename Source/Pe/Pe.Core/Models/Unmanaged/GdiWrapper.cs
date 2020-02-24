using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// Windows 万歳な GDI 系オブジェクトを扱う。
    /// </summary>
    public abstract class GdiObjectBase : UnmanagedHandleModelBase, IMakeBitmapSource
    {
        public GdiObjectBase(IntPtr hHandle)
            : base(hHandle)
        { }

        #region property

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

        #region UnmanagedHandleModelBase

        protected override void ReleaseHandle()
        {
            NativeMethods.DeleteObject(Raw);
        }

        #endregion
    }

    public class IconHandleWrapper : GdiObjectBase
    {
        public IconHandleWrapper(IntPtr hIcon)
            : base(hIcon)
        { }

        #region UnmanagedHandle

        protected override void ReleaseHandle()
        {
            NativeMethods.DestroyIcon(Raw);
            /* なんだったかなぁこれ
            NativeMethods.SendMessage(Raw, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            */
        }

        #endregion

        #region GdiObjectModelBase

        public override bool CanMakeImageSource => true;

        protected override BitmapSource MakeBitmapSourceCore()
        {
            var result = Imaging.CreateBitmapSourceFromHIcon(
                Raw,
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
    public class BitmapHandleWrapper : GdiObjectBase
    {
        public BitmapHandleWrapper(IntPtr hBitmap)
            : base(hBitmap)
        { }

        #region GdiObjectModelBase

        public override bool CanMakeImageSource => true;

        protected override BitmapSource MakeBitmapSourceCore()
        {
            var result = Imaging.CreateBitmapSourceFromHBitmap(
                Raw,
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return result;
        }

        #endregion
    }
}
