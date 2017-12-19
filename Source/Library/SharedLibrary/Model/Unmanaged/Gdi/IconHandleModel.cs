/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Gdi
{
    /// <summary>
    /// アイコンハンドルを管理。
    /// </summary>
    public class IconHandleModel: GdiObjectModelBase
    {
        public IconHandleModel(IntPtr hIcon)
            : base(hIcon)
        { }

        #region UnmanagedHandle

        protected override void ReleaseHandle()
        {
            NativeMethods.DestroyIcon(Handle);
            NativeMethods.SendMessage(Handle, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        #endregion

        #region GdiObjectModelBase

        public override bool CanMakeImageSource { get { return true; } }

        protected override BitmapSource MakeBitmapSourceImpl()
        {
            var result = Imaging.CreateBitmapSourceFromHIcon(
                Handle,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return result;
        }

        #endregion

    }

}
