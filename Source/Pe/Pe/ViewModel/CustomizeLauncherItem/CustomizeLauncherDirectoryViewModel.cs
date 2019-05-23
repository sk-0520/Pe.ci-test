using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;

namespace ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem
{
    public class CustomizeLauncherDirectoryViewModel : CustomizeLauncherDetailViewModelBase
    {
        public CustomizeLauncherDirectoryViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
