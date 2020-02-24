using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public static class KeyExtensions
    {
        #region function

        public static bool IsModifierKey(this Key @this)
        {
            switch(@this) {
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LWin:
                case Key.RWin:
                    return true;

                default:
                    return false;
            }
        }

        #endregion
    }
}
