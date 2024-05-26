using System;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi
{
    /// <summary>
    /// ビットマップハンドルを管理。
    /// </summary>
    public class BitmapHandleWrapper: GdiBase
    {
        public BitmapHandleWrapper(IntPtr hBitmap)
            : base(hBitmap)
        { }

        #region GdiBase

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
