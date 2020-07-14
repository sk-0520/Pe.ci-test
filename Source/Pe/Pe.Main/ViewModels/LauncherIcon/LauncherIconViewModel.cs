using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon
{
    [Obsolete]
    public class LauncherIconViewModel: SingleModelViewModelBase<LauncherIconElement>
    {
        #region variable

        #endregion

        public LauncherIconViewModel(LauncherIconElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Icon = new IconViewerViewModel(Model.IconImageLoaderPack, dispatcherWrapper, LoggerFactory);
        }

        #region property

        public IconViewerViewModel Icon { get; }

        #endregion

        #region command
        #endregion

        #region function

        public void Reload()
        {
            ThrowIfDisposed();

            //TODO
        }

        #endregion


        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Icon.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion

    }
}
