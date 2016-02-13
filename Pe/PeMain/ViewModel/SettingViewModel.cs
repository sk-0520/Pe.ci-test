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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class SettingViewModel: ViewModelBase, IHasCommonData, IHasView<SettingWindow>
    {
        #region variable

        TabItem _selectedTab;

        MainSettingViewModel _mainSetting;
        LauncherItemSettingViewModel _launcherItemSetting;
        ToolbarSettingViewModel _toolbarSetting;
        CommandSettingViewModel _commandSetting;
        NoteSettingViewModel _noteSetting;
        ClipboardSettingViewModel _clipboardSetting;
        TemplateSettingViewModel _templateSetting;

        #endregion

        public SettingViewModel(CommonData commonData, SettingWindow view)
        {
            CommonData = commonData;
            View = view;
            SettingNotifiyItem = new SettingNotifyData();

            if(HasView) {
                this._selectedTab = View.pageMain;
            }
        }

        #region property

        public SettingNotifyData SettingNotifiyItem { get; private set; }

        public TabItem SelectedTab
        {
            get { return this._selectedTab; }
            set
            {
                if(SetVariableValue(ref this._selectedTab, value)) {
                    if(HasView) {
                        if(value == View.pageToolbar) {
                            ToolbarSetting.Refresh();
                        }
                    }
                }
            }
        }

        public MainSettingViewModel MainSetting
        {
            get
            {
                if(this._mainSetting == null) {
                    this._mainSetting = new MainSettingViewModel(
                        CommonData.MainSetting.RunningInformation,
                        CommonData.MainSetting.Language,
                        CommonData.MainSetting.Logging,
                        CommonData.MainSetting.SystemEnvironment,
                        CommonData.MainSetting.Stream,
                        CommonData.MainSetting.WindowSave,
                        CommonData.MainSetting.General,
                        HasView ? View.controlMainSetting : null,
                        CommonData.NonProcess,
                        SettingNotifiyItem
                    );
                }

                return this._mainSetting;
            }
        }

        public LauncherItemSettingViewModel LauncherItemSetting
        {
            get
            {
                if(this._launcherItemSetting == null) {
                    this._launcherItemSetting = new LauncherItemSettingViewModel(
                        CommonData.LauncherItemSetting,
                        HasView ? View.controlLauncherItemSetting : null,
                        CommonData.NonProcess,
                        CommonData.AppSender,
                        SettingNotifiyItem
                    );
                }

                return this._launcherItemSetting;
            }
        }

        public ToolbarSettingViewModel ToolbarSetting
        {
            get
            {
                if(this._toolbarSetting == null) {
                    this._toolbarSetting = new ToolbarSettingViewModel(
                        CommonData.MainSetting.Toolbar,
                        CommonData.LauncherGroupSetting,
                        CommonData.LauncherItemSetting,
                        HasView ? View.controlToolbarSetting : null,
                        CommonData.NonProcess,
                        CommonData.AppSender,
                        SettingNotifiyItem
                    );
                }

                return this._toolbarSetting;
            }
        }

        public CommandSettingViewModel CommandSetting
        {
            get
            {
                if(this._commandSetting == null) {
                    this._commandSetting = new CommandSettingViewModel(
                        CommonData.MainSetting.Command,
                        HasView ? View.controlCommandSetting : null,
                        CommonData.NonProcess,
                        SettingNotifiyItem
                    );
                }

                return this._commandSetting;
            }
        }

        public NoteSettingViewModel NoteSetting
        {
            get
            {
                if(this._noteSetting == null) {
                    this._noteSetting = new NoteSettingViewModel(
                        CommonData.MainSetting.Note,
                        HasView ? View.controlNoteSetting : null,
                        CommonData.NonProcess,
                        SettingNotifiyItem
                    );
                }

                return this._noteSetting;
            }
        }

        public ClipboardSettingViewModel ClipboardSetting
        {
            get
            {
                if(this._clipboardSetting == null) {
                    this._clipboardSetting = new ClipboardSettingViewModel(
                        CommonData.MainSetting.Clipboard,
                        HasView ? View.controlClipboardSetting : null,
                        CommonData.NonProcess,
                        SettingNotifiyItem
                    );
                }

                return this._clipboardSetting;
            }
        }

        public TemplateSettingViewModel TemplateSetting
        {
            get
            {
                if(this._templateSetting == null) {
                    this._templateSetting = new TemplateSettingViewModel(
                        CommonData.MainSetting.Template,
                        HasView ? View.controlTemplateSetting : null,
                        CommonData.NonProcess,
                        SettingNotifiyItem
                    );
                }

                return this._templateSetting;
            }
        }

        #region IHasCommonData

        public CommonData CommonData { get; private set; }

        #endregion

        #region IHasView

        public SettingWindow View { get; private set; }
        public bool HasView { get { return HasViewUtility.GetHasView(this); } }

        #endregion

        #endregion

        #region command

        public ICommand CancelCommand
        {
            get
            {
                var reslut = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Close();
                        }
                    }
                );

                return reslut;
            }
        }

        public ICommand AcceptCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.DialogResult = true;
                        }
                    }
                );

                return result;
            }
        }

        #endregion
    }
}
