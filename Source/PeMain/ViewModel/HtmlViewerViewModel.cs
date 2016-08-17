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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Data.Model;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class HtmlViewerViewModel: HasViewSingleModelWrapperViewModelBase<HtmlViewerModel, HtmlViewerWindow>, IHasAppNonProcess
    {
        #region variable

        string _windowTitle;

        #endregion

        public HtmlViewerViewModel(HtmlViewerModel model, HtmlViewerWindow view, IAppNonProcess appNonProcess)
            : base(model, view)
        {
            AppNonProcess = appNonProcess;
            WindowTitle = AppNonProcess.Language[Model.TitleLanguageKey];
            View.WindowStartupLocation = Model.WindowStartupLocation;
            View.Width = Model.WindowWidth;
            View.Height = Model.WindowHeight;
        }

        #region property

        public string WindowTitle {
            get { return this._windowTitle; }
            set { SetVariableValue(ref this._windowTitle, value); }
        }

        public double WindowWidth
        {
            get { return Model.WindowWidth; }
            set { SetModelValue(value); }
        }

        public double WindowHeight
        {
            get { return Model.WindowHeight; }
            set { SetModelValue(value); }
        }

        public bool Topmost
        {
            get { return Model.Topmost; }
            set { SetModelValue(value); }
        }

        public WindowStyle WindowStyle
        {
            get { return Model.WindowStyle; }
            set { SetModelValue(value); }
        }

        public WindowState WindowState
        {
            get { return Model.WindowState; }
            set { SetModelValue(value); }
        }

        public bool IgnoreScriptError
        {
            get { return Model.IgnoreScriptError; }
        }

        public bool UsingSystemContextMenu
        {
            get { return Model.UsingSystemContextMenu; }
        }

        #endregion

        #region function

        public void InitializeHtmlViewer(WebBrowser browser)
        {
            var map = HtmlViewerUtility.CreateBaseDictionary(AppNonProcess.Language, Model.CustomStylesheet, Model.CustomScript);
            foreach(var pair in Model.ReplaceKeys) {
                map[pair.Key] = pair.Value;
            }
            var timestamp = DateTime.Now;

            // TODO: 二重処理何とかならんか
            var replacedLanguage = Model.HtmlSource.ReplaceRange("@{", "}", key => AppNonProcess.Language.GetReplacedWordText(key, timestamp, null).Replace(Constants.HtmlViewerTagReplaceBreak, "<br />"));
            var replacedSysyem = AppNonProcess.Language.GetReplacedWordText(replacedLanguage, timestamp, map);
            
            browser.NavigateToString(replacedSysyem);
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion
    }
}
