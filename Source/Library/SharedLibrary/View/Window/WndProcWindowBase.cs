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
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Window
{
    /// <summary>
    /// Windowsのウィンドウプロシージャを持つWindow。
    /// </summary>
    public abstract class WndProcWindowBase: WindowsAPIWindowBase
    {
        #region variable

        HwndSource _hWndSource;

        #endregion

        public WndProcWindowBase()
            : base()
        { }

        #region property

        public bool IsHandleCreated { get; set; }

        protected HwndSource HandleSource
        {
            get
            {
                if(this._hWndSource == null) {
                    this._hWndSource = HwndSource.FromHwnd(Handle);
                }

                return this._hWndSource;
            }
        }

        #endregion

        #region function

        protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }

        #endregion

        #region WindowsAPIWindowBase

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HandleSource.AddHook(WndProc);
            Closed += WndProcWindowBase_Closed;

            IsHandleCreated = true;
        }

        #endregion

        void WndProcWindowBase_Closed(object sender, EventArgs e)
        {
            Closed -= WndProcWindowBase_Closed;
            HandleSource.RemoveHook(WndProc);
        }
    }
}
