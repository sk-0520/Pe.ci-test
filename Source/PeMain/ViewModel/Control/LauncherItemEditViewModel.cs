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
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
    public class LauncherItemEditViewModel: LauncherItemSimpleViewModel
    {
        #region define

        void NullRefresh()
        { }

        #endregion

        public LauncherItemEditViewModel(LauncherItemModel model, Action refreshText, Action refreshImage, IAppNonProcess nonPorocess, IAppSender appSender)
            : base(model, nonPorocess, appSender)
        {
            RefreshText = refreshText ?? NullRefresh;
            RefreshImage = refreshImage ?? NullRefresh;
        }

        #region property

        public Action RefreshText { get; private set; }
        public Action RefreshImage { get; private set; }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                    RefreshText();
                }
            }
        }

        public override LauncherKind LauncherKind
        {
            get { return base.LauncherKind; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                    RefreshImage();
                }
            }
        }

        public override string Command
        {
            get { return base.Command; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                    RefreshImage();
                }
            }
        }

        public string IconDisplayText
        {
            get { return Model.Icon.DisplayText; }
        }

        public EnvironmentVariablesEditViewModel EnvironmentVariables
        {
            get { return new EnvironmentVariablesEditViewModel(Model.EnvironmentVariables, AppNonProcess); }
        }

        #endregion

        #region command

        public ICommand SelectCommandFileCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dialogResult = LauncherItemUtility.ShowOpenCommandDialog(Command, AppNonProcess);
                        if(dialogResult != null) {
                            Command = dialogResult;
                        }
                    }
                );

                return result;
            }
        }

        public ICommand SelectCommandDirectoryCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dialogResult = DialogUtility.ShowDirectoryDialog(Command);
                        if(dialogResult != null) {
                            Command = dialogResult;
                        }
                    }
                );

                return result;
            }
        }

        public ICommand SelectOptionFilesCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var files = LauncherItemUtility.ShowOpenOptionDialog(Option);
                        if(files != null) {
                            Option = files;
                        }
                    }
                );

                return result;
            }
        }

        public ICommand SelectOptionDirectoryCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dialogResult = DialogUtility.ShowDirectoryDialog(Option);
                        if(dialogResult != null) {
                            Option = dialogResult;
                        }
                    }
                );

                return result;
            }
        }

        public ICommand SelectWorkDirectoryCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dialogResult = DialogUtility.ShowDirectoryDialog(WorkDirectoryPath);
                        if(dialogResult != null) {
                            WorkDirectoryPath = dialogResult;
                        }
                    }
                );

                return result;
            }
        }

        public ICommand SelectIconCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dialog = new IconDialog();
                        dialog.Icon.Path = Environment.ExpandEnvironmentVariables(Icon.Path ?? string.Empty);
                        dialog.Icon.Index = Icon.Index;
                        var dialogResult = dialog.ShowDialog();
                        if(dialogResult.GetValueOrDefault()) {
                            Icon.Path = dialog.Icon.Path;
                            Icon.Index = dialog.Icon.Index;

                            CallOnPropertyChange(nameof(IconDisplayText));
                            RefreshImage();
                        }
                    }
                );

                return result;
            }
        }


        #endregion
    }
}
