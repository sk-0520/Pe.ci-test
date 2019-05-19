using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public abstract class LauncherFileSystemViewModelBase : LauncherDetailViewModelBase, ILauncherFilePath, ILauncherWorkingDirectoryPath
    {
        #region variable

        FileSystemInfo _fileSystemInfo;

        bool _existsPath;
        bool _existsParentDirectory;
        bool _existsWorkingDirectory;

        #endregion

        public LauncherFileSystemViewModelBase(LauncherItemElement model, Screen screen, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property

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
            () => !NowLoading && ExistsPath
        ));

        public ICommand OpenParentDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenParentDirectory();
            },
            () => !NowLoading && ExistsParentDirectory
        ));

        public ICommand OpenWorkingDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenWorkingDirectory();
            },
            () => !NowLoading && ExistsWorkingDirectory
        ));

        #endregion

        #region function

        protected abstract void RaiseFileSystemInfoChanged();

        protected abstract Task InitializeFileSystemAsync();

        #endregion

        #region ILauncherFilePath

        public bool ExistsPath
        {
            get => this._existsPath;
            protected set => SetProperty(ref this._existsPath, value);
        }

        public bool CanExecutePath => ExistsPath;
        public bool CanCopyPath => ExistsPath;

        public bool ExistsParentDirectory
        {
            get => this._existsParentDirectory;
            protected set => SetProperty(ref this._existsParentDirectory, value);
        }
        public bool CanOpenParentDirectory => ExistsParentDirectory;
        public bool CanCopyParentDirectory => ExistsParentDirectory;

        #endregion

        #region ILauncherWorkingDirectoryPath

        public bool ExistsWorkingDirectory
        {
            get => this._existsWorkingDirectory;
            protected set => SetProperty(ref this._existsWorkingDirectory, value);
        }

        public bool CanOpenWorkingDirectory => ExistsWorkingDirectory;
        public bool CanCopyWorkingDirectory => ExistsWorkingDirectory;


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
                ExistsPath = FileSystemInfo.Exists;
                ExistsParentDirectory = Directory.Exists(Path.GetDirectoryName(FileSystemInfo.FullName));
                NowLoading = false;
            });
        }

        #endregion
    }
}
