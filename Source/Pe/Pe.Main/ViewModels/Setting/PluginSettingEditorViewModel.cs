using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginSettingEditorViewModel: SingleModelViewModelBase<PluginSettingEditorElement>
    {
        public PluginSettingEditorViewModel(PluginSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        IDispatcherWrapper DispatcherWrapper { get; }

        public string PluginName => Model.Plugin.PluginInformations.PluginIdentifiers.PluginName;
        public Guid PluginId => Model.Plugin.PluginInformations.PluginIdentifiers.PluginId;
        public string PrimaryCategory => Model.Plugin.PluginInformations.PluginCategory.PluginPrimaryCategory;
        public IReadOnlyList<string> SecondaryCategories => Model.Plugin.PluginInformations.PluginCategory.PluginSecondaryCategories;
        public bool HasSecondaryCategories => Model.Plugin.PluginInformations.PluginCategory.PluginSecondaryCategories.Count != 0;

        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }
}
