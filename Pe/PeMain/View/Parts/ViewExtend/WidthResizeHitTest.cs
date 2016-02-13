/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
using ContentTypeTextNet.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend
{
    public class WidthResizeHitTest: CaptionCursorHitTest
    {
        public WidthResizeHitTest(Window view, IWindowHitTestData restrictionViewModel, INonProcess nonProcess)
            : base(view, restrictionViewModel, nonProcess)
        { }

        #region CaptionCursorHitTest

        public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var result = base.WndProc(hWnd, msg, wParam, lParam, ref handled);
            if(HitCaption && handled) {
                return result;
            }

            //var result = base.WndProc(hWnd, msg, wParam, lParam, ref handled);
            if(RestrictionViewModel.UsingBorderHitTest && handled && result != IntPtr.Zero) {
                var hitTest = (HT)result.ToInt32();
                var map = new Dictionary<HT, HT>() {
                    { HT.HTTOP, HT.HTNOWHERE },
                    { HT.HTBOTTOM, HT.HTNOWHERE },

                    { HT.HTTOPLEFT, HT.HTLEFT },
                    { HT.HTBOTTOMLEFT, HT.HTLEFT },

                    { HT.HTTOPRIGHT, HT.HTRIGHT },
                    { HT.HTBOTTOMRIGHT, HT.HTRIGHT },
                };
                HT resultHitTest;
                if(map.TryGetValue(hitTest, out resultHitTest)) {
                    return new IntPtr((int)resultHitTest);
                }
            }

            return result;
        }

        #endregion
    }
}
