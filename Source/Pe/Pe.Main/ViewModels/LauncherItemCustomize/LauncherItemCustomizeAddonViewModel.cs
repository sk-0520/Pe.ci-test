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
        }

        #region property

        public UserControl? PreferencesControl { get; set; }

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region LauncherItemCustomizeDetailViewModelBase

        protected override void InitializeImpl()
        {
            if(Model.SupportedPreferences) {
                PreferencesControl = Model.BeginPreferences();
            }
        }

        #endregion
    }
}
