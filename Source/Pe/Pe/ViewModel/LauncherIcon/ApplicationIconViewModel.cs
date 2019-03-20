using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModel.IconViewer;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon
{
    public class ApplicationIconViewModel : ViewModelBase, ILauncherIconViewModel
    {
        public ApplicationIconViewModel(IApplicationIconImageLoaders applicationIconImageLoaders, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Small = new IconViewerViewModel(applicationIconImageLoaders.IconImageLoaders[IconScale.Small], Logger.Factory);
            Normal = new IconViewerViewModel(applicationIconImageLoaders.IconImageLoaders[IconScale.Normal], Logger.Factory);
            Big = new IconViewerViewModel(applicationIconImageLoaders.IconImageLoaders[IconScale.Big], Logger.Factory);
            Large = new IconViewerViewModel(applicationIconImageLoaders.IconImageLoaders[IconScale.Large], Logger.Factory);
        }

        #region property

        public IconViewerViewModel Small { get; }
        public IconViewerViewModel Normal { get; }
        public IconViewerViewModel Big { get; }
        public IconViewerViewModel Large { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}
