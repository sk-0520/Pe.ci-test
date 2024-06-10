using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeSeparatorViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        public LauncherItemCustomizeSeparatorViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region LauncherItemCustomizeSeparatorViewModel

        protected override void InitializeImpl()
        {
            if(Model.IsLazyLoad) {
                return;
            }
        }

        #endregion
    }
}
