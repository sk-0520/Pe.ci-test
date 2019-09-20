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
using Forms = System.Windows.Forms;
using Wpf = System.Windows.Input;
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

        IReadOnlyDictionary<Forms.Keys, Wpf.ModifierKeys> modKeyMap = new Dictionary<Forms.Keys, Wpf.ModifierKeys>() {
            { Forms.Keys.Shift, Wpf.ModifierKeys.Shift },
            { Forms.Keys.Alt, Wpf.ModifierKeys.Alt},
            { Forms.Keys.Control, Wpf.ModifierKeys.Control },
            { Forms.Keys.LWin, Wpf.ModifierKeys.Windows },
            { Forms.Keys.RWin, Wpf.ModifierKeys.Windows },
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

        Wpf.Key ConvertWpfKeyFromFromsKey(int keyValue)
        {
            return Wpf.KeyInterop.KeyFromVirtualKey(keyValue);
        }

        Wpf.ModifierKeys ConvertWpfModKeyFromFromsModKey(Forms.Keys keyValue)
        {
            var result = modKeyMap
                .Where(p => p.Key == keyValue)
                .Select(p => p.Value)
                .Aggregate(Wpf.ModifierKeys.None, (k, a) => k | a)
            ;

            return result;
        }

        void OnKeyCore(object sender, KeyEventArgsExt e)
        {
            if(e.IsKeyDown) {
                if(KeyDown == null) {
                    return;
                }
            } else if(e.IsKeyUp) {
                if(KeyUp == null) {
                    return;
                }
            }

            var wpfKey = ConvertWpfKeyFromFromsKey(e.KeyValue);
            var wpfModKey = ConvertWpfModKeyFromFromsModKey(e.Modifiers);
            var timestamp = TimeSpan.FromMilliseconds(e.Timestamp);

            var usingEvent = new MouseKeyHookKeyEventArgs(sender, wpfKey, wpfModKey, e.IsKeyDown, timestamp);
            
            if(e.IsKeyDown) {
                KeyDown(sender, usingEvent);
            } else if(e.IsKeyUp){
                KeyUp(sender, usingEvent);
            }

            e.Handled = usingEvent.Handled;
        }

        private void OnKeyDown(object sender, KeyEventArgsExt e)
        {
            OnKeyCore(sender, e);
        }

        private void OnKeyUp(object sender, KeyEventArgsExt e)
        {
            OnKeyCore(sender, e);
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

        private void HookEvent_KeyDown(object sender, Forms.KeyEventArgs e)
        {
            OnKeyDown(sender, (KeyEventArgsExt)e);
        }

        private void HookEvent_KeyUp(object sender, Forms.KeyEventArgs e)
        {
            OnKeyUp(sender, (KeyEventArgsExt)e);
        }
    }
}
