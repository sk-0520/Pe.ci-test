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
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class ScreenViewModel: HasViewModelBase<ScreenWindow>, IHasAppNonProcess
    {
        #region variable

        Color _backColor;
        Brush _foreground;
        Brush _background;

        #endregion

        public ScreenViewModel(ScreenWindow view, ScreenModel screen, IAppNonProcess appNonProcess)
            : base(view)
        {
            Screen = screen;
            AppNonProcess = appNonProcess;

            byte alpha = 180;
            if(Screen.Primary) {
                this._backColor = Color.FromArgb(alpha, 0xff, 0xff, 0xff);
            } else {
                var rand = new Random(screen.DeviceName.GetHashCode());
                this._backColor = Color.FromArgb(
                    alpha,
                    (byte)rand.Next(0x00, 0xff),
                    (byte)rand.Next(0x00, 0xff),
                    (byte)rand.Next(0x00, 0xff)
                );
            }
            this._foreground = MakeBrush(MediaUtility.GetAutoColor(this._backColor));
            this._background = MakeBrush(this._backColor);
        }

        #region property

        ScreenModel Screen { get; set; }

        public Brush Foreground { get { return this._foreground; } }

        public Brush Background { get { return this._background; } }

        public string ScreenName
        {
            get { return ScreenUtility.GetScreenName(Screen); }
        }

        public string DeviceName
        {
            get { return Screen.DeviceName; }
        }

        public bool IsPrimaryScreen
        {
            get { return Screen.Primary; }
        }

        #endregion

        #region function

        Brush MakeBrush(Color color)
        {
            var brush = new SolidColorBrush() {
                Color = color,
            };
            FreezableUtility.SafeFreeze(brush);

            return brush;
        }

        #endregion

        #region HasViewModelBase

        protected override void InitializeView()
        {
            base.InitializeView();

            View.Loaded += View_Loaded;
        }

        protected override void UninitializeView()
        {
            View.Loaded -= View_Loaded;

            base.UninitializeView();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Screen = null;
                this._foreground = null;
                this._background = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var deviceArea = PodStructUtility.Convert(Screen.DeviceBounds);
            NativeMethods.MoveWindow(View.Handle, deviceArea.X, deviceArea.Y, deviceArea.Width, deviceArea.Height, true);
        }
    }
}
