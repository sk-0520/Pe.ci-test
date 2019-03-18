using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.ViewModel.IconViewer;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public class LauncherItemViewModel : SingleModelViewModelBase<LauncherItemElement>
    {
        public LauncherItemViewModel(LauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Icon = new LauncherIconViewModel(model.Icon, Logger.Factory);
        }

        #region property

        public LauncherIconViewModel Icon { get; }

        #endregion

        #region command


        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}
