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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.View;
using ContentTypeTextNet.Library.SharedLibrary.View.Window;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Window
{
    public abstract class CommonDataWindow: UserClosableWindowWindowBase, ICommonData
    {
        public CommonDataWindow()
            : base()
        {
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        #region property

        //StartupWindowStatus Startup { get; set; }
        protected object ExtensionData { get; private set; }

        #endregion

        #region function

        protected virtual void ApplySetting()
        {
            Debug.Assert(CommonData != null);

            this.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);

            CreateViewModel();
            ApplyViewModel();
        }

        protected virtual void CreateViewModel()
        { }

        public virtual void ApplyLanguage(Dictionary<string, string> map)
        {
            Debug.Assert(CommonData != null);

            LanguageUtility.RecursiveSetLanguage(this, CommonData.Language, map);
        }

        protected virtual void ApplyViewModel()
        {
            Debug.Assert(CommonData != null);
        }

        protected virtual void SetChildCommonData()
        {
            Debug.Assert(CommonData != null);

            foreach(var ui in UIUtility.FindLogicalChildren<System.Windows.Controls.Control>(this).OfType<ICommonData>()) {
                ui.SetCommonData(CommonData, ExtensionData);
            }
        }


        #endregion

        #region ICommonData

        public void SetCommonData(CommonData commonData, object extensionData)
        {
            CommonData = commonData;
            ExtensionData = extensionData;

            ApplySetting();
        }

        public CommonData CommonData { get; private set; }

        #endregion

        #region UserClosableWindowWindowBase

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);

            var map = new Dictionary<string, string>();
            ApplyLanguage(map);
            SetChildCommonData();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //CommonData = null;
            DataContext = null;
        }

        #endregion

        void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            this.Dispatcher.ShutdownStarted -= Dispatcher_ShutdownStarted;

            CommonData = null;
            DataContext = null;
        }

    }
}
