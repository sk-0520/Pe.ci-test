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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class EventUtility
    {
        public static Delegate Create(Delegate handler, Action<Delegate> releaseEvent, out EventDisposer eventDisposer, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            eventDisposer = new EventDisposer();
            return eventDisposer.Handling(handler, releaseEvent, callerFile, callerLine, callerMember);
        }
        public static Delegate Auto(Delegate handler, Action<Delegate> releaseEvent, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            EventDisposer eventDisposer = null;
            var result = Create(handler, dg => {
                //releaseEvent(dg);
                eventDisposer.Dispose();
                eventDisposer = null;
            }, out eventDisposer, callerFile, callerLine, callerMember);
            return result;
        }

        public static TEventHandler Create<TEventHandler>(TEventHandler handler, Action<TEventHandler> releaseEvent, out EventDisposer<TEventHandler> eventDisposer, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
            where TEventHandler: class
        {
            eventDisposer = new EventDisposer<TEventHandler>();
            return eventDisposer.Handling(handler, releaseEvent, callerFile, callerLine, callerMember);
        }
        public static TEventHandler Auto<TEventHandler>(TEventHandler handler, Action<TEventHandler> releaseEvent, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
            where TEventHandler : class
        {
            EventDisposer<TEventHandler> eventDisposer = null;
            var result = Create(handler, e => {
                //releaseEvent(e);
                eventDisposer.Dispose();
                eventDisposer = null;
            }, out eventDisposer, callerFile, callerLine, callerMember);
            return result;
        }
    }
}
