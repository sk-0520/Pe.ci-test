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
using System.Windows.Forms;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using Gma.System.MouseKeyHook;

namespace ContentTypeTextNet.Pe.Library.FormsCushion.MouseKeyHookCompatibility
{
    public class MouseKeyHookCompatibility: DisposeFinalizeBase
    {
        #region event

        public event EventHandler<MouseKeyHookKeyEventArgs> KeyDown;
        public event EventHandler<MouseKeyHookKeyEventArgs> KeyUp;

        #endregion

        #region variable

        IReadOnlyDictionary<System.Windows.Forms.Keys, System.Windows.Input.ModifierKeys> modKeyMap = new Dictionary<System.Windows.Forms.Keys, System.Windows.Input.ModifierKeys>() {
            { System.Windows.Forms.Keys.Shift, System.Windows.Input.ModifierKeys.Shift },
            { System.Windows.Forms.Keys.Alt, System.Windows.Input.ModifierKeys.Alt},
            { System.Windows.Forms.Keys.Control, System.Windows.Input.ModifierKeys.Control },
            { System.Windows.Forms.Keys.LWin, System.Windows.Input.ModifierKeys.Windows },
            { System.Windows.Forms.Keys.RWin, System.Windows.Input.ModifierKeys.Windows },
        };

        #endregion

        public MouseKeyHookCompatibility()
        {
            HookEvent = Hook.GlobalEvents();

            HookEvent.KeyDown += HookEvent_KeyDown;
            HookEvent.KeyUp += HookEvent_KeyUp;
        }

        #region property

        IKeyboardMouseEvents HookEvent { get; }

        #endregion

        #region function

        System.Windows.Input.Key ConvertWpfKeyFromFromsKey(int keyValue)
        {
            return System.Windows.Input.KeyInterop.KeyFromVirtualKey(keyValue);
        }

        System.Windows.Input.ModifierKeys ConvertWpfModKeyFromFromsModKey(System.Windows.Forms.Keys keyValue)
        {
            var result = modKeyMap
                .Where(p => p.Key == keyValue)
                .Select(p => p.Value)
                .Aggregate(System.Windows.Input.ModifierKeys.None, (k, a) => k | a)
            ;

            return result;
        }

        void OnKeyCore(object sender, System.Windows.Forms.KeyEventArgs e, bool isDown)
        {
            var wpfKey = ConvertWpfKeyFromFromsKey(e.KeyValue);
            var wpfModKey = ConvertWpfModKeyFromFromsModKey(e.Modifiers);

            var usingEvent = new MouseKeyHookKeyEventArgs(sender, wpfKey, wpfModKey, isDown);

            if(isDown) {
                KeyDown(sender, usingEvent);
            } else {
                KeyUp(sender, usingEvent);
            }
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(KeyDown == null) {
                return;
            }

            OnKeyCore(sender, e, true);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if(KeyUp == null) {
                return;
            }

            OnKeyCore(sender, e, false);
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                HookEvent.KeyDown -= HookEvent_KeyDown;
                HookEvent.KeyUp -= HookEvent_KeyUp;
                HookEvent.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        private void HookEvent_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            OnKeyDown(sender, e);
        }

        private void HookEvent_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            OnKeyUp(sender, e);
        }
    }
}
