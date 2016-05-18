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
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Library.FormsCushion.MouseKeyHookCompatibility
{
    public class MouseKeyHookKeyEventArgs: EventArgs
    {
        public MouseKeyHookKeyEventArgs(object sender, Key key, ModifierKeys modifierKeys, bool isDown, TimeSpan timestamp)
        {
            Key = key;
            ModifierKeys = modifierKeys;
            IsDown = isDown;
            Timestamp = timestamp;
        }

        #region property

        public Key Key { get; }
        public ModifierKeys ModifierKeys { get; }
        public bool IsDown { get; }
        public TimeSpan Timestamp { get; }
        /// <summary>
        /// イベントが処理されたかどうかを示す値を取得または設定します。
        /// </summary>
        public bool Handled { get; set; }

        #endregion

    }
}
