using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public abstract class SettingEditorViewModelBase<TSettingEditorElement> : SingleModelViewModelBase<TSettingEditorElement>
        where TSettingEditorElement: SettingEditorElementBase
    {
        public SettingEditorViewModelBase(TSettingEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }
}
