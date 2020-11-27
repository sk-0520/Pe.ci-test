using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginSettingEditorViewModel: SingleModelViewModelBase<PluginSettingEditorElement>
    {
        #region variable

        UserControl? _settingControl;

        #endregion

        public PluginSettingEditorViewModel(PluginSettingEditorElement model, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ImageLoader = imageLoader;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        IImageLoader ImageLoader { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        public string PluginName => Model.PluginState.PluginName;
        public string PluginVersion => Model.PluginVersion.ToString();
        public Guid PluginId => Model.PluginId;
        public string? PrimaryCategory => Model.Plugin?.PluginInformations.PluginCategory.PluginPrimaryCategory;
        public IReadOnlyList<string> SecondaryCategories => Model.Plugin?.PluginInformations.PluginCategory.PluginSecondaryCategories ?? new List<string>();
        public bool HasSecondaryCategories => SecondaryCategories.Count != 0;

        public bool MarkedUninstall => Model.MarkedUninstall;
        public bool CanUninstall => Model.CanUninstall;
        public bool IsEnabledPlugin => !MarkedUninstall;

        public DependencyObject PluginIcon
        {
            get
            {
                if(Model.Plugin == null) {
                    return null!;
                }

                return DispatcherWrapper.Get(() => {
                    try {
                        var scale = ImageLoader.GetPrimaryDpiScale();
                        return Model.Plugin.GetIcon(ImageLoader, new IconScale(IconBox.Small, scale));
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

                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_Setting_Plugins_Item_SupportVersions_Format,
                    new Dictionary<string, string>() {
                        ["MIN"] = PluginUtility.IsUnlimitedVersion(Model.Plugin.PluginInformations.PluginVersions.MinimumSupportVersion) ? Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited : Model.Plugin.PluginInformations.PluginVersions.MinimumSupportVersion.ToString(),
                        ["MAX"] = PluginUtility.IsUnlimitedVersion(Model.Plugin.PluginInformations.PluginVersions.MaximumSupportVersion) ? Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited : Model.Plugin.PluginInformations.PluginVersions.MaximumSupportVersion.ToString()
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

        public ICommand ToggleUninstallMarkCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ToggleUninstallMark();
                RaisePropertyChanged(nameof(MarkedUninstall));
                RaisePropertyChanged(nameof(IsEnabledPlugin));
            }
        ));

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
