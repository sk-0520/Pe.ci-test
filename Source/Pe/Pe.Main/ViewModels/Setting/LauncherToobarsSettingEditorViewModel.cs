using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherToobarsSettingEditorViewModel : SettingEditorViewModelBase<LauncherToobarsSettingEditorElement>
    {
        public LauncherToobarsSettingEditorViewModel(LauncherToobarsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property


        #endregion

        #region function

        #endregion

        #region command

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => throw new NotImplementedException();

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
