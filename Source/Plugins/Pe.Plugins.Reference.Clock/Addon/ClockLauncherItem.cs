using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Text;
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

        ClockLauncherItemSetting? _setting;
        DateTime _currentTime = DateTime.Now;

        internal void Test()
        {
            CurrentTime = DateTime.Now;
        }

        #endregion

        public ClockLauncherItem(ILauncherItemExtensionCreateParameter parameter, IPluginInformations pluginInformations, PluginBase plugin)
            : base(parameter, pluginInformations)
        {
            Plugin = plugin;

            ClockTimer = new DispatcherTimer(DispatcherPriority.Normal);
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = TimeSpan.FromMilliseconds(500);
        }

        #region property

        PluginBase Plugin { get; }
        ClockLauncherItemSetting Setting
        {
            get
            {
                if(this._setting == null) {
                    ContextWorker.RunLauncherItemAddon(c => {
                        if(!c.Storage.Persistent.Normal.TryGet<ClockLauncherItemSetting>(LauncherItemId, string.Empty, out this._setting)) {
                            this._setting = new ClockLauncherItemSetting();
                        }
                    });
                    Debug.Assert(this._setting != null);
                }

                return this._setting;
            }
        }
        DispatcherTimer ClockTimer { get; }

        public DateTime CurrentTime
        {
            get => this._currentTime;
            private set
            {
                this._currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        ClockLauncherItemViewModel? ClockLauncherItemViewModel { get; set; }
        ClockLauncherItemControl? ClockLauncherItemControl { get; set; }

        #endregion

        #region function


        #endregion

        #region ILauncherItemExtension

        public override bool CustomDisplayText => true;

        public override string DisplayText => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local).ToString(Setting.Format);

        public override bool CustomLauncherIcon => throw new NotImplementedException();

        public override bool SupportedPreferences => true;

        public override void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible)
        {
            switch(iconMode) {
                case LauncherItemIconMode.Toolbar:
                    if(isVisible) {
                        ClockTimer.Start();
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
                        if(ClockLauncherItemViewModel == null) {
                            Debug.Assert(ClockLauncherItemControl == null);

                            ClockLauncherItemViewModel = new ClockLauncherItemViewModel(this, SkeletonImplements, PlatformTheme, MediaConverter, DispatcherWrapper, LoggerFactory);
                            ClockLauncherItemControl = new ClockLauncherItemControl() {
                                DataContext = ClockLauncherItemViewModel,
                            };
                        }
                        Debug.Assert(ClockLauncherItemControl != null);
                        return ClockLauncherItemControl;
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

        public override void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
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
