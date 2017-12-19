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
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend
{
    /// <summary>
    /// ウィンドウとかに何かしら機能拡張する実装。
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class WindowsViewExtendBase<TViewModel>: DisposeFinalizeBase, IHasWndProc
        where TViewModel : class, IWindowsViewExtendRestrictionViewModelMarker
    {
        public WindowsViewExtendBase(System.Windows.Window view, TViewModel restrictionViewModel, INonProcess nonProcess)
            : base()
        {
            CheckUtility.EnforceNotNull(view);
            CheckUtility.EnforceNotNull(restrictionViewModel);
            CheckUtility.Enforce(view.IsLoaded);

            View = view;
            RestrictionViewModel = restrictionViewModel;
            NonProcess = nonProcess;

            Handle = HandleUtility.GetWindowHandle(View);
            HwndSource = HwndSource.FromHwnd(Handle);
        }

        #region property

        protected TViewModel RestrictionViewModel { get; private set; }
        protected System.Windows.Window View { get; private set; }
        protected INonProcess NonProcess { get; private set; }
        protected IntPtr Handle { get; private set; }
        protected HwndSource HwndSource { get; private set; }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(HwndSource != null) {
                    HwndSource.Dispose();
                    HwndSource = null;
                }
                Handle = IntPtr.Zero;
                RestrictionViewModel = null;
                View = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region IHavingWndProc

        public virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }

        #endregion
    }
}
