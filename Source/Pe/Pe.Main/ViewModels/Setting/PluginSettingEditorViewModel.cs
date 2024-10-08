using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginSettingEditorViewModel: SingleModelViewModelBase<PluginSettingEditorElement>
    {
        #region variable

        private UserControl? _settingControl;

        #endregion

        public PluginSettingEditorViewModel(PluginSettingEditorElement model, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ImageLoader = imageLoader;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private IImageLoader ImageLoader { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        public string PluginName => Model.PluginState.PluginName;
        public string PluginVersion
        {
            get
            {
                var versionConverter = new VersionConverter();
                return versionConverter.ConvertNormalVersion(Model.PluginVersion);
            }
        }
        public PluginId PluginId => Model.PluginId;

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
                        Logger.LogError(ex, "[{0}] {1}, {2}", Model.Plugin.PluginInformation.PluginIdentifiers.PluginName, ex.Message, Model.Plugin.PluginInformation.PluginIdentifiers.PluginId);
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

                var unlimitedMinVersion = PluginUtility.IsUnlimitedVersion(Model.Plugin.PluginInformation.PluginVersions.MinimumSupportVersion);
                var unlimitedMaxVersion = PluginUtility.IsUnlimitedVersion(Model.Plugin.PluginInformation.PluginVersions.MaximumSupportVersion);

                if(unlimitedMinVersion && unlimitedMaxVersion) {
                    return Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited;
                }

                var versionConverter = new VersionConverter();

                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_Setting_Plugins_Item_SupportVersions_Format,
                    new Dictionary<string, string>() {
                        ["MIN"] = unlimitedMinVersion ? Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited : versionConverter.ConvertNormalVersion(Model.Plugin.PluginInformation.PluginVersions.MinimumSupportVersion),
                        ["MAX"] = unlimitedMaxVersion ? Properties.Resources.String_Setting_Plugins_Item_SupportVersion_Unlimited : versionConverter.ConvertNormalVersion(Model.Plugin.PluginInformation.PluginVersions.MaximumSupportVersion)
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

        private ICommand? _ToggleUninstallMarkCommand;
        public ICommand ToggleUninstallMarkCommand => this._ToggleUninstallMarkCommand ??= new DelegateCommand(
            () => {
                Model.ToggleUninstallMark();
                RaisePropertyChanged(nameof(MarkedUninstall));
                RaisePropertyChanged(nameof(IsEnabledPlugin));
            }
        );

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
