using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginSettingEditorElement: ElementBase, IPLuginId
    {
        public PluginSettingEditorElement(IPlugin plugin, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Plugin = plugin;
            if(Plugin is IPreferences preferences) {
                SupportedPreferences = true;
                Preferences = preferences;
            } else {
                SupportedPreferences = false;
            }
        }

        #region property

        public IPlugin Plugin { get; }

        public bool SupportedPreferences { get; }
        IPreferences? Preferences { get; }

        #endregion

        #region function
        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion

        #region IPLuginId

        public Guid PluginId => Plugin.PluginInformations.PluginIdentifiers.PluginId;

        #endregion
    }
}
