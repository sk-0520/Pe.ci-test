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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Logic;
    using ContentTypeTextNet.Pe.PeMain.View;
    using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

    public class AcceptViewModel: HavingViewSingleModelWrapperViewModelBase<RunningInformationSettingModel, AcceptWindow>
    {
        public AcceptViewModel(RunningInformationSettingModel model, AcceptWindow view)
            : base(model, view)
        { }

        #region property

        public bool CheckUpdateRelease
        {
            get { return Model.CheckUpdateRelease; }
            set { Model.CheckUpdateRelease = value; }
        }

        public bool CheckUpdateRC
        {
            get { return Model.CheckUpdateRC; }
            set { Model.CheckUpdateRC = value; }
        }

        public bool Accept
        {
            get { return Model.Accept; }
            set { Model.Accept = value; }
        }

        public bool SendPersonalInformation
        {
            get { return Model.SendPersonalInformation; }
            set { Model.SendPersonalInformation = value; }
        }
        #endregion

        #region command


        public ICommand OkCommand
        {
            get
            {
                return CreateCommand(o => OnDailogCommand(true));
            }
        }

        public ICommand NgCommand
        {
            get
            {
                return CreateCommand(o => OnDailogCommand(false));
            }
        }

        #endregion

        #region function

        void OnDailogCommand(bool result)
        {
            Model.Accept = result;
            if(HasView) {
                View.DialogResult = result;
            }
        }

        public void SetAcceptDocument(WebBrowser browser, AppLanguageManager language, VariableConstants variableConstants)
        {
            var acceptSource = File.ReadAllText(language.AcceptDocumentFilePath);
            var acceptMap = new Dictionary<string, string>() {
                { LanguageKey.acceptWeb, Constants.UriAbout },
                { LanguageKey.acceptDevelopment, Constants.UriDevelopment },
                { LanguageKey.acceptMail, Constants.MailAbout },
                { LanguageKey.acceptForum, Constants.UriForum },
                { LanguageKey.acceptStyle, File.ReadAllText(Path.Combine(Constants.ApplicationStyleDirectoryPath, Constants.styleCommonFileName), Encoding.UTF8) },
                { LanguageKey.acceptApplicationName, Constants.ApplicationName },
                { LanguageKey.acceptOk, language.GuiTextToPlainText(language["accept/ok"]) },
                { LanguageKey.acceptNg, language.GuiTextToPlainText(language["accept/ng"]) },
                { LanguageKey.acceptRelease, language.GuiTextToPlainText(language["accept/update-check/release"]) },
                { LanguageKey.acceptRc, language.GuiTextToPlainText(language["accept/update-check/rc"]) },
                { LanguageKey.acceptSendUserInformation, language.GuiTextToPlainText(language["accept/send-user-info"]) },
            };
            var replacedAcceptSource = acceptSource.ReplaceRangeFromDictionary("${", "}", acceptMap);

            browser.NavigateToString(replacedAcceptSource);
        }

        #endregion
    }
}
