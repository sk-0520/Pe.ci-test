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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using ContentTypeTextNet.Library.SharedLibrary.Data;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.IF;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Property;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View;
    using Microsoft.Win32;

    public class LoggingViewModel: HavingViewSingleModelWrapperViewModelBase<LoggingSettingModel, LoggingWindow>, ILogAppender, IWindowStatus, IHavingNonProcess
    {
        public LoggingViewModel(LoggingSettingModel model, LoggingWindow view, FixedSizeCollectionModel<LogItemModel> logItems, INonProcess nonProcess)
            : base(model, view)
        {
            NonProcess = nonProcess;

            if(logItems != null) {
                LogItems = logItems;
                View.Loaded += View_Loaded;
            } else {
                var loggingCount = Constants.loggingStockCount.GetClamp(Constants.LoggingStockCount);
                LogItems = new FixedSizeCollectionModel<LogItemModel>(loggingCount);
            }
        }

        #region property

        public FixedSizeCollectionModel<LogItemModel> LogItems { get; set; }

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
            set
            {
                VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged);
                if(IsVisible && HasView && View.listLog.HasItems) {
                    View.Dispatcher.BeginInvoke(new Action(() => {
                        View.listLog.ScrollIntoView(View.listLog.Items[View.listLog.Items.Count - 1]);
                    }));
                }
            }
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

        #endregion

        #region command

        public ICommand SaveCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SaveFileInDialog(LogItems);
                    }
                );

                return result;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        LogItems.Clear();
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        bool SaveFileInDialog(IEnumerable<LogItemModel> logItems)
        {
            var filter = new DialogFilterList() {
                new DialogFilterItem(NonProcess.Language["dialog/filter/log"], Constants.dialogFilterLog),
            };
            //var dialog = new SaveFileDialog() {
            //	AddExtension = true,
            //	CheckPathExists = true,
            //	ValidateNames = true,
            //	Filter = filter.FilterText,
            //	InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            //	FileName = Constants.GetNowTimestampFileName(),
            //};

            //var dialogResult = dialog.ShowDialog();
            //if (dialogResult.GetValueOrDefault()) {
            //	return SaveFile(dialog.FileName, logItems);
            //} else {
            //	return false;
            //}
            var dialogResut = DialogUtility.ShowSaveFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Constants.GetNowTimestampFileName(), filter);
            if(dialogResut != null) {
                return SaveFile(dialogResut, logItems);
            } else {
                return false;
            }
        }

        bool SaveFile(string path, IEnumerable<LogItemModel> logItems)
        {
            using(var stream = new StreamWriter(File.Create(path))) {
                foreach(var logMessage in logItems.Select(l => LogUtility.MakeLogDetailText(l))) {
                    stream.WriteLine(logMessage);
                }
            }

            return true;
        }

        void ShowTrigger(LogItemModel item)
        {
            var map = new Dictionary<LogKind, bool>() {
                { LogKind.Debug, Model.ShowTriggerDebug },
                { LogKind.Trace, Model.ShowTriggerTrace },
                { LogKind.Information, Model.ShowTriggerInformation },
                { LogKind.Warning, Model.ShowTriggerWarning },
                { LogKind.Error, Model.ShowTriggerError },
                { LogKind.Fatal, Model.ShowTriggerFatal },
            };
            if(map[item.LogKind]) {
                if(HasView) {
                    View.Dispatcher.BeginInvoke(new Action(() => {
                        if(!IsClosed) {
                            IsVisible = true;
                        }
                    }));
                } else {
                    IsVisible = true;
                }
            }
        }

        #endregion

        #region HavingViewSingleModelWrapperViewModelBase

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

        #region ILogCollector

        public void AddLog(LogItemModel item)
        {
            if(HasView) {
                View.Dispatcher.BeginInvoke(new Action(() => {
                    LogItems.Add(item);
                }));
            } else {
                LogItems.Add(item);
            }
            if(Model.AddShow) {
                ShowTrigger(item);
            }
            if(HasView) {
                View.Dispatcher.BeginInvoke(new Action(() => {
                    View.listLog.SelectedItem = item;
                    if(!View.IsActive && View.IsVisible) {
                        View.listLog.ScrollIntoView(View.listLog.SelectedItem);
                    }
                }));
            }
        }

        #endregion

        #region IHavingNonProcess

        public INonProcess NonProcess { get; private set; }

        #endregion

        void View_UserClosing(object sender, CancelEventArgs e)
        {
            Debug.Assert(HasView);

            e.Cancel = true;
            IsVisible = false;
        }

        void View_Loaded(object sender, RoutedEventArgs e)
        {
            View.Loaded -= View_Loaded;

            if(View.listLog.Items.Count != -1) {
                View.listLog.SelectedIndex = View.listLog.Items.Count - 1;
                View.listLog.ScrollIntoView(View.listLog.SelectedItem);
            }
        }

    }
}
