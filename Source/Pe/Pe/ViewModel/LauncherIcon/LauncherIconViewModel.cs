using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon
{
    public class LauncherIconViewModel : ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel.SingleModelViewModelBase<ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon.LauncherIconElement>
    {
        public LauncherIconViewModel(LauncherIconElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
        }

        #region property

        public ContentTypeTextNet.Pe.Main.ViewModel.IconViewer.IconViewerViewModel Small { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}
