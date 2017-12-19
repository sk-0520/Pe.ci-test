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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    public class WeakEventListener<TWeakEventManager, TEventArgs>: IWeakEventListener
        where TWeakEventManager : WeakEventManager
        where TEventArgs : EventArgs
    {
        public WeakEventListener(EventHandler<TEventArgs> handler)
        {
            CheckUtility.EnforceNotNull(handler);
            Handler = handler;
        }

        #region property

        protected EventHandler<TEventArgs> Handler { get; private set; }

        #endregion

        #region IWeakEventListener

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if(managerType == typeof(TWeakEventManager)) {
                return CastUtility.AsFunc<TEventArgs, bool>(
                    e,
                    te => {
                        Handler(sender, te);
                        return true;
                    },
                    false
                );
            }

            return false;
        }

        #endregion

    }
}
