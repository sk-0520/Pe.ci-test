using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
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

        public string PluginName => Model.PluginState.Name;
        public string PluginVersion => Model.PluginVersion.ToString();
        public Guid PluginId => Model.PluginId;
        public string? PrimaryCategory => Model.Plugin?.PluginInformations.PluginCategory.PluginPrimaryCategory;
        public IReadOnlyList<string> SecondaryCategories => Model.Plugin?.PluginInformations.PluginCategory.PluginSecondaryCategories ?? new List<string>();
        public bool HasSecondaryCategories => SecondaryCategories.Count != 0;

        public DependencyObject PluginIcon
        {
            get
            {
                if(Model.Plugin == null) {
                    return null!;
                }

                return DispatcherWrapper.Get(() => {
                    try {
                        return Model.Plugin.GetIcon(Bridge.Models.Data.IconBox.Small, IconSize.DefaultScale);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "[{0}] {1}, {2}", Model.Plugin.PluginInformations.PluginIdentifiers.PluginName, ex.Message, Model.Plugin.PluginInformations.PluginIdentifiers.PluginId);
                        return null!;
                    }
                });
            }
        }

        public string SupportVersions
        {
            get
            {
                if(Model.Plugin == null) {
                    return Properties.Resources.String_Setting_Plugins_Item_NotLoaded_SupportVersions;
                }

                bool IsUnlimitedVersion(Version version) {
                    return version.Major == 0 && version.Minor == 0 && version.Build == 0;
                }
                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_Setting_Plugins_Item_SupportVersions_Format,
                    new Dictionary<string, string>() {
                        ["MIN"] = IsUnlimitedVersion(Model.Plugin.PluginInformations.PluginVersions.MinimumSupportVersion) ? Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited : Model.Plugin.PluginInformations.PluginVersions.MinimumSupportVersion.ToString(),
                        ["MAX"] = IsUnlimitedVersion(Model.Plugin.PluginInformations.PluginVersions.MaximumSupportVersion) ? Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited : Model.Plugin.PluginInformations.PluginVersions.MaximumSupportVersion.ToString()
                    }
                );
            }
        }

        public bool HasSettingControl => Model.SupportedPreferences;

        public bool IsLoadedPlugin => Model.Plugin != null;

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

        #region SingleModelViewModelBase

        protected override void ValidateDomain()
        {
            base.ValidateDomain();

            if(Model.SupportedPreferences) {
                Model.CheckPreferences();
            }
        }

        #endregion
    }
}
