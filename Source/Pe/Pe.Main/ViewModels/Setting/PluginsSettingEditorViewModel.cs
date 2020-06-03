using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginsSettingEditorViewModel: SettingEditorViewModelBase<PluginsSettingEditorElement>
    {
        public PluginsSettingEditorViewModel(PluginsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region SettingEditorViewModelBase
        public override string Header => Properties.Resources.String_Setting_Plugins_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        #endregion

    }
}
