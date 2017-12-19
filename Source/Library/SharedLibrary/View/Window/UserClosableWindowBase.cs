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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Window
{
    /// <summary>
    /// × とか Alt + F4 で閉じたことを検知できる Window。
    /// </summary>
    public abstract class UserClosableWindowWindowBase: WndProcWindowBase, IUserClosableWindow
    {
        #region event

        public event CancelEventHandler UserClosing = delegate { };

        #endregion

        public UserClosableWindowWindowBase()
            : base()
        { }

        #region function

        protected virtual void OnUserClosing(CancelEventArgs e)
        {
            UserClosing(this, e);
        }

        #endregion

        #region WndProcWindowBase

        protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == (int)WM.WM_SYSCOMMAND) {
                if(WindowsUtility.ConvertSCFromWParam(wParam) == SC.SC_CLOSE) {
                    var e = new CancelEventArgs(false);
                    OnUserClosing(e);
                    if(e.Cancel) {
                        handled = true;
                    }
                }
            }
            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion

        #region IUserClosableWindow

        public void UserClose()
        {
            var e = new CancelEventArgs(false);
            OnUserClosing(e);
            if(!e.Cancel) {
                Close();
            }
        }

        #endregion
    }
}
