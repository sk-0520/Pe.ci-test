using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModel.IconViewer;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon
{
    public interface ILauncherIconViewModel
    {
        #region property

        IconViewerViewModel Small { get; }
        IconViewerViewModel Normal { get; }
        IconViewerViewModel Big { get; }
        IconViewerViewModel Large { get; }

        #endregion
    }

    public class LauncherIconViewModel : SingleModelViewModelBase<LauncherIconElement>, IIconPack<IconViewerViewModel>
    {
        public LauncherIconViewModel(LauncherIconElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Small = new IconViewerViewModel(Model.IconImageLoaderPack.Small, Logger.Factory);
            Normal = new IconViewerViewModel(Model.IconImageLoaderPack.Normal, Logger.Factory);
            Big = new IconViewerViewModel(Model.IconImageLoaderPack.Big, Logger.Factory);
            Large = new IconViewerViewModel(Model.IconImageLoaderPack.Large, Logger.Factory);
        }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IIconPack

        public IconViewerViewModel Small { get; }
        public IconViewerViewModel Normal { get; }
        public IconViewerViewModel Big { get; }
        public IconViewerViewModel Large { get; }

        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}
