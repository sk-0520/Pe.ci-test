using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        const string personalizeEnableTransparencyName = @"EnableTransparency";

        const string dwmKey = @"SOFTWARE\Microsoft\Windows\DWM";
        const string dwmColorizationColor = "ColorizationColor";
        const string dwmAccentColor = "AccentColor";

        const string accentKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent";
        const string accentStartColorMenuName = @"StartColorMenu";

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

        Color ConvertFromABGR(uint rawColor)
        {
            var a = (byte)((rawColor & 0xff000000) >> 24);
            var b = (byte)((rawColor & 0x00ff0000) >> 16);
            var g = (byte)((rawColor & 0x0000ff00) >> 8);
            var r = (byte)((rawColor & 0x000000ff) >> 0);

            return Color.FromArgb(a, r, g, b);
        }

        Color GetColor(RegistryKey key, string name, Func<uint, Color> converter)
        {
            var rawValue = key.GetValue(name);
            if(rawValue != null) {
                var rawColor = (uint)(int)rawValue;
                var color= converter(rawColor);
                NonProcess.Logger.Trace($"{name}: {color}");
                return color;
            }

            return Colors.Transparent;
        }

        private void SetColor(Color color)
        {
            RestrictionViewModel.VisualAlphaColor = color;
            RestrictionViewModel.VisualPlainColor = Color.FromRgb(color.R, color.G, color.B);
        }

        bool SetWindowColorWindows10()
        {
            using(var personalize = Registry.CurrentUser.OpenSubKey(personalizeKey))
            //using(var accent = Registry.CurrentUser.OpenSubKey(accentKey))
            using(var dwm = Registry.CurrentUser.OpenSubKey(dwmKey)) {

                var rawPersonalizeColorPrevalence = personalize.GetValue(personalizeColorPrevalenceName);
                if(rawPersonalizeColorPrevalence == null) {
                    return false;
                }
                var personalizeColorPrevalence = (uint)(int)rawPersonalizeColorPrevalence;
                if(personalizeColorPrevalence == 1 || true) { // 強制遷移
                    //// タスクバーに色を付ける
                    //var specialColor = GetColor(personalize, personalizeSpecialColorName, ConvertFromABGR);

                    // DWMアクセントカラー
                    var accentColor = GetColor(dwm, dwmAccentColor, ConvertFromABGR);
                    var colorizationColor = GetColor(dwm, dwmColorizationColor, MediaUtility.ConvertColorFromRawColor);

                    if(colorizationColor.A == 0) {
                        SetColor(accentColor);
                        return true;
                    }

                    //// エクスプローラアクセントカラー
                    //var accentStartColorMenu = GetColor(accent, accentStartColorMenuName, MediaUtility.ConvertColorFromRawColor);

                    SetColor(colorizationColor);

                    return true;
                } else {
                    // タスクバーに色を付けない
                }
            }

            return false;
        }

        void SetBrush()
        {
            View.Background = new SolidColorBrush(RestrictionViewModel.VisualAlphaColor);
            //View.Background = SystemParameters.WindowGlassBrush;
        }

        #endregion

        #region VisualStyle

        protected override void SetWindowColor()
        {
            if(IsWindows10) {
                if(SetWindowColorWindows10()) {
                    SetBrush();
                    return;
                }
            }

            base.SetWindowColor();
        }

        public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(RestrictionViewModel.UsingVisualStyle) {
                switch(msg) {
                    case (int)WM.WM_DWMCOLORIZATIONCOLORCHANGED:
                        if(IsWindows10) {
                            var color = MediaUtility.ConvertColorFromRawColor((uint)wParam);
                            SetColor(color);
                            SetBrush();
                        } else {
                            base.SetWindowColor();
                        }
                        break;
                    case (int)WM.WM_SETTINGCHANGE:
                    case (int)WM.WM_DWMCOMPOSITIONCHANGED:
                        SetWindowColor();
                        break;
                }
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}
