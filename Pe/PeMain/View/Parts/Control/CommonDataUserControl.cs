/**
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
    public abstract class CommonDataUserControl: UserControl, ICommonData
    {
        public CommonDataUserControl()
        {
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        #region property

        //StartupWindowStatus Startup { get; set; }
        protected object ExtensionData { get; private set; }

        #endregion

        #region ICommonData

        public void SetCommonData(CommonData commonData, object extensionData)
        {
            CommonData = commonData;
            ExtensionData = extensionData;
        }

        public CommonData CommonData { get; private set; }

        #endregion

        #region function

        //protected virtual void ApplySetting()
        //{
        //	Debug.Assert(CommonData != null);

        //	ApplyViewModel();
        //	ApplyLanguage();
        //}

        //protected virtual void ApplyLanguage()
        //{
        //	Debug.Assert(CommonData != null);

        //	LanguageUtility.SetLanguage(this, CommonData.Language);
        //}

        //protected virtual void ApplyViewModel()
        //{
        //	Debug.Assert(CommonData != null);
        //}

        #endregion

        void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            this.Dispatcher.ShutdownStarted -= Dispatcher_ShutdownStarted;

            CommonData = null;
            ExtensionData = null;
        }

    }
}
