using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public class LauncherFileViewModel: LauncherDetailViewModelBase, ILauncherFilePath, ILauncherWorkingDirectoryPath
    {
        #region variable

        private bool _existsPath;
        private bool _canExecutePath;

        private bool _existsParentDirectory;
        private bool _canOpenParentDirectory;
        private bool _canCopyParentDirectory;

        private bool _canCopyOption;

        private bool _existsWorkingDirectory;
        private bool _canOpenWorkingDirectory;
        private bool _canCopyWorkingDirectory;

        #endregion

        public LauncherFileViewModel(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property

        private LauncherFileDetailData? Detail { get; set; }
        private bool DelayWaiting { get; set; }

        #endregion

        #region command

        public ICommand ExecuteExtendsCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenExtendsExecuteView(Screen);
            },
            () => !NowLoading && CanExecutePath
        ));


        public ICommand ExecuteSimpleCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ExecuteMainAsync().ConfigureAwait(false);
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

        public ICommand ShowPropertyCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.ShowProperty();
             },
             () => !NowLoading && CanExecutePath
         ));
        #endregion

        #region function

        private void StartDelayExecute()
        {
            if(DelayWaiting) {
                Logger.LogWarning("抑制待機中: {0}", Model.LauncherItemId);
                return;
            }
            ThrowIfDisposed();

            DelayWaiting = true;

            if(!NowLoading) {
                ExecuteMainAsync();
            } else {
                PropertyChanged += LauncherFileViewModel_PropertyChanged;
            }
        }

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

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            PropertyChanged -= LauncherFileViewModel_PropertyChanged;
        }

        protected override Task ExecuteMainImplAsync()
        {
            if(NowLoading) {
                Logger.LogWarning("読み込み中のため抑制: {0}", Model.LauncherItemId);
                StartDelayExecute();
                return Task.CompletedTask;
            }

            Logger.LogTrace("TODO: 起動準備 {0}, {1}", Model.LauncherItemId, Detail?.FullPath);
            return Task.Run(() => {
                Model.Execute(Screen);
            });
        }

        /// <summary>
        /// NOTE: こいつは CanExecute を意図的に固定。
        /// <para><see cref="ExecuteMainCommand"/>と<see cref="ExecuteSimpleCommand"/>で分けておかないとツールバー右クリックでアイテムのメニューが出なくなる気がする</para>
        /// </summary>
        protected override bool CanExecuteMain => true;


        protected override Task InitializeImplAsync()
        {
            return Task.Run(() => {
                if(Model == null) {
                    return;
                }
                if(IsDisposed) {
                    return;
                }

                Detail = Model.LoadFileDetail();

                var workingDirectoryPath = Environment.ExpandEnvironmentVariables(Detail.PathData.WorkDirectoryPath);
                if(!string.IsNullOrWhiteSpace(workingDirectoryPath)) {
                    CanCopyWorkingDirectory = true;
                    if(Directory.Exists(workingDirectoryPath)) {
                        ExistsWorkingDirectory = true;
                    }
                }
                CanCopyOption = !string.IsNullOrEmpty(Detail.PathData.Option);

                ExistsPath = IOUtility.Exists(Detail.FullPath);
                CanExecutePath = ExistsPath;

                var parentDirectoryPath = Path.GetDirectoryName(Detail.FullPath);
                ExistsParentDirectory = Directory.Exists(parentDirectoryPath);
                CanOpenParentDirectory = ExistsParentDirectory;
                CanCopyParentDirectory = !PathUtility.IsRootName(Detail.FullPath);

                if(ExistsWorkingDirectory && parentDirectoryPath is not null) {
                    var expantedParentDirectoryPath = Environment.ExpandEnvironmentVariables(parentDirectoryPath);
                    CanOpenWorkingDirectory = !PathUtility.IsEquals(workingDirectoryPath, expantedParentDirectoryPath);
                }
            });
        }

        protected override Task UninitializeImplAsync()
        {
            return Task.CompletedTask;
        }

        protected override object GetIcon(IconKind iconKind)
        {
            var factory = Model.CreateLauncherIconFactory();
            var iconSource = factory.CreateIconSource(DispatcherWrapper);
            return factory.CreateView(iconSource, false, DispatcherWrapper);
        }

        #endregion

        void LauncherFileViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(NowLoading)) {
                if(NowLoading) {
                    PropertyChanged -= LauncherFileViewModel_PropertyChanged;
                    DelayWaiting = false;
                    ExecuteMainAsync();
                }
            }
        }
    }
}
