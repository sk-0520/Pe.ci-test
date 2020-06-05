using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginSettingEditorViewModel: SingleModelViewModelBase<PluginSettingEditorElement>
    {
        #region variable

        UserControl? _settingControl;

        #endregion

        public PluginSettingEditorViewModel(PluginSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        IDispatcherWrapper DispatcherWrapper { get; }

        public string PluginName => Model.Plugin.PluginInformations.PluginIdentifiers.PluginName;
        public Guid PluginId => Model.Plugin.PluginInformations.PluginIdentifiers.PluginId;
        public Version PluginVersion => Model.Plugin.PluginInformations.PluginVersions.PluginVersion;
        public Version MinimumSupportVersion => Model.Plugin.PluginInformations.PluginVersions.MinimumSupportVersion;
        public Version MaximumSupportVersion => Model.Plugin.PluginInformations.PluginVersions.MaximumSupportVersion;
        public string PrimaryCategory => Model.Plugin.PluginInformations.PluginCategory.PluginPrimaryCategory;
        public IReadOnlyList<string> SecondaryCategories => Model.Plugin.PluginInformations.PluginCategory.PluginSecondaryCategories;
        public bool HasSecondaryCategories => Model.Plugin.PluginInformations.PluginCategory.PluginSecondaryCategories.Count != 0;

        public bool HasSettingControl => Model.SupportedPreferences;

        public UserControl? SettingControl
        {
            get
            {
                if(!Model.SupportedPreferences) {
                    return null;
                }

                if(this._settingControl == null) {
                    this._settingControl = Model.BeginPreferences();
                }

                return this._settingControl;
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }
}
