/**
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
using System.Windows.Controls.Primitives;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class PopupUtility
    {
        public static void Attachment(Window window, Popup popup)
        {
            EventDisposer<EventHandler> locationEvent;
            window.LocationChanged += EventUtility.Create<EventHandler>(
                (sender, e) => ResetPopupPosition(popup),
                releaseEvent => window.LocationChanged -= releaseEvent,
                out locationEvent
            );

            EventDisposer<SizeChangedEventHandler> sizeEvent;
            window.SizeChanged += EventUtility.Create<SizeChangedEventHandler>(
                (sender, e) => ResetPopupPosition(popup),
                releaseEvent => window.SizeChanged -= releaseEvent,
                out sizeEvent
            );

            EventDisposer<EventHandler> closeEvent = null;
            window.Closed += EventUtility.Create<EventHandler>(
                (sender, e) => {
                    locationEvent.Dispose();
                    sizeEvent.Dispose();
                    closeEvent.Dispose();

                    locationEvent = null;
                    sizeEvent = null;
                },
                releaseEvent => {
                    window.Closed -= releaseEvent;
                    closeEvent = null;
                },
                out closeEvent
            );
        }

        #region function

        static void ResetPopupPosition(Popup popup)
        {
            popup.HorizontalOffset += 1;
            popup.HorizontalOffset -= 1;
        }

        #endregion
    }
}
