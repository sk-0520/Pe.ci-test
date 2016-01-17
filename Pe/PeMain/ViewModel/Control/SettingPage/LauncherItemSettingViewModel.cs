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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using ContentTypeTextNet.Library.SharedLibrary.Data;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.Library.PeData.Setting;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

    public class LauncherItemSettingViewModel: SettingPageLauncherIconCacheViewModelBase<LauncherItemSettingControl>, IHavingAppSender
    {
        #region variable

        LauncherListViewModel _launcherItems;
        bool _isItemEdited;
        LauncherListItemViewModel _selectedLauncherItem;

        #endregion

        public LauncherItemSettingViewModel(LauncherItemSettingModel launcherItemSetting, LauncherItemSettingControl view, IAppNonProcess appNonProcess, IAppSender appSender, SettingNotifyData settingNotifiyItem)
            : base(view, appNonProcess, settingNotifiyItem)
        {
            LauncherItemSetting = launcherItemSetting;
            AppSender = appSender;
        }

        #region proerty

        LauncherItemSettingModel LauncherItemSetting { get; set; }

        public bool IsItemEdited
        {
            get { return this._isItemEdited; }
            set
            {
                SetVariableValue(ref this._isItemEdited, value);
            }
        }

        public LauncherListViewModel LauncherItems
        {
            get
            {
                if(this._launcherItems == null) {
                    this._launcherItems = new LauncherListViewModel(
                        LauncherItemSetting.Items,
                        AppNonProcess,
                        AppSender
                    );
                }

                return this._launcherItems;
            }
        }

        public LauncherListItemViewModel SelectedLauncherItem
        {
            get { return this._selectedLauncherItem; }
            set { SetVariableValue(ref this._selectedLauncherItem, value); }
        }

        #endregion

        #region command

        public ICommand AppendItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var model = SettingUtility.CreateLauncherItem(LauncherItemSetting.Items, AppNonProcess);
                        SettingUtility.UpdateUniqueGuid(model, LauncherItems.LauncherItemPairList.ModelList);
                        var pair = LauncherItems.LauncherItemPairList.Add(model, null);
                        SelectedLauncherItem = pair.ViewModel;
                    }
                );

                return result;
            }
        }

        public ICommand RemoveItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var viewModel = o as LauncherListItemViewModel;
                        LauncherItems.LauncherItemPairList.Remove(viewModel.Model);
                        //LauncherItems.Items.Remove(o);
                    }
                );

                return result;
            }
        }

        public ICommand DragOverCommand
        {
            get
            {
                var reslut = CreateCommand(
                    o => {
                        var eventData = (EventData<DragEventArgs>)o;

                        eventData.EventArgs.Effects = DragDropEffects.None;

                        if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
                            var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];
                            if(filePathList != null && filePathList.Count() == 1) {
                                eventData.EventArgs.Effects = DragDropEffects.Copy;
                            }
                        }

                        eventData.EventArgs.Handled = true;
                    }
                );

                return reslut;
            }
        }

        public ICommand DragDropCommand
        {
            get
            {
                var reslut = CreateCommand(
                    o => {
                        var eventData = (EventData<DragEventArgs>)o;
                        if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
                            var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];
                            if(filePathList != null && filePathList.Count() == 1) {
                                // TODO: LauncherToolbarViewModel.DragDropCommandと重複。いつか見た光景。
                                var filePath = filePathList.First();
                                var loadShorcut = true;
                                if(PathUtility.IsShortcut(filePath)) {
                                    var dialogResult = MessageBox.Show(
                                        AppNonProcess.Language["confirm/shortcut/message"],
                                        AppNonProcess.Language["confirm/shortcut/caption"],
                                        MessageBoxButton.YesNoCancel,
                                        MessageBoxImage.Question
                                    );
                                    if(dialogResult == MessageBoxResult.Cancel) {
                                        // やめる
                                        return;
                                    }
                                    loadShorcut = dialogResult == MessageBoxResult.Yes;
                                }
                                var item = LauncherItemUtility.CreateFromFile(filePath, loadShorcut, AppNonProcess);
                                SettingUtility.UpdateUniqueGuid(item, LauncherItems.LauncherItemPairList.ModelList);
                                var pair = LauncherItems.LauncherItemPairList.Add(item, null);
                                SelectedLauncherItem = pair.ViewModel;
                            }
                        }
                    }
                );

                return reslut;
            }
        }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion
    }
}
