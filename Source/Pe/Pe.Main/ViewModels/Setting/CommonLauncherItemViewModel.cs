using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public sealed class CommonLauncherItemViewModel : SingleModelViewModelBase<CommonLauncherItemElement>, ILauncherItemId
    {
        public CommonLauncherItemViewModel(CommonLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public string Code => Model.Code;
        public string Name => Model.Name;
        public LauncherItemKind Kind => Model.Kind;

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion
    }
}
