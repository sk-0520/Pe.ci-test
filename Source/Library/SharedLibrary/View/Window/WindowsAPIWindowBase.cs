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
    public abstract class WindowsAPIWindowBase: OnLoadedWindowBase, IWindowsHandle
    {
        public WindowsAPIWindowBase()
            : base()
        { }

        #region property

        protected WindowInteropHelper WindowInteropHelper { get; private set; }

        #region IWindowsHandle

        public IntPtr Handle
        {
            get
            {
                if(WindowInteropHelper == null) {
                    WindowInteropHelper = new WindowInteropHelper(this);
                }

                return WindowInteropHelper.Handle;
            }
        }

        #endregion

        #endregion

        #region OnLoadedWindowBase

        protected override void OnSourceInitialized(EventArgs e)
        {
            // うーん呼ばれない ;-(
            WindowInteropHelper = new WindowInteropHelper(this);

            base.OnSourceInitialized(e);
        }

        #endregion
    }
}
