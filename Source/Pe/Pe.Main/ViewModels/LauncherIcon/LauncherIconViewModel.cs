using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon
{
    public class LauncherIconViewModel : SingleModelViewModelBase<LauncherIconElement>, IIconPack<IconViewerViewModel>
    {
        #region variable

        IReadOnlyDictionary<IconBox, IconViewerViewModel>? _iconItems;

        #endregion

        public LauncherIconViewModel(LauncherIconElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Small = new IconViewerViewModel(Model.IconImageLoaderPack.Small, dispatcherWrapper, LoggerFactory);
            Normal = new IconViewerViewModel(Model.IconImageLoaderPack.Normal, dispatcherWrapper, LoggerFactory);
            Big = new IconViewerViewModel(Model.IconImageLoaderPack.Big, dispatcherWrapper, LoggerFactory);
            Large = new IconViewerViewModel(Model.IconImageLoaderPack.Large, dispatcherWrapper, LoggerFactory);
        }

        #region property
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

        #region IIconPack

        public IconViewerViewModel Small { get; }
        public IconViewerViewModel Normal { get; }
        public IconViewerViewModel Big { get; }
        public IconViewerViewModel Large { get; }

        public IReadOnlyDictionary<IconBox, IconViewerViewModel> IconItems => this._iconItems ??= new Dictionary<IconBox, IconViewerViewModel>() {
            [IconBox.Small] = Small,
            [IconBox.Normal] = Normal,
            [IconBox.Big] = Big,
            [IconBox.Large] = Large,
        };

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var vm in IconItems.Values) {
                        vm.Dispose();
                    }
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
