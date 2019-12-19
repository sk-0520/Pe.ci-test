using System;
using System.Collections.Generic;
using System.Linq;
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
        {
            Toolbars = Model.Toolbars
                .Select(i => new LauncherToobarSettingEditorViewModel(i, DispatcherWrapper, LoggerFactory))
                .ToList()
            ;
        }

        #region property

        public IReadOnlyList<LauncherToobarSettingEditorViewModel> Toolbars { get; }

        #endregion

        #region function

        #endregion

        #region command

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_LauncherToolbars;

        public override void Flush()
        {
        }

        #endregion
    }
}
