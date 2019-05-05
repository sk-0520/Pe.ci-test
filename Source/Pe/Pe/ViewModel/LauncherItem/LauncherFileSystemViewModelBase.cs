using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public abstract class LauncherFileSystemViewModelBase : LauncherDetailViewModelBase
    {
        #region property

        bool _exists;
        FileSystemInfo _fileSystemInfo;

        #endregion

        public LauncherFileSystemViewModelBase(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property
        public bool Exists
        {
            get => this._exists;
            set => SetProperty(ref this._exists, value);
        }

        public FileSystemInfo FileSystemInfo
        {
            get => this._fileSystemInfo;
            protected set
            {
                if(SetProperty(ref this._fileSystemInfo, value)) {
                    RaiseFileSystemInfoChanged();
                }
            }
        }


        #endregion

        #region command
        #endregion

        #region function

        protected abstract void RaiseFileSystemInfoChanged();

        protected abstract Task InitializeFileSystemAsync();

        #endregion

        #region LauncherItemViewModelBase

        protected override Task InitializeAsyncImpl()
        {
            NowLoading = true;
            return InitializeFileSystemAsync().ContinueWith(_ => {
                NowLoading = false;
            });
        }

        #endregion
    }
}
