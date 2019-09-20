using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms
{
    public static class KeyConvertUtility
    {
        public static Key ConvertKey(WinForms.Keys key)
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
