using System;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi
{
    public class IconHandleWrapper: GdiBase
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

        #region GdiBase

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
}
