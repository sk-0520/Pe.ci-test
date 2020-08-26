using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Preferences;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon
{
    internal class ClockLauncherItem: LauncherItemExtensionBase
    {
        #region variable

        ClockLauncherItemSetting? _setting;

        #endregion

        public ClockLauncherItem(ILauncherItemExtensionCreateParameter parameter, IPluginInformations pluginInformations, PluginBase plugin)
            : base(parameter, pluginInformations)
        {
            Plugin = plugin;

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
        DispatcherTimer ClockTimer { get; } = new DispatcherTimer(DispatcherPriority.Normal);

        #endregion

        #region function


        #endregion

        #region ILauncherItemExtension

        public override bool CustomDisplayText => true;

        public override string DisplayText => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local).ToString(Setting.Format);

        public override bool CustomLauncherIcon => throw new NotImplementedException();

        public override bool SupportedPreferences => true;

        public override void Start()
        {
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = TimeSpan.FromMilliseconds(500);
            ClockTimer.Start();
        }

        public override void End()
        {
            ClockTimer.Stop();
            ClockTimer.Tick -= ClockTimer_Tick;
        }

        public override object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale)
        {
            switch(iconMode) {
                case LauncherItemIconMode.Toolbar:
                case LauncherItemIconMode.Tooltip:
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

            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            RaisePropertyChanged(nameof(DisplayText));

            ClockTimer.Interval = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds - currentTime.Millisecond);
            ClockTimer.Start();
        }

    }
}
