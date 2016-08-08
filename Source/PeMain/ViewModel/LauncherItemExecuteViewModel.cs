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
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class LauncherItemExecuteViewModel: LauncherItemSimpleViewModel, IHasView<LauncherItemExecuteWindow>
    {
        #region variable

        EnvironmentVariablesItemModel _environmentVariablesItem;
        string _option;
        string _workDirPath;
        bool _stdStreamOutput;
        bool _stdStreamInput;
        bool _admin;

        #endregion

        public LauncherItemExecuteViewModel(LauncherItemModel model, LauncherItemExecuteWindow view, ScreenModel screen, IAppNonProcess nonPorocess, IAppSender appSender)
            : base(model, nonPorocess, appSender)
        {
            View = view;
            Screen = screen;

            this._environmentVariablesItem = (EnvironmentVariablesItemModel)Model.EnvironmentVariables.DeepClone();
            EnvironmentVariables = new EnvironmentVariablesEditViewModel(this._environmentVariablesItem, AppNonProcess);

            this._option = Model.Option;
            this._workDirPath = Model.WorkDirectoryPath;
            this._stdStreamOutput = Model.StdStream.OutputWatch;
            this._stdStreamInput = Model.StdStream.InputUsing;

            if(HasView) {
                ScreenUtility.AttachmentStartupMoveScreenCenter(View, Screen);
                View.Dispatcher.BeginInvoke(new Action(() => {
                    View.selectOption.Focus();
                }), System.Windows.Threading.DispatcherPriority.SystemIdle);
            }

        }

        #region property

        ScreenModel Screen { get; set; }

        public EnvironmentVariablesEditViewModel EnvironmentVariables { get; set; }

        public override bool StdStreamOutput
        {
            get { return this._stdStreamOutput; }
            set { SetVariableValue(ref this._stdStreamOutput, value); }
        }

        public override bool StdStreamInput
        {
            get { return this._stdStreamInput; }
            set { SetVariableValue(ref this._stdStreamInput, value); }
        }

        public override bool Administrator
        {
            get { return this._admin; }
            set { SetVariableValue(ref this._admin, value); }
        }

        public override string Option
        {
            get { return this._option ?? string.Empty; }
            set { SetVariableValue(ref this._option, value); }
        }

        public override string WorkDirectoryPath
        {
            get { return this._workDirPath ?? string.Empty; }
            set { SetVariableValue(ref this._workDirPath, value); }
        }

        public IEnumerable<string> Options
        {
            get
            {
                var result = new List<string>(1 + Model.History.Options.Count);

                result.Add(Option);
                result.AddRange(Model.History.Options);

                return result;
            }
        }

        public IReadOnlyList<string> WorkDirectoryPaths
        {
            get
            {
                var result = new List<string>(1 + Model.History.WorkDirectoryPaths.Count);

                result.Add(WorkDirectoryPath);
                result.AddRange(Model.History.WorkDirectoryPaths);

                return result;
            }
        }

        #endregion

        #region command

        public ICommand RunCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dummyModel = (LauncherItemModel)Model.DeepClone();
                        dummyModel.Option = Option;
                        dummyModel.WorkDirectoryPath = WorkDirectoryPath;
                        dummyModel.StdStream.OutputWatch = StdStreamOutput;
                        dummyModel.StdStream.InputUsing = StdStreamInput;
                        dummyModel.Administrator = Administrator;
                        dummyModel.EnvironmentVariables = this._environmentVariablesItem;
                        try {
                            ExecuteUtility.RunItem(dummyModel, Screen, AppNonProcess, AppSender);
                            SettingUtility.IncrementLauncherItem(Model, Option, WorkDirectoryPath, AppNonProcess);
                        } catch(Exception ex) {
                            AppNonProcess.Logger.Warning(ex);
                        }

                        if(HasView) {
                            View.Close();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Close();
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

        public ICommand SelectWorkDirectoryDirectoryCommand
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

        #endregion

        #region function

        /// <summary>
        /// 外部からデータを設定する。
        /// </summary>
        /// <param name="path"></param>
        public void SetFile(string path)
        { }

        #endregion

        #region IHasView

        public LauncherItemExecuteWindow View { get; private set; }

        public bool HasView { get { return HasViewUtility.GetHasView(this); } }

        #endregion
    }
}
