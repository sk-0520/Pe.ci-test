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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class AboutViewModel: HasViewModelBase<AboutWindow>, IHasAppNonProcess
    {
        #region define

        static string separator = "____________";

        #endregion

        public AboutViewModel(AboutWindow view, AboutNotifyData notify, IAppNonProcess appNonProcess)
            : base(view)
        {
            CheckUtility.DebugEnforceNotNull(notify);

            Notify = notify;
            AppNonProcess = appNonProcess;

            var componentFilePath = Path.Combine(Constants.ApplicationDocumentDirectoryPath, Constants.componentListFileNam);
            ComponentCollection = SerializeUtility.LoadXmlSerializeFromFile<ComponentItemCollectionModel>(componentFilePath);
        }

        #region property

        AboutNotifyData Notify { get; set; }
        ComponentItemCollectionModel ComponentCollection { get; set; }
        public IEnumerable<ComponentItemViewModel> ComponentItems
        {
            get
            {
                return ComponentCollection.Select(i => new ComponentItemViewModel(i, AppNonProcess));
            }
        }

        #endregion

        #region command

        public ICommand OpenApplicationDirectoryCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ExecuteUtility.OpenDirectory(Constants.applicationRootDirectoryPath, AppNonProcess, null);
                    }
                );

                return result;
            }
        }

        public ICommand OpenUserSettingDirectoryCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var path = Environment.ExpandEnvironmentVariables(AppNonProcess.VariableConstants.UserSettingDirectoryPath);
                        if(!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                        }
                        ExecuteUtility.OpenDirectory(path, AppNonProcess, null);
                    }
                );

                return result;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var messageResult = MessageBox.Show(
                            AppNonProcess.Language["about/update/dialog/message"],
                            AppNonProcess.Language["about/update/dialog/caption"],
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information
                        );
                        if(messageResult == MessageBoxResult.Yes) {
                            Notify.CheckUpdate = true;

                            CloseView();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        CloseView();
                    }
                );

                return result;
            }
        }

        public ICommand OpenLinkCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var command = (string)o;
                        try {
                            if(command.Any(c => c == '@')) {
                                var mail = "mailto:" + command;
                                ExecuteUtility.ExecuteCommand(mail, AppNonProcess);
                            } else {
                                ExecuteUtility.ExecuteCommand(command, AppNonProcess);
                            }
                        } catch(Exception ex) {
                            AppNonProcess.Logger.Error(ex);
                        }
                    }
                );

                return result;
            }
        }

        public ICommand CopyLinkCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var copyKind = (AboutCopyKind)o;

                        switch(copyKind) {
                            case AboutCopyKind.Short:
                                {
                                    var list = new List<string>();
                                    list.Add("Software: " + Constants.ApplicationName);
                                    list.Add("Version: " + Constants.ApplicationVersion);
                                    list.Add("BuildType: " + Constants.BuildType);
                                    list.Add("Process: " + Constants.BuildProcess);
                                    list.Add("Platform: " + (Environment.Is64BitOperatingSystem ? "64" : "32"));
                                    list.Add("OS: " + System.Environment.OSVersion);
                                    list.Add("CLR: " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());
                                    var text = Environment.NewLine + separator + Environment.NewLine + string.Join(Environment.NewLine, list.Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine;
                                    ClipboardUtility.CopyText(text, AppNonProcess.ClipboardWatcher);
                                }
                                break;

                            case AboutCopyKind.Long:
                                {
                                    var appInfo = new AppInformationCollection();
                                    var text
                                        = Environment.NewLine
                                        + separator
                                        + Environment.NewLine
                                        + string.Join(
                                            Environment.NewLine,
                                            appInfo.ToString()
                                                .SplitLines()
                                                .Select(s => "    " + s)
                                        )
                                        + Environment.NewLine
                                        + Environment.NewLine
                                    ;
                                    ClipboardUtility.CopyText(text, AppNonProcess.ClipboardWatcher);
                                }
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                );

                return result;
            }
        }

        //public ICommand OpenComponentCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //			}
        //		);

        //		return result;
        //	}
        //}

        #endregion

        #region function

        void CloseView()
        {
            if(HasView) {
                View.Close();
            }
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion
    }
}
