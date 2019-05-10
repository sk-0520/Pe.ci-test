using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using Prism.Commands;

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

        public ICommand ExecuteSimpleCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ExecuteMainImplAsync().ConfigureAwait(false);
            },
            () => !NowLoading && Exists
        ));


        #endregion

        #region function

        protected abstract void RaiseFileSystemInfoChanged();

        protected abstract Task InitializeFileSystemAsync();

        #endregion

        #region LauncherItemViewModelBase

        /// <summary>
        /// NOTE: こいつは CanExecute を意図的に固定。
        /// <para><see cref="ExecuteMainCommand"/>と<see cref="ExecuteSimpleCommand"/>で分けておかないとツールバー右クリックでアイテムのメニューが出なくなる気がする</para>
        /// </summary>
        protected override bool CanExecuteMain => true;

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
