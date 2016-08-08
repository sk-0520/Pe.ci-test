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
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.Pe.PeMain.View
{
    /// <summary>
    /// NoteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NoteWindow: ViewModelCommonDataWindow<NoteViewModel>, IHasWindowKind
    {
        public NoteWindow()
        {
            InitializeComponent();
        }

        #region property

        CaptionDoubleClick CaptionDoubleClick { get; set; }
        WindowHitTest WindowHitTest { get; set; }
        WindowAreaCorrection WindowAreaCorrection { get; set; }

        #endregion

        #region ViewModelCommonDataWindow

        protected override void CreateViewModel()
        {
            var model = (NoteIndexItemModel)ExtensionData;
            ViewModel = new NoteViewModel(model, this, CommonData.NonProcess, CommonData.AppSender);
        }

        protected override void ApplyViewModel()
        {
            DataContext = ViewModel;

            base.ApplyViewModel();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            //int exStyle = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_EXSTYLE);
            //exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
            //WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

            //var style = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_STYLE);
            //style &= ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
            //WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_STYLE, (IntPtr)style); 
            UIUtility.SetStyleToolWindow(this, false, false);

            base.OnLoaded(sender, e);

            CaptionDoubleClick = new CaptionDoubleClick(this, ViewModel, CommonData.NonProcess);
            //WindowAreaCorrection = new WindowAreaCorrection(this, ViewModel, CommonData.NonProcess);
            WindowHitTest = new CaptionCursorHitTest(this, ViewModel, CommonData.NonProcess);
        }

        protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var extends = new IHasWndProc[] {
                WindowAreaCorrection,
                WindowHitTest,
                CaptionDoubleClick,
            };
            foreach(var extend in extends.Where(e => e != null)) {
                var result = extend.WndProc(hWnd, msg, wParam, lParam, ref handled);
                if(handled) {
                    return result;
                }
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        public override void ApplyLanguage(Dictionary<string, string> map)
        {
            map[LanguageKey.noteTitle] = ViewModel.Name;

            base.ApplyLanguage(map);
        }

        #endregion

        #region IHasWindowKind

        public WindowKind WindowKind { get { return WindowKind.Note; } }

        #endregion

        #region function

        //void ResetPopupPosition()
        //{
        //	//popup.HorizontalOffset += 1;
        //	//popup.HorizontalOffset -= 1;
        //}

        #endregion

        ///// <summary>
        ///// <para>http://stackoverflow.com/questions/5736359/popup-control-moves-with-parent</para>
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected override void OnLocationChanged(EventArgs e)
        //{
        //	ResetPopupPosition();
        //	base.OnLocationChanged(e);
        //}

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //	ResetPopupPosition();
        //	base.OnRenderSizeChanged(sizeInfo);
        //}
    }
}
