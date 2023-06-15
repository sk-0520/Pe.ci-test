using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface IGeneralSettingEditor: IDisposable
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

    public abstract class GeneralSettingEditorViewModelBase<TModel>: SettingItemViewModelBase<TModel>, IGeneralSettingEditor
        where TModel : GeneralSettingEditorElementBase
    {
        protected GeneralSettingEditorViewModelBase(TModel model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function

        #endregion

        #region SingleModelViewModelBase
        #endregion

        #region IGeneralSettingEditor

        public abstract string Header { get; }

        #endregion

    }

    public sealed class AppExecuteSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppExecuteSettingEditorElement>
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
        public bool IsEnabledTelemetry
        {
            get => Model.IsEnabledTelemetry;
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

        public ICommand OpenPrivacyPolicyDocumentCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenPrivacyPolicyDocument();
            }
        ));

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_Execute_Header;

        #endregion
    }

    public sealed class AppGeneralSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppGeneralSettingEditorElement>
    {
        #region define

        public class ThemePluginItemViewModel: ViewModelBase
        {
            public ThemePluginItemViewModel(IPlugin plugin, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
                : base(loggerFactory)
            {
                Plugin = plugin;
                ImageLoader = imageLoader;
                DispatcherWrapper = dispatcherWrapper;
            }

            #region property

            IPlugin Plugin { get; }
            IImageLoader ImageLoader { get; }
            IDispatcherWrapper DispatcherWrapper { get; }

            public string Name => Plugin.PluginInformation.PluginIdentifiers.PluginName;
            public PluginId Id => Plugin.PluginInformation.PluginIdentifiers.PluginId;

            public DependencyObject PluginIcon
            {
                get
                {
                    return DispatcherWrapper.Get(() => {
                        try {
                            var scale = ImageLoader.GetPrimaryDpiScale();
                            return Plugin.GetIcon(ImageLoader, new IconScale(IconBox.Small, scale));
                        } catch(Exception ex) {
                            Logger.LogError(ex, "[{0}] {1}, {2}", Plugin.PluginInformation.PluginIdentifiers.PluginName, ex.Message, Plugin.PluginInformation.PluginIdentifiers.PluginId);
                            return null!;
                        }
                    });
                }
            }

            #endregion
        }

        #endregion

        public AppGeneralSettingEditorViewModel(AppGeneralSettingEditorElement model, IReadOnlyCollection<string> cultureNames, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            CultureInfoItems = new ObservableCollection<CultureInfo>();
            var cultures = cultureNames.Select(i => CultureInfo.GetCultureInfo(i));
            CultureInfoItems.Add(CultureInfo.InvariantCulture);
            CultureInfoItems.AddRange(cultures);

            ThemePluginItems = Model.ThemePlugins.Select(i => new ThemePluginItemViewModel(i, imageLoader, DispatcherWrapper, LoggerFactory)).ToList();
        }

        #region property

        public RequestSender UserBackupDirectorySelectRequest { get; } = new RequestSender();

        public ObservableCollection<CultureInfo> CultureInfoItems { get; }
        public CultureInfo SelectedCultureInfo
        {
            get => Model.CultureInfo;
            set => SetModelValue(value, nameof(Model.CultureInfo));
        }

        public string UserBackupDirectoryPath
        {
            get => Model.UserBackupDirectoryPath;
            set => SetModelValue(value);
        }

        public PluginId ThemePluginId
        {
            get => Model.ThemePluginId;
            set => SetModelValue(value);
        }

        public IReadOnlyList<ThemePluginItemViewModel> ThemePluginItems { get; }

        public bool IsRegisterStartup
        {
            get => Model.IsRegisterStartup;
            set => SetModelValue(value);
        }

        public bool DelayStartup
        {
            get => Model.DelayStartup;
            set => SetModelValue(value);
        }
        public string StartupArgument
        {
            get => Model.StartupArgument;
            set => SetModelValue(value);
        }

        public double MinimumStartupWaitTimeSeconds => TimeSpan.FromSeconds(1).TotalSeconds;
        public double MaximumStartupWaitTimeSeconds => TimeSpan.FromMinutes(1).TotalSeconds;
        public double StartupWaitTimeSeconds
        {
            get => Model.StartupWaitTime.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value), nameof(Model.StartupWaitTime));
        }

        #endregion

        #region command

        public ICommand UserBackupDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectDirectory(
                    UserBackupDirectorySelectRequest,
                    dialogRequester.ExpandPath(UserBackupDirectoryPath),
                    r => {
                        UserBackupDirectoryPath = r.ResponseFilePaths[0];
                    }
                );
            }
        ));

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_General_Header;

        #endregion
    }

    public sealed class AppUpdateSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppUpdateSettingEditorElement>
    {
        public AppUpdateSettingEditorViewModel(AppUpdateSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public UpdateKind UpdateKind
        {
            get => Model.UpdateKind;
            set => SetModelValue(value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_Update_Header;

        #endregion
    }

    public sealed class AppNotifyLogSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppNotifyLogSettingEditorElement>
    {
        public AppNotifyLogSettingEditorViewModel(AppNotifyLogSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        public NotifyLogPosition Position
        {
            get => Model.Position;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_NotifyLog_Header;

        #endregion
    }

    public sealed class AppLauncherToolbarSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppLauncherToolbarSettingEditorElement>
    {
        public AppLauncherToolbarSettingEditorViewModel(AppLauncherToolbarSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public LauncherToolbarContentDropMode ContentDropMode
        {
            get => Model.ContentDropMode;
            set => SetModelValue(value);
        }

        public LauncherGroupPosition GroupMenuPosition
        {
            get => Model.GroupMenuPosition;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_LauncherToolbar_Header;

        #endregion
    }

    public sealed class AppCommandSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppCommandSettingEditorElement>
    {
        public AppCommandSettingEditorViewModel(AppCommandSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
        }

        #region property

        private IGeneralTheme GeneralTheme { get; }

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

        public double MinimumWidth => 100;
        public double MaximumWidth => 400;
        public double Width
        {
            get => Model.Width;
            set => SetModelValue(value);
        }

        public double MinimumHideWaitSeconds => TimeSpan.FromMilliseconds(250).TotalSeconds;
        public double MaximumHideWaitSeconds => TimeSpan.FromSeconds(5).TotalSeconds;
        public double HideWaitSeconds
        {
            get => Model.HideWaitTime.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value), nameof(Model.HideWaitTime));
        }

        public bool FindTag
        {
            get => Model.FindTag;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_Command_Header;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                    Font = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class AppNoteSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppNoteSettingEditorElement>
    {
        public AppNoteSettingEditorViewModel(AppNoteSettingEditorElement model, NoteConfiguration noteConfiguration, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            NoteConfiguration = noteConfiguration;
            GeneralTheme = generalTheme;
        }

        #region property

        private NoteConfiguration NoteConfiguration { get; }
        private IGeneralTheme GeneralTheme { get; }
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

        public NoteCaptionPosition CaptionPosition
        {
            get => Model.CaptionPosition;
            set => SetModelValue(value);
        }

        public int BlindWaitTimeSeconds
        {
            get => (int)Model.WaitTimes[NoteHiddenMode.Blind].TotalSeconds;
            set => Model.WaitTimes[NoteHiddenMode.Blind] = TimeSpan.FromSeconds(value);
        }
        public int MinimumBlindWaitTimeSeconds => (int)NoteConfiguration.HiddenBlindWaitTime.Minimum.TotalSeconds;
        public int MaximumBlindWaitTimeSeconds => (int)NoteConfiguration.HiddenBlindWaitTime.Maximum.TotalSeconds;

        public int CompactWaitTimeSeconds
        {
            get => (int)Model.WaitTimes[NoteHiddenMode.Compact].TotalSeconds;
            set => Model.WaitTimes[NoteHiddenMode.Compact] = TimeSpan.FromSeconds(value);
        }
        public int MinimumCompactWaitTimeSeconds => (int)NoteConfiguration.HiddenCompactWaitTime.Minimum.TotalSeconds;
        public int MaximumCompactWaitTimeSeconds => (int)NoteConfiguration.HiddenCompactWaitTime.Maximum.TotalSeconds;

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_Note_Header;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                    Font = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class AppStandardInputOutputSettingEditorViewModel: GeneralSettingEditorViewModelBase<AppStandardInputOutputSettingEditorElement>
    {
        public AppStandardInputOutputSettingEditorViewModel(AppStandardInputOutputSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
        }

        #region property
        private IGeneralTheme GeneralTheme { get; }

        public FontViewModel? Font { get; private set; }
        public Color OutputForegroundColor
        {
            get => Model.OutputForegroundColor;
            set => SetModelValue(value);
        }
        public Color OutputBackgroundColor
        {
            get => Model.OutputBackgroundColor;
            set => SetModelValue(value);
        }
        public Color ErrorForegroundColor
        {
            get => Model.ErrorForegroundColor;
            set => SetModelValue(value);
        }
        public Color ErrorBackgroundColor
        {
            get => Model.ErrorBackgroundColor;
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

        public override string Header => Properties.Resources.String_Setting_Generals_StandardInputOutput_Header;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                    Font = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class AppProxySettingEditorViewModel: GeneralSettingEditorViewModelBase<AppProxySettingEditorElement>
    {
        public AppProxySettingEditorViewModel(AppProxySettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public bool ProxyIsEnabled
        {
            get => Model.ProxyIsEnabled;
            set => SetModelValue(value);
        }
        public string ProxyUrl
        {
            get => Model.ProxyUrl;
            set => SetModelValue(value);
        }
        public bool CredentialIsEnabled
        {
            get => Model.CredentialIsEnabled;
            set => SetModelValue(value);
        }
        public string CredentialUser
        {
            get => Model.CredentialUser;
            set => SetModelValue(value);
        }
        public string CredentialPassword
        {
            get => Model.CredentialPassword;
            set => SetModelValue(value);
        }

        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Generals_Proxy_Header;

        #endregion
    }
}
