using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon
{
    internal class ClockLauncherItem: LauncherItemExtensionBase
    {
        public ClockLauncherItem(IAddonParameter parameter, IPluginInformations pluginInformations)
            : base(parameter, pluginInformations)
        {
        }

        #region function


        #endregion

        #region ILauncherItemExtension


        #endregion
        public override bool CustomDisplayText => throw new NotImplementedException();

        public override string DisplayText => throw new NotImplementedException();

        public override bool CustomLauncherIcon => throw new NotImplementedException();

        public override bool SupportedPreferences => throw new NotImplementedException();

        public override void Initialize(ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public override object GetIcon(LauncherItemIconMode iconMode, IconScale iconScale, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public override void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public override ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }
    }
}
