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
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend
{
    public class WindowHitTest: WindowsViewExtendBase<IWindowHitTestData>
    {
        public WindowHitTest(System.Windows.Window view, IWindowHitTestData restrictionViewModel, INonProcess nonProcess)
            : base(view, restrictionViewModel, nonProcess)
        { }

        #region WindowsViewExtendBase

        public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if((RestrictionViewModel.UsingBorderHitTest || RestrictionViewModel.UsingCaptionHitTest) && msg == (int)WM.WM_NCHITTEST) {
                var deviceScreenPoint = PodStructUtility.Convert(WindowsUtility.ConvertPOINTFromLParam(lParam));
                var logicalClientPoint = View.PointFromScreen(deviceScreenPoint);

                var hitTest = HT.HTNOWHERE;
                if(RestrictionViewModel.UsingCaptionHitTest && RestrictionViewModel.CaptionArea.Contains(logicalClientPoint)) {
                    hitTest = HT.HTCAPTION;
                } else if(RestrictionViewModel.UsingBorderHitTest) {
                    var hitState = new HitState();
                    hitState.CalculateAndSetValue(new Rect(0, 0, View.Width, View.Height), RestrictionViewModel.ResizeThickness, logicalClientPoint);
                    if(hitState.IsEnabled) {
                        if(hitState.Left) {
                            if(hitState.Top) {
                                hitTest = HT.HTTOPLEFT;
                            } else if(hitState.Bottom) {
                                hitTest = HT.HTBOTTOMLEFT;
                            } else {
                                hitTest = HT.HTLEFT;
                            }
                        } else if(hitState.Right) {
                            if(hitState.Top) {
                                hitTest = HT.HTTOPRIGHT;
                            } else if(hitState.Bottom) {
                                hitTest = HT.HTBOTTOMRIGHT;
                            } else {
                                hitTest = HT.HTRIGHT;
                            }
                        } else if(hitState.Top) {
                            hitTest = HT.HTTOP;
                        } else if(hitState.Bottom) {
                            hitTest = HT.HTBOTTOM;
                        }
                    }
                }

                if(hitTest != HT.HTNOWHERE) {
                    handled = true;
                    return (IntPtr)hitTest;
                }
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}
