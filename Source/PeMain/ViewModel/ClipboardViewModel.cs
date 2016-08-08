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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.View.Window;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
//	using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Extensions;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class ClipboardViewModel: HasViewSingleModelWrapperIndexViewModelBase<ClipboardSettingModel, ClipboardWindow, ClipboardIndexItemCollectionModel, ClipboardIndexItemModel, ClipboardItemViewModel>
    {
        #region variable

        ClipboardItemViewModel _selectedViewModel;
        ImageScale _imageScale;

        #endregion

        public ClipboardViewModel(ClipboardSettingModel model, ClipboardWindow view, ClipboardIndexSettingModel indexModel, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view, indexModel, appNonProcess, appSender)
        {
            Items.SortDescriptions.Add(new SortDescription("Sort", ListSortDirection.Descending));
        }

        #region property

        public bool IsEnabled
        {
            get { return Model.IsEnabled; }
            set { SetModelValue(value); }
        }

        public ClipboardItemViewModel SelectedViewModel
        {
            get { return this._selectedViewModel; }
            set
            {
                var prevViewModel = this._selectedViewModel;
                if(SetVariableValue(ref this._selectedViewModel, value)) {
                    if(this._selectedViewModel != null) {
                        if(HasView) {
                            // TODO: View依存
                            var map = new Dictionary<ClipboardType, TabItem>() {
                                { ClipboardType.Text, View.pageText },
                                { ClipboardType.Rtf, View.pageRtf },
                                { ClipboardType.Html, View.pageHtml },
                                { ClipboardType.Image, View.pageImage },
                                { ClipboardType.Files, View.pageFiles },
                            };
                            var type = ClipboardUtility.GetSingleClipboardType(this._selectedViewModel.Model.Type);
                            foreach(var tab in map.Values) {
                                tab.IsSelected = false;
                            }
                            map[type].IsSelected = true;
                        }
                        if(prevViewModel != null) {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// リスト部の幅。
        /// </summary>
        public double ItemsListWidth
        {
            get { return Model.ItemsListWidth; }
            set { SetModelValue(value); }
        }

        public ImageScale ImageScale
        {
            get { return this._imageScale; }
            set { SetVariableValue(ref this._imageScale, value); }
        }

        public int LimitSize
        {
            get { return Model.SaveCount; }
        }

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Font); }
            //set { FontModelProperty.SetFamily(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Font); }
            //set { FontModelProperty.SetBold(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Font); }
            //set { FontModelProperty.SetItalic(Model.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Font); }
            //set { FontModelProperty.SetSize(Model.Font, value, OnPropertyChanged); }
        }

        #endregion

        public bool DuplicationMoveHead
        {
            get { return Model.DuplicationMoveHead; }
        }

        public IndexItemsDoubleClickBehavior DoubleClickBehavior
        {
            get { return Model.DoubleClickBehavior; }
            set { SetModelValue(value); }
        }

        #endregion

        #region command

        public ICommand RemoveItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var nowViewModel = SelectedViewModel;
                        if(nowViewModel == null) {
                            return;
                        }

                        var index = IndexPairList.ViewModelList.IndexOf(nowViewModel);

                        IndexPairList.Remove(nowViewModel);
                        AppSender.SendRemoveIndex(IndexKind.Clipboard, nowViewModel.Model.Id, Timing.Delay);

                        if(IndexPairList.Any()) {
                            while(IndexPairList.ViewModelList.Count <= index) {
                                index -= 1;
                            }
                            SelectedViewModel = IndexPairList.ViewModelList[index];
                        }
                    }
                );

                return result;
            }
        }

        public ICommand ClipboardClearCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        Clipboard.Clear();
                    }
                );

                return result;
            }
        }

        public ICommand SaveItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(SelectedViewModel == null) {
                            return;
                        }

                        SaveFileInDialog(SelectedViewModel);
                    }
                );

                return result;
            }
        }

        public ICommand ItemDoubleClickCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(SelectedViewModel == null) {
                            return;
                        }

                        switch(DoubleClickBehavior) {
                            case IndexItemsDoubleClickBehavior.Copy: {
                                    SelectedViewModel.CopyItemCommand.ExecuteIfCanExecute(o);
                                }
                                break;

                            case IndexItemsDoubleClickBehavior.Send: {
                                    if(!SelectedViewModel.EnabledClipboardTypesText) {
                                        return;
                                    }
                                    SelectedViewModel.SendItemCommand.ExecuteIfCanExecute(o);
                                }
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                );
            }
        }

        #endregion

        #region function

        protected override ClipboardItemViewModel CreateIndexViewModel(ClipboardIndexItemModel model, object data)
        {
            var result = new ClipboardItemViewModel(
                model,
                AppSender,
                AppNonProcess
            );

            return result;
        }

        bool SaveFileInDialog(ClipboardItemViewModel vm)
        {
            var srcFilters = new[] {
                new DialogFilterValueItem<ClipboardType>(ClipboardType.Text, AppNonProcess.Language["dialog/filter/txt"], Constants.dialogFilterText),
                new DialogFilterValueItem<ClipboardType>(ClipboardType.Rtf, AppNonProcess.Language["dialog/filter/rtf"], Constants.dialogFilterRtf),
                new DialogFilterValueItem<ClipboardType>(ClipboardType.Html, AppNonProcess.Language["dialog/filter/html"], Constants.dialogFilterHtml),
                new DialogFilterValueItem<ClipboardType>(ClipboardType.Image, AppNonProcess.Language["dialog/filter/png"], Constants.dialogFilterPng),
            };
            var filter = new DialogFilterList();

            var bestType = ClipboardUtility.GetSingleClipboardType(vm.Model.Type);
            var types = ClipboardUtility.GetClipboardTypeList(vm.Model.Type);
            var defIndex = 0;
            var tempIndex = 0;
            foreach(var type in types.Where(t => t != ClipboardType.Files)) {
                var filterItem = srcFilters.FirstOrDefault(f => f.Value == type);
                if(filterItem != null) {
                    filter.Add(filterItem);
                    tempIndex += 1;
                    if(filterItem.Value == bestType) {
                        defIndex = tempIndex;
                    }
                }
            }

            if(!filter.Any()) {
                AppNonProcess.Logger.Information("type list: 0");
                return false;
            }

            var name = PathUtility.ToSafeNameDefault(vm.Model.Name);
            if(string.IsNullOrWhiteSpace(name)) {
                name = Constants.GetNowTimestampFileName();
            }
            var dialog = new SaveFileDialog() {
                Filter = filter.FilterText,
                FilterIndex = defIndex,
                AddExtension = true,
                CheckPathExists = true,
                ValidateNames = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                FileName = name,
            };

            var dialogResult = dialog.ShowDialog();
            if(dialogResult.GetValueOrDefault()) {
                var type = ((DialogFilterValueItem<ClipboardType>)filter[dialog.FilterIndex - 1]).Value;
                SaveFile(dialog.FileName, vm, type);
                return true;
            } else {
                return false;
            }
        }

        bool SaveFile(string path, ClipboardItemViewModel vm, ClipboardType saveType)
        {
            Debug.Assert(saveType != ClipboardType.Files);

            var map = new Dictionary<ClipboardType, Action>() {
                { ClipboardType.Text, () => File.WriteAllText(path, vm.Text) },
                { ClipboardType.Rtf, () => File.WriteAllText(path, vm.Rtf) },
                { ClipboardType.Html, () => File.WriteAllText(path, vm.HtmlCode) },
                { ClipboardType.Image, () => {
                    using(var stream = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(vm.Image));
                        encoder.Save(stream);
                    }
                } },
            };

            try {
                map[saveType]();
                return true;
            } catch(Exception ex) {
                AppNonProcess.Logger.Error(ex);
                return false;
            }

        }

        #endregion

        #region IWindowStatus

        public double WindowLeft
        {
            get { return WindowStatusProperty.GetWindowLeft(Model); }
            set { WindowStatusProperty.SetWindowLeft(Model, value, OnPropertyChanged); }
        }

        public double WindowTop
        {
            get { return WindowStatusProperty.GetWindowTop(Model); }
            set { WindowStatusProperty.SetWindowTop(Model, value, OnPropertyChanged); }
        }

        public double WindowWidth
        {
            get { return WindowStatusProperty.GetWindowWidth(Model); }
            set { WindowStatusProperty.SetWindowWidth(Model, value, OnPropertyChanged); }
        }

        public double WindowHeight
        {
            get { return WindowStatusProperty.GetWindowHeight(Model); }
            set { WindowStatusProperty.SetWindowHeight(Model, value, OnPropertyChanged); }
        }

        public WindowState WindowState
        {
            get { return WindowStatusProperty.GetWindowState(Model); }
            set { WindowStatusProperty.SetWindowState(Model, value, OnPropertyChanged); }
        }

        #region IVisible

        public Visibility Visibility
        {
            get { return VisibleVisibilityProperty.GetVisibility(Model); }
            set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
        }

        public bool IsVisible
        {
            get { return VisibleVisibilityProperty.GetVisible(Model); }
            set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
        }

        #endregion

        #region ITopMost

        public bool IsTopmost
        {
            get { return TopMostProperty.GetTopMost(Model); }
            set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
        }

        #endregion

        #endregion

        #region HasViewSingleModelWrapperIndexViewModelBase

        protected override void InitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing += View_UserClosing;

            base.InitializeView();
        }

        protected override void UninitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing -= View_UserClosing;

            base.UninitializeView();
        }

        #endregion

        private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            IsVisible = false;
        }
    }
}
