using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeAddonViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        public LauncherItemCustomizeAddonViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            if(!Model.LauncherItemSupportedPreferences) {
                throw new InvalidOperationException(nameof(Model.LauncherItemSupportedPreferences));
            }
        }

        #region property

        public UserControl? PreferencesControl { get; set; }

        public string LauncherItemHeader => Model.GetLauncherItemPluginHeader();

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region LauncherItemCustomizeDetailViewModelBase

        protected override void InitializeImpl()
        {
            if(Model.LauncherItemSupportedPreferences) {
                PreferencesControl = Model.BeginLauncherItemPreferences();
            }
        }

        #endregion
    }
}
