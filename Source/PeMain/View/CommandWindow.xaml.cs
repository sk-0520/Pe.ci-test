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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
using ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.Pe.PeMain.View
{
    /// <summary>
    /// CommandWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CommandWindow: ViewModelCommonDataWindow<CommandViewModel>
    {
        public CommandWindow()
        {
            InitializeComponent();
        }

        #region property

        CaptionCursorHitTest WindowHitTest { get; set; }
        VisualStyle VisualStyle { get; set; }

        #endregion

        #region ViewModelCommonDataWindow

        protected override void CreateViewModel()
        {
            ViewModel = new CommandViewModel(CommonData.MainSetting.Command, this, CommonData.LauncherItemSetting, CommonData.NonProcess, CommonData.AppSender);
        }

        protected override void ApplyViewModel()
        {
            base.ApplyViewModel();

            DataContext = ViewModel;
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            UIUtility.SetStyleToolWindow(this, false, false);

            base.OnLoaded(sender, e);

            WindowHitTest = new WidthResizeHitTest(this, ViewModel, CommonData.NonProcess);
            VisualStyle = new ThemeStyle(this, ViewModel, CommonData.NonProcess);
        }

        protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //if(Appbar != null) {
            //	Appbar.WndProc(hWnd, msg, wParam, lParam, ref handled);
            //}
            //if (VisualStyle != null) {
            //	VisualStyle.WndProc(hWnd, msg, wParam, lParam, ref handled);
            //}
            //if (WindowAreaCorrection != null) {
            //	WindowAreaCorrection.WndProc(hWnd, msg, wParam, lParam, ref handled);
            //}
            //if (handled) {
            //	return IntPtr.Zero;
            //}


            var extends = new IHasWndProc[] {
                VisualStyle,
                WindowHitTest,
            };
            foreach(var extend in extends.Where(e => e != null)) {
                var result = extend.WndProc(hWnd, msg, wParam, lParam, ref handled);
                if(handled) {
                    return result;
                }
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}
