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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class LauncherItemButtonViewModel: LauncherItemSimpleViewModel, ILauncherButton
    {
        #region varable

        IconScale _iconScale;

        bool _existsCommand;
        bool _existsParentDir;
        bool _existsWorkDir;

        bool _hasDataCommand;
        bool _hasDataParentDir;
        bool _hasDataWorkDir;

        bool _isMenuOpen;

        #endregion

        public LauncherItemButtonViewModel(LauncherItemModel model, ScreenModel dockScreen, LauncherItemSettingModel launcherItemSetting, IAppNonProcess nonPorocess, IAppSender appSender)
            : base(model, nonPorocess, appSender)
        {
            DockScreen = dockScreen;
            LauncherItemSetting = launcherItemSetting;
        }

        #region property

        protected ScreenModel DockScreen { get; set; }
        protected LauncherItemSettingModel LauncherItemSetting { get; private set; }

        public IconScale IconScale
        {
            get { return this._iconScale; }
            set
            {
                //if(this._iconScale != value) {
                //	this._iconScale = value;
                //	OnPropertyChanged();
                //	OnPropertyChanged("ToolbarImage");
                //}
                if(SetVariableValue(ref this._iconScale, value)) {
                    OnPropertyChanged(nameof(ToolbarImage));
                }
            }
        }

        public Visibility VisibilityFile { get { return ToVisibility(Model.LauncherKind == LauncherKind.File); } }
        //public Visibility VisibilityDirectory { get { return ToVisibility(Model.LauncherKind == LauncherKind.Directory); } }
        public Visibility VisibilityCommand { get { return ToVisibility(Model.LauncherKind == LauncherKind.Command); } }

        public bool ExistsCommand
        {
            get { return this._existsCommand; }
            set { SetVariableValue(ref this._existsCommand, value); }
        }
        public bool ExistsParentDirectory
        {
            get { return this._existsParentDir; }
            set { SetVariableValue(ref this._existsParentDir, value); }
        }
        public bool ExistsWorkDirectory
        {
            get { return this._existsWorkDir; }
            set { SetVariableValue(ref this._existsWorkDir, value); }
        }

        public bool HasDataCommand
        {
            get { return this._hasDataCommand; }
            set { SetVariableValue(ref this._hasDataCommand, value); }
        }
        public bool HasDataParentDirectory
        {
            get { return this._hasDataParentDir; }
            set { SetVariableValue(ref this._hasDataParentDir, value); }
        }
        public bool HasDataWorkDirectory
        {
            get { return this._hasDataWorkDir; }
            set { SetVariableValue(ref this._hasDataWorkDir, value); }
        }

        #endregion

        #region command

        public ICommand OpenDropDownCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        Application.Current.Dispatcher.BeginInvoke(new Action(CalculateStatus));
                    }
                );

                return result;
            }
        }

        public ICommand RunCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        Execute(DockScreen);
                    }
                );

                return result;
            }
        }

        public ICommand RunExCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var data = new LauncherItemWithScreen(Model, DockScreen, null);
                        var window = AppSender.SendCreateWindow(WindowKind.LauncherExecute, data, null);

                        window.Show();
                    }
                );

                return result;
            }
        }

        public ICommand OpenDirectoryCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var type = (LauncherCommandType)o;

                        if(type == LauncherCommandType.ParentDirectory) {
                            var path = Environment.ExpandEnvironmentVariables(Model.Command);
                            ExecuteUtility.OpenDirectoryWithFileSelect(path, AppNonProcess, default(LauncherItemModel));
                        } else {
                            Debug.Assert(type == LauncherCommandType.WorkDirectory);
                            var path = Environment.ExpandEnvironmentVariables(Model.WorkDirectoryPath);
                            ExecuteUtility.OpenDirectory(path, AppNonProcess, default(LauncherItemModel));
                        }
                    }
                );

                return result;
            }
        }

        public ICommand CopyTextCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var type = (LauncherCommandType)o;
                        var map = new Dictionary<LauncherCommandType, Func<string>>() {
                            { LauncherCommandType.Command, () => Model.Command },
                            { LauncherCommandType.ParentDirectory, () => Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(Model.Command)) },
                            { LauncherCommandType.WorkDirectory, () => Model.WorkDirectoryPath },
                        };
                        var s = map[type]();

                        ClipboardUtility.CopyText(s, AppNonProcess.ClipboardWatcher);
                    }
                );

                return result;
            }
        }

        public ICommand OpenCustomizeCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var data = new LauncherItemWithScreen(Model, DockScreen, null);
                        var window = AppSender.SendCreateWindow(WindowKind.LauncherCustomize, data, null);
                        window.Show();
                    }
                );

                return result;
            }
        }

        public ICommand OpenPropertyCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var s = Environment.ExpandEnvironmentVariables(Model.Command);
                        ExecuteUtility.OpenProperty(s, IntPtr.Zero);
                    }
                );

                return result;
            }
        }

        public ICommand DragOverCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var eventData = (EventData<DragEventArgs>)o;
                        if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
                            eventData.EventArgs.Effects = DragDropEffects.Move;
                        } else {
                            eventData.EventArgs.Effects = DragDropEffects.None;
                        }
                        eventData.EventArgs.Handled = true;
                    }
                );

                return result;
            }
        }

        public ICommand DragDropCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var eventData = (EventData<DragEventArgs>)o;
                        if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
                            var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];

                            switch(LauncherItemSetting.FileDropMode) {
                                case LauncherItemFileDropMode.ShowExecuteWindow:
                                    {
                                        var data = new LauncherItemWithScreen(Model, DockScreen, filePathList);
                                        var window = AppSender.SendCreateWindow(WindowKind.LauncherExecute, data, null);
                                        window.Show();
                                        window.Dispatcher.BeginInvoke(new Action(() => {
                                            WindowsUtility.ShowActive(HandleUtility.GetWindowHandle(window));
                                        }), DispatcherPriority.SystemIdle);
                                    }
                                    break;

                                case LauncherItemFileDropMode.ArgumentExecute:
                                    {
                                        var dummyModel = (LauncherItemModel)Model.DeepClone();
                                        var option = string.Join(" ", filePathList.WhitespaceToQuotation());
                                        dummyModel.Option = option;
                                        try {
                                            ExecuteUtility.RunItem(dummyModel, DockScreen, AppNonProcess, AppSender);
                                            SettingUtility.IncrementLauncherItem(Model, option, null, AppNonProcess);
                                        } catch(Exception ex) {
                                            AppNonProcess.Logger.Warning(ex);
                                        }
                                    }
                                    break;

                                default:
                                    throw new NotImplementedException();
                            }

                            eventData.EventArgs.Handled = true;
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        Visibility ToVisibility(bool test)
        {
            return test ? Visibility.Visible : Visibility.Collapsed;
        }

        void CalculateStatus()
        {
            // コマンド
            var command = Environment.ExpandEnvironmentVariables(Model.Command ?? string.Empty);
            HasDataCommand = !string.IsNullOrWhiteSpace(command);

            // 作業ディレクトリ
            var workDir = Environment.ExpandEnvironmentVariables(Model.WorkDirectoryPath ?? string.Empty);
            HasDataWorkDirectory = !string.IsNullOrWhiteSpace(workDir);
            ExistsWorkDirectory = HasDataWorkDirectory && Directory.Exists(workDir);

            switch(Model.LauncherKind) {
                case LauncherKind.File:
                    {
                        // ファイル(ディレクトリとして有効か)
                        ExistsCommand = HasDataCommand && FileUtility.Exists(command);

                        // 親ディレクトリ
                        if(!string.IsNullOrWhiteSpace(command)) {
                            var parentDir = Path.GetDirectoryName(command);
                            HasDataParentDirectory = !string.IsNullOrWhiteSpace(parentDir);
                            ExistsParentDirectory = ExistsCommand && HasDataParentDirectory && Directory.Exists(parentDir);
                        } else {
                            HasDataParentDirectory = false;
                            ExistsParentDirectory = false;
                        }
                    }
                    break;
            }
        }

        #endregion

        #region ILauncherButton

        public string ToolbarText { get { return DisplayText; } }
        public ImageSource ToolbarImage { get { return GetIcon(IconScale); } }
        public ImageSource MenuImage { get { return GetIcon(IconScale.Small); } }
        public Color ToolbarHotTrack { get { return GetIconColor(IconScale); } }
        public string ToolTipTitle { get { return ToolbarText; } }

        public string ToolTipMessage { get { return Model.Comment; } }
        public bool HasToolTipMessage { get { return !string.IsNullOrEmpty(ToolTipMessage); } }
        public ImageSource ToolTipImage { get { return GetIcon(IconScale.Normal); } }

        public bool IsMenuOpen
        {
            get { return this._isMenuOpen; }
            set { SetVariableValue(ref this._isMenuOpen, value); }
        }

        #endregion

        #region LauncherItemSimpleViewModel

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DockScreen = null;
                LauncherItemSetting = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
