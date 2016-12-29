using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend
{
    public class ThemeStyle: VisualStyle
    {
        #region define

        const string personalizeKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        const string personalizeColorPrevalenceName = @"ColorPrevalence";
        const string personalizeSpecialColorName = @"SpecialColor";

        const string dwmKey = @"SOFTWARE\Microsoft\Windows\DWM";
        const string dwmAccentColor = "ColorizationColor";

        const string accentKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent";
        const string accentColorMenuName = @"StartColorMenu";

        #endregion

        public ThemeStyle(System.Windows.Window view, IVisualStyleData restrictionViewModel, INonProcess nonProcess)
            : base(view, restrictionViewModel, nonProcess)
        {
            var version = Environment.OSVersion;
            IsWindows10 = 10 <= version.Version.Major;
        }

        #region property

        public bool IsWindows10 { get; }

        #endregion

        #region function

        //Color ToColor(uint rawColor)
        //{
        //    var a = (byte)((rawColor & 0xff000000) >> 24);
        //    var r = (byte)((rawColor & 0x00ff0000) >> 16);
        //    var g = (byte)((rawColor & 0x0000ff00) >> 8);
        //    var b = (byte)((rawColor & 0x000000ff) >> 0);

        //    return Color.FromArgb(a, r, g, b);
        //}

        bool SetWindowColorWindows10()
        {
            using(var dwm = Registry.CurrentUser.OpenSubKey(dwmKey))
            using(var personalize = Registry.CurrentUser.OpenSubKey(personalizeKey))
            using(var accent = Registry.CurrentUser.OpenSubKey(accentKey)) {
                var rawColorPrevalence = personalize.GetValue(personalizeColorPrevalenceName);
                if(rawColorPrevalence == null) {
                    return false;
                }
                var colorPrevalence = (uint)(int)rawColorPrevalence;
                if(colorPrevalence == 1) {
                    // タスクバーに色を付ける
                    var rawSpecialColor = personalize.GetValue(personalizeSpecialColorName);
                    if(rawSpecialColor != null) {
                    var specialColor = (uint)(int)rawSpecialColor;
                        RestrictionViewModel.VisualAlphaColor = MediaUtility.ConvertColorFromRawColor((uint)specialColor);
                        RestrictionViewModel.VisualPlainColor = MediaUtility.ConvertColorFromRawColor((uint)(specialColor & 0x00ffffff));
                        return true;
                    }

                    var rawDwmAccentColor = dwm.GetValue(dwmAccentColor);
                    if(rawDwmAccentColor != null) {
                        var dwmAccentColorMenu = (uint)(int)rawDwmAccentColor;
                        RestrictionViewModel.VisualAlphaColor = MediaUtility.ConvertColorFromRawColor((uint)dwmAccentColorMenu);
                        RestrictionViewModel.VisualPlainColor = MediaUtility.ConvertColorFromRawColor((uint)(dwmAccentColorMenu & 0x00ffffff));
                        return true;
                    }

                    // エクスプローラアクセントカラー
                    var rawAccentColorMenu = accent.GetValue(accentColorMenuName);
                    if(rawAccentColorMenu != null) {
                        var accentColorMenu = (uint)(int)rawAccentColorMenu;
                        RestrictionViewModel.VisualAlphaColor = MediaUtility.ConvertColorFromRawColor((uint)accentColorMenu);
                        RestrictionViewModel.VisualPlainColor = MediaUtility.ConvertColorFromRawColor((uint)(accentColorMenu & 0x00ffffff));
                        return true;
                    }


                    return true;
                } else {
                    // タスクバーに色を付けない
                }
            }

            return false;
        }


        #endregion

        #region VisualStyle

        protected override void SetWindowColor()
        {
            if(IsWindows10) {
                if(SetWindowColorWindows10()) {
                    //View.Background = new SolidColorBrush(RestrictionViewModel.VisualAlphaColor);
                    View.Background = SystemParameters.WindowGlassBrush;
                    return;
                }
            }

            base.SetWindowColor();
        }

        public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(RestrictionViewModel.UsingVisualStyle) {
                if(msg == (int)WM.WM_SETTINGCHANGE) {
                    SetWindowColor();
                }
                
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}
