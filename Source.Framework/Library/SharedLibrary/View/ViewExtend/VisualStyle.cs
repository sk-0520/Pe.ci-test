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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend
{
    public class VisualStyle: WindowsViewExtendBase<IVisualStyleData>
    {
        public VisualStyle(System.Windows.Window view, IVisualStyleData restrictionViewModel, INonProcess nonProcess)
            : base(view, restrictionViewModel, nonProcess)
        {
            StockBackground = view.Background;

            SetStyle();
        }

        #region property

        Brush StockBackground { get; set; }
        bool UsingAeroGlass { get; set; }
        System.Windows.Window Window { get; set; }

        #endregion

        #region function

        static bool SupportAeroGlass()
        {
            var osVersion = Environment.OSVersion.Version;
            if(osVersion.Major == 6) {
                // vista, 7
                if(osVersion.Minor == 0 || osVersion.Minor == 1) {
                    return true;
                }
            }

            return false;
        }

        public void UnsetStyle()
        {
            if(RestrictionViewModel.UsingVisualStyle) {
                if(RestrictionViewModel.EnabledVisualStyle && UsingAeroGlass) {
                    var blurHehind = new DWM_BLURBEHIND();
                    blurHehind.fEnable = false;
                    //blurHehind.hRgnBlur = IntPtr.Zero;
                    blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;// | DWM_BB.DWM_BB_BLURREGION;
                    NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);

                    UsingAeroGlass = false;
                }
                View.Background = StockBackground;

                RestrictionViewModel.EnabledVisualStyle = false;
            }
        }

        public void SetStyle()
        {
            //if(RestrictionViewModel.EnabledVisualStyle) {
            //	var blurHehind = new DWM_BLURBEHIND();
            //	blurHehind.fEnable = false;
            //	//blurHehind.hRgnBlur = IntPtr.Zero;
            //	blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;// | DWM_BB.DWM_BB_BLURREGION;
            //	NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
            //	View.Background = StockBackground;
            //	WindowsUtility.Reload(Handle);
            //	//if(RestrictionViewModel.EnabledVisualStyle) {
            //	//	var margin = new MARGINS();
            //	//	margin.leftWidth = 0;
            //	//	margin.rightWidth = 0;
            //	//	margin.topHeight = 0;
            //	//	margin.bottomHeight = 0;
            //	//	NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margin); 

            //	//	DWM_BLURBEHIND blurHehind = new DWM_BLURBEHIND();
            //	//	blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;
            //	//	blurHehind.fEnable = false;

            //	//	NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
            //	//}
            //	return;
            //}
            if(RestrictionViewModel.UsingVisualStyle) {
                bool aeroSupport;
                NativeMethods.DwmIsCompositionEnabled(out aeroSupport);
                RestrictionViewModel.EnabledVisualStyle = aeroSupport;

                SetWindowStyle();
                SetWindowColor();
            }
        }

        protected void SetWindowStyle()
        {
            Debug.Assert(RestrictionViewModel.UsingVisualStyle);

            // AllowsTransparency="True" で aero glass の動作がもうほんとわけわかめ
            if(RestrictionViewModel.EnabledVisualStyle && /*SupportAeroGlass()*/ false) {
                if(!UsingAeroGlass) {
                    // Aero Glass

                    View.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
                    HwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;

                    //var margin = new MARGINS() {
                    //	leftWidth = -1,
                    //	rightWidth = -1,
                    //	topHeight = -1,
                    //	bottomHeight = -1,
                    //};
                    //NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margin); 

                    var blurHehind = new DWM_BLURBEHIND();
                    blurHehind.fEnable = true;
                    blurHehind.hRgnBlur = IntPtr.Zero;
                    blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE | DWM_BB.DWM_BB_BLURREGION;
                    NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);

                    //var DWMNCRP_DISABLED = DWMNCRP.DWMNCRP_DISABLED; 
                    //NativeMethods.DwmSetWindowAttribute(Handle, DWMWA.DWMWA_NCRENDERING_POLICY, ref DWMNCRP_DISABLED, sizeof(int));
                    //					var a = DWMNCRP.DWMNCRP_DISABLED; 
                    //					NativeMethods.DwmSetWindowAttribute(Handle, DWMWA.DWMWA_TRANSITIONS_FORCEDISABLED, ref a, sizeof(int));
                    //					NativeMethods.SetWindowThemeAttribute(hwnd, WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref options, WTA_OPTIONS.Size);

                    //NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margins);

                    View.MouseLeftButtonDown += (s1, e1) => {
                        ((System.Windows.Window)s1).DragMove();
                        e1.Handled = true;
                    };
                }
                UsingAeroGlass = true;
            }
            WindowsUtility.Reload(Handle);
        }

        protected virtual void SetWindowColor()
        {
            Debug.Assert(RestrictionViewModel.UsingVisualStyle);

            if(RestrictionViewModel.EnabledVisualStyle) {
                if(UsingAeroGlass) {
                    return;
                }

                // 色を取得
                //uint rawColor;
                //bool blend;
                //NativeMethods.DwmGetColorizationColor(out rawColor, out blend);
                ////VisualColor = Color.FromArgb((int)(rawColor & 0x00ffffff));
                //var a = (byte)((rawColor & 0xff000000) >> 24);
                //var r = (byte)((rawColor & 0x00ff0000) >> 16);
                //var g = (byte)((rawColor & 0x0000ff00) >> 8);
                //var b = (byte)((rawColor & 0x000000ff) >> 0);
                DWM_COLORIZATION_PARAMS colorization;
                NativeMethods.DwmGetColorizationParameters(out colorization);
                var rawColor = colorization.clrColor;
                var a = (byte)((rawColor & 0xff000000) >> 24);
                var r = (byte)((rawColor & 0x00ff0000) >> 16);
                var g = (byte)((rawColor & 0x0000ff00) >> 8);
                var b = (byte)((rawColor & 0x000000ff) >> 0);

                RestrictionViewModel.VisualAlphaColor = Color.FromArgb(a, r, g, b);
                RestrictionViewModel.VisualPlainColor = Color.FromRgb(r, g, b);

                View.Background = new SolidColorBrush(RestrictionViewModel.VisualAlphaColor);
            } else {
                View.Background = StockBackground;
            }
        }

        #endregion

        #region WindowsViewExtendBase

        public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(RestrictionViewModel.UsingVisualStyle) {
                switch(msg) {
                    case (int)WM.WM_DESTROY:
                        UnsetStyle();
                        break;

                    case (int)WM.WM_DWMCOMPOSITIONCHANGED:
                        SetStyle();
                        handled = true;
                        break;

                    case (int)WM.WM_DWMCOLORIZATIONCOLORCHANGED:
                        SetWindowColor();
                        handled = true;
                        break;


                    //case (int)WM.WM_NCHITTEST:
                    //	IntPtr result = new IntPtr();
                    //	handled = NativeMethods.DwmDefWindowProc(hwnd, msg, wParam, lParam, ref result);
                    //	handled = true;
                    //	break;

                    //case (int)WM.WM_WINDOWPOSCHANGED:
                    //	SetStyle();
                    //	break;

                    default:
                        break;
                }
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}
