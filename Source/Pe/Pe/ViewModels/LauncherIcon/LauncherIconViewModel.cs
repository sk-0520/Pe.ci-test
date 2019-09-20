using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon
{
    public class LauncherIconViewModel : SingleModelViewModelBase<LauncherIconElement>, IIconPack<IconViewerViewModel>
    {
        public LauncherIconViewModel(LauncherIconElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Small = new IconViewerViewModel(Model.IconImageLoaderPack.Small, dispatcherWapper, LoggerFactory);
            Normal = new IconViewerViewModel(Model.IconImageLoaderPack.Normal, dispatcherWapper, LoggerFactory);
            Big = new IconViewerViewModel(Model.IconImageLoaderPack.Big, dispatcherWapper, LoggerFactory);
            Large = new IconViewerViewModel(Model.IconImageLoaderPack.Large, dispatcherWapper, LoggerFactory);
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
