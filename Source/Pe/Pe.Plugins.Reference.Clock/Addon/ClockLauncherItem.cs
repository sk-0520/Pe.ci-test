using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Preferences;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Views;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon
{
    internal class ClockLauncherItem: LauncherItemExtensionBase
    {
        #region variable

        private ClockLauncherItemSetting? _setting;
        private DateTime _currentTime = DateTime.Now;

        internal void Test()
        {
            CurrentTime = DateTime.Now;
        }

        #endregion

        public ClockLauncherItem(ILauncherItemExtensionCreateParameter parameter, IPluginInformation pluginInformation, PluginBase plugin)
            : base(parameter, pluginInformation)
        {
            Plugin = plugin;

            ClockTimer = new DispatcherTimer(DispatcherPriority.Normal);
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = TimeSpan.FromMilliseconds(500);
        }

        #region property

        private PluginBase Plugin { get; }
        private ClockLauncherItemSetting Setting
        {
            get
            {
                if(this._setting == null) {
                    ReloadSetting();
                    Debug.Assert(this._setting != null);
                }

                return this._setting;
            }
        }
        private DispatcherTimer ClockTimer { get; }

        public DateTime CurrentTime
        {
            get => this._currentTime;
            private set
            {
                this._currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        #endregion

        #region function

        private void ReloadSetting()
        {
            ContextWorker.RunLauncherItemAddon(c => {
                if(!c.Storage.Persistence.Normal.TryGet<ClockLauncherItemSetting>(LauncherItemId, string.Empty, out this._setting)) {
                    this._setting = new ClockLauncherItemSetting();
                }
                return false;
            });

            RaisePropertyChanged(nameof(Setting));
        }

        #endregion

        #region ILauncherItemExtension

        public override bool CustomDisplayText => true;

        public override string DisplayText => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local).ToString(Setting.Format, CultureInfo.InvariantCulture);

        public override bool CustomLauncherIcon => throw new NotImplementedException();

        public override bool SupportedPreferences => true;

        public override void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible, object callerObject)
        {
            switch(iconMode) {
                case LauncherItemIconMode.Toolbar:
                    if(isVisible) {
                        CallerObjects.Add(callerObject);
                    } else {
                        CallerObjects.Remove(callerObject);
                    }
                    if(CallerObjects.Count != 0) {
                        if(!ClockTimer.IsEnabled) {
                            ReloadSetting();
                            ClockTimer.Start();
                        }
                    } else {
                        ClockTimer.Stop();
                    }
                    break;

                case LauncherItemIconMode.Tooltip:
                case LauncherItemIconMode.Command:
                case LauncherItemIconMode.Setting:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public override object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale)
        {
            switch(iconMode) {
                case LauncherItemIconMode.Toolbar: {
                        var viewModel = new ClockLauncherItemControlViewModel(this, SkeletonImplements, PlatformTheme, MediaConverter, DispatcherWrapper, LoggerFactory);
                        return new ClockLauncherItemControl() {
                            DataContext = viewModel,
                        };
                    }

                case LauncherItemIconMode.Tooltip: {
                        return new Viewbox() {
                            Stretch = System.Windows.Media.Stretch.Fill,
                            StretchDirection = StretchDirection.Both,
                            Child = new TextBlock() {
                                Text = ClockUtility.GetClockEmoji(CurrentTime),
                            }
                        };
                    };

                case LauncherItemIconMode.Command:
                case LauncherItemIconMode.Setting:
                    return Plugin.GetIcon(ImageLoader, iconScale);

                default:
                    throw new NotImplementedException();
            }
        }

        public override void Execute(string? argument, ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            var viewModel = new ClockLauncherItemWindowViewModel(this, SkeletonImplements, DispatcherWrapper, LoggerFactory);
            var view = new ClockLauncherItemWindow() {
                DataContext = viewModel,
            };

            launcherItemExtensionExecuteParameter.ViewSupporter.RegisterWindowAsync(
                view,
                () => !viewModel.CanStop,
                () => {
                    view.DataContext = null;
                    viewModel.Dispose();
                }
            );
        }

        public override ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext)
        {
            return new ClockLauncherItemPreferences(Plugin, LauncherItemId, AddonExecutor, DispatcherWrapper, SkeletonImplements, ImageLoader, HttpUserAgentFactory, LoggerFactory);
        }

        #endregion

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            ClockTimer.Stop();

            CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            RaisePropertyChanged(nameof(DisplayText));

            ClockTimer.Interval = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds - CurrentTime.Millisecond);
            ClockTimer.Start();
        }
    }
}
