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
        bool _canExecutePath;

        bool _existsParentDirectory;
        bool _canOpenParentDirectory;
        bool _canCopyParentDirectory;

        bool _canCopyOption;

        bool _existsWorkingDirectory;
        bool _canOpenWorkingDirectory;
        bool _canCopyWorkingDirectory;

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
            () => !NowLoading && CanExecutePath
        ));

        public ICommand OpenParentDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenParentDirectory();
            },
            () => !NowLoading && CanOpenParentDirectory
        ));

        public ICommand CopyOptionCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.CopyOption();
            },
            () => !NowLoading && CanCopyOption
        ));
        public ICommand CopyExecutePathCommand => GetOrCreateCommand(() => new DelegateCommand(
              () => {
                  Model.CopyExecutePath();
              },
              () => !NowLoading && CanCopyPath
          ));

        public ICommand CopyParentDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.CopyParentDirectory();
             },
             () => !NowLoading && CanCopyParentDirectory
         ));


        public ICommand OpenWorkingDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenWorkingDirectory();
            },
            () => !NowLoading && CanOpenWorkingDirectory
        ));
        public ICommand CopyWorkingDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.CopyWorkingDirectory();
            },
            () => !NowLoading && CanCopyWorkingDirectory
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

        public bool CanExecutePath
        {
            get => this._canExecutePath;
            protected set => SetProperty(ref this._canExecutePath, value);
        }
        public bool CanCopyPath { get; } = true;

        public bool ExistsParentDirectory
        {
            get => this._existsParentDirectory;
            protected set => SetProperty(ref this._existsParentDirectory, value);
        }
        public bool CanOpenParentDirectory
        {
            get => this._canOpenParentDirectory;
            protected set => SetProperty(ref this._canOpenParentDirectory, value);
        }

        public bool CanCopyParentDirectory
        {
            get => this._canCopyParentDirectory;
            protected set => SetProperty(ref this._canCopyParentDirectory, value);
        }

        public bool CanCopyOption
        {
            get => this._canCopyOption;
            protected set => SetProperty(ref this._canCopyOption, value);
        }

        #endregion

        #region ILauncherWorkingDirectoryPath

        public bool ExistsWorkingDirectory
        {
            get => this._existsWorkingDirectory;
            protected set => SetProperty(ref this._existsWorkingDirectory, value);
        }

        public bool CanOpenWorkingDirectory
        {
            get => this._canOpenWorkingDirectory;
            protected set => SetProperty(ref this._canOpenWorkingDirectory, value);
        }
        public bool CanCopyWorkingDirectory
        {
            get => this._canCopyWorkingDirectory;
            protected set => SetProperty(ref this._canCopyWorkingDirectory, value);
        }


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
                CanExecutePath = ExistsPath;

                var parentDirectoryPath = Path.GetDirectoryName(FileSystemInfo.FullName);
                ExistsParentDirectory = Directory.Exists(parentDirectoryPath);
                CanOpenParentDirectory = ExistsParentDirectory;
                CanCopyParentDirectory = true; //TODO: ドライブとか

                NowLoading = false;
            });
        }

        #endregion
    }
}
