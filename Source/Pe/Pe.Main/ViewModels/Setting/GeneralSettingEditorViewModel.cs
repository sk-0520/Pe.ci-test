using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface IGeneralSettingEditor
    {
        #region property

        string Header { get; }
        bool IsInitialized { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }

    public abstract class GeneralSettingEditorViewModelBase<TModel> : SingleModelViewModelBase<TModel>, IGeneralSettingEditor
        where TModel : GeneralSettingEditorElementBase
    {
        #region variable

        bool _isInitialized;

        #endregion

        public GeneralSettingEditorViewModelBase(TModel model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(Model.IsInitialized), OnInitialized);
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected PropertyChangedHooker PropertyChangedHooker { get; }
        #endregion

        #region command
        #endregion

        #region function

        protected virtual void BuildChildren()
        { }
        protected virtual void RaiseChildren()
        { }

        private void OnInitialized()
        {
            if(!Model.IsInitialized) {
                return;
            }

            BuildChildren();

            var properties = GetType().GetProperties()
                .Where(i => i.CanRead)
            ;
            foreach(var property in properties) {
                RaisePropertyChanged(property.Name);
            }

            RaiseChildren();

            IsInitialized = true;
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            Model.PropertyChanged -= Model_PropertyChanged;

            base.DetachModelEventsImpl();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IGeneralSettingEditor

        public abstract string Header { get; }

        public bool IsInitialized
        {
            get => this._isInitialized;
            private set => SetProperty(ref this._isInitialized, value);
        }

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }


    }

    public sealed class AppExecuteSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppExecuteSettingEditorElement>
    {
        public AppExecuteSettingEditorViewModel(AppExecuteSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string UserId
        {
            get => Model.UserId;
            private set => SetModelValue(value);
        }
        public bool SendUsageStatistics
        {
            get => Model.SendUsageStatistics;
            set => SetModelValue(value);
        }

        #endregion

        #region command

        public ICommand CreateUserIdFromRandomCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var userIdManager = new UserIdManager(LoggerFactory);
                UserId = userIdManager.CreateFromRandom();
            }
        ));

        public ICommand CreateUserIdFromEnvironmentCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var userIdManager = new UserIdManager(LoggerFactory);
                UserId = userIdManager.CreateFromEnvironment();
            }
        ));

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Execute;

        #endregion
    }


    public sealed class AppGeneralSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppGeneralSettingEditorElement>
    {
        public AppGeneralSettingEditorViewModel(AppGeneralSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            //TODO: リソース制限が必要
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            CultureInfoItems = new ObservableCollection<CultureInfo>(cultures);
        }

        #region property

        public ObservableCollection<CultureInfo> CultureInfoItems { get; }
        public CultureInfo SelectedCultureInfo
        {
            get => Model.CultureInfo;
            set => SetModelValue(value, nameof(Model.CultureInfo));
        }

        #endregion

        #region command


        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_General;

        #endregion
    }

    public sealed class AppUpdateSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppUpdateSettingEditorElement>
    {
        public AppUpdateSettingEditorViewModel(AppUpdateSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public bool IsCheckReleaseVersion
        {
            get => Model.IsCheckReleaseVersion;
            set => SetModelValue(value);
        }
        public bool IsCheckRcVersion
        {
            get => Model.IsCheckRcVersion;
            set => SetModelValue(value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Update;

        #endregion
    }


    public sealed class AppCommandSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppCommandSettingEditorElement>
    {
        public AppCommandSettingEditorViewModel(AppCommandSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public FontViewModel? Font { get; private set; }
        public IconBox IconBox
        {
            get => Model.IconBox;
            set => SetModelValue(value);
        }

        //public TimeSpan HideWaitTime
        //{
        //    get => Model.HideWaitTime;
        //    set => SetModelValue(value);
        //}

        public double MinimumHideWaitSeconds => TimeSpan.FromMilliseconds(250).TotalSeconds;
        public double MaximumHideWaitSeconds => TimeSpan.FromSeconds(5).TotalSeconds;
        public double HideWaitMilliseconds
        {
            get => Model.HideWaitTime.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value), nameof(Model.HideWaitTime));
        }

        public bool FindTag
        {
            get => Model.FindTag;
            set => SetModelValue(value);
        }
        public bool FindFile
        {
            get => Model.FindFile;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Command;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }


        #endregion
    }


    public sealed class AppNoteSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppNoteSettingEditorElement>
    {
        public AppNoteSettingEditorViewModel(AppNoteSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public FontViewModel? Font { get; private set; }
        public NoteCreateTitleKind TitleKind
        {
            get => Model.TitleKind;
            set => SetModelValue(value);
        }
        public NoteLayoutKind LayoutKind
        {
            get => Model.LayoutKind;
            set => SetModelValue(value);
        }

        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetModelValue(value);
        }

        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetModelValue(value);
        }

        public bool IsTopmost
        {
            get => Model.IsTopmost;
            set => SetModelValue(value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Note;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        #endregion
    }


    public sealed class AppStandardInputOutputSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppStandardInputOutputSettingEditorElement>
    {
        public AppStandardInputOutputSettingEditorViewModel(AppStandardInputOutputSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => ToString()!;

        #endregion
    }

    public sealed class AppWindowSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppWindowSettingEditorElement>
    {
        public AppWindowSettingEditorViewModel(AppWindowSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property
        public bool IsEnabled
        {
            get => Model.IsEnabled;
            set => SetModelValue(value);
        }
        public int Count
        {
            get => Model.Count;
            set => SetModelValue(value);
        }
        public TimeSpan Interval
        {
            get => Model.Interval;
            set => SetModelValue(value);
        }

        public int CountMaximum => 100;
        public int CountMinimum => 3;

        public double IntervalMinutes
        {
            get => Interval.TotalMinutes;
            set
            {
                Interval = TimeSpan.FromMinutes(value);
                RaisePropertyChanged();
            }
        }
        public double IntervalMaximum => TimeSpan.FromMinutes(30).TotalMinutes;
        public double IntervalMinimum => TimeSpan.FromMinutes(1).TotalMinutes;

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Window;

        #endregion
    }

}
