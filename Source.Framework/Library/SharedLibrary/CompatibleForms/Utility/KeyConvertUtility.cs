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
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using Forms = System.Windows.Forms;

namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility
{
    public static class KeyConvertUtility
    {
        public static Key ConvertKey(Forms.Keys key)
        {
            return KeyInterop.KeyFromVirtualKey((int)key);
        }

        public static ModifierKeys ConvertModifierKeys(MOD mod)
        {
            var map = new Dictionary<MOD, ModifierKeys>() {
                { MOD.MOD_ALT, ModifierKeys.Alt },
                { MOD.MOD_SHIFT, ModifierKeys.Shift },
                { MOD.MOD_CONTROL, ModifierKeys.Control },
                { MOD.MOD_WIN, ModifierKeys.Windows },
            };
            return map
                .Where(p => mod.HasFlag(p.Key))
                .Select(p => p.Value).
                Aggregate(ModifierKeys.None, (a, b) => a | b)
            ;
        }

    }
}
