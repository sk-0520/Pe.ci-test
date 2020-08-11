using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Preferences;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon
{
    internal class ClockLauncherItem: LauncherItemExtensionBase
    {
        public ClockLauncherItem(ILauncherItemExtensionCreateParameter parameter, IPluginInformations pluginInformations, PluginBase plugin)
            : base(parameter, pluginInformations)
        {
            Plugin = plugin;
        }

        #region function

        PluginBase Plugin { get; }

        #endregion

        #region ILauncherItemExtension


        #endregion
        public override bool CustomDisplayText => throw new NotImplementedException();

        public override string DisplayText => throw new NotImplementedException();

        public override bool CustomLauncherIcon => throw new NotImplementedException();

        public override bool SupportedPreferences => true;

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
            return new ClockLauncherItemPreferences(Plugin, AddonExecutor, DispatcherWrapper, SkeletonImplements, ImageLoader, HttpUserAgentFactory, LoggerFactory);
        }
    }
}
