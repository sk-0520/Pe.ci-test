using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface IGeneralSettingEditor
    {
        #region property

        string Header { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }

    public abstract class GeneralSettingEditorViewModelBase<TModel> : SingleModelViewModelBase<TModel>, IGeneralSettingEditor
        where TModel : GeneralSettingEditorElementBase
    {
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

        private void OnInitialized()
        {
            if(!Model.IsInitialized) {
                return;
            }

            var properties = GetType().GetProperties()
                .Where(i => i.CanRead)
            ;
            foreach(var property in properties) {
                RaisePropertyChanged(property.Name);
            }
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
        { }

        #region property


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
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => ToString()!;

        #endregion
    }


    public sealed class AppCommandSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppCommandSettingEditorElement>
    {
        public AppCommandSettingEditorViewModel(AppCommandSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
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


    public sealed class AppNoteSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppNoteSettingEditorElement>
    {
        public AppNoteSettingEditorViewModel(AppNoteSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
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
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => ToString()!;

        #endregion
    }

}
