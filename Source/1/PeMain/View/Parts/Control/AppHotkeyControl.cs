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
using ContentTypeTextNet.Library.SharedLibrary.View.Control;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
    public class AppHotkeyControl: HotkeyControl, ICommonData
    {
        public AppHotkeyControl()
        {
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        #region property

        protected object ExtensionData { get; private set; }

        #endregion

        #region ICommonData

        public void SetCommonData(CommonData commonData, object extensionData)
        {
            CommonData = commonData;
            ExtensionData = extensionData;

            SetText();
        }

        public CommonData CommonData { get; private set; }

        #endregion

        #region HotkeyControl

        protected override string GetDisplayModText(ModifierKeys mod)
        {
            if(CommonData != null) {
                return LanguageUtility.GetTextFromSingleModifierKey(mod, CommonData.Language);
            } else {
                return base.GetDisplayModText(mod);
            }
        }

        protected override string GetDisplayKeyText(Key key)
        {
            if(CommonData != null) {
                return LanguageUtility.GetTextFromSingleKey(key, CommonData.Language);
            } else {
                return base.GetDisplayKeyText(key);
            }
        }

        protected override string DisplayAddText
        {
            get
            {
                if(CommonData != null) {
                    return LanguageUtility.GetKeySeparatorText(CommonData.Language);
                } else {
                    return base.DisplayAddText;
                }
            }
        }

        #endregion

        void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            this.Dispatcher.ShutdownStarted -= Dispatcher_ShutdownStarted;

            CommonData = null;
            ExtensionData = null;
        }
    }
}
