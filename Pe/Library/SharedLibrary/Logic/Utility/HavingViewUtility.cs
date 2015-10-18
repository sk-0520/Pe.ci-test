﻿/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using ContentTypeTextNet.Library.SharedLibrary.IF;

    public static class HavingViewUtility
    {
        public static bool GetHasView<TView>(IHavingView<TView> view)
            where TView : UIElement
        {
            return view.View != null;
        }

        public static DispatcherOperation BeginInvoke<TView>(IHavingView<TView> view, Action action, DispatcherPriority priority, params object[] args)
            where TView : UIElement
        {
            Func<Action, DispatcherPriority, object[], DispatcherOperation> beginInvoke;

            if(view.HasView) {
                beginInvoke = view.View.Dispatcher.BeginInvoke;
            } else {
                beginInvoke = Dispatcher.CurrentDispatcher.BeginInvoke;
            }

            return beginInvoke(action, priority, args);
        }

        public static DispatcherOperation BeginInvoke<TView>(IHavingView<TView> view, Action action, params object[] args)
            where TView : UIElement
        {
            Func<Action, object[], DispatcherOperation> beginInvoke;

            if(view.HasView) {
                beginInvoke = view.View.Dispatcher.BeginInvoke;
            } else {
                beginInvoke = Dispatcher.CurrentDispatcher.BeginInvoke;
            }

            return beginInvoke(action, args);
        }
    }
}
