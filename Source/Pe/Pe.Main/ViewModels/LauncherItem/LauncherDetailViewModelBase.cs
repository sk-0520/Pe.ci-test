using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public interface ILauncherFilePath
    {
        #region property

        bool ExistsPath { get; }
        bool CanExecutePath { get; }
        bool CanCopyPath { get; }

        bool ExistsParentDirectory { get; }
        bool CanOpenParentDirectory { get; }
        bool CanCopyParentDirectory { get; }

        bool CanCopyOption { get; }

        #endregion
    }

    public interface ILauncherWorkingDirectoryPath
    {
        #region property

        bool ExistsWorkingDirectory { get; }
        bool CanOpenWorkingDirectory { get; }
        bool CanCopyWorkingDirectory { get; }

        #endregion
    }

    public abstract class LauncherDetailViewModelBase: SingleModelViewModelBase<LauncherItemElement>, ILauncherItemId
    {
        #region define

        protected enum IconKind
        {
            Main,
            Tooltip,
        }

        #endregion

        #region variable

        private bool _nowLoading;
        private bool _nowMainExecuting;

        private object? _mainIcon;
        private object? _tooltipIcon;

        #endregion

        protected LauncherDetailViewModelBase(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Screen = screen;
            KeyGestureGuide = keyGestureGuide;
            DispatcherWrapper = dispatcherWrapper;
            LauncherToolbarTheme = launcherToolbarTheme;
        }

        #region property

        protected IScreen Screen { get; }
        protected IKeyGestureGuide KeyGestureGuide { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ILauncherToolbarTheme LauncherToolbarTheme { get; }
        public object MainIcon => this._mainIcon ??= GetIcon(IconKind.Main);
        public object TooltipIcon => this._tooltipIcon ??= GetIcon(IconKind.Tooltip);

        public virtual string? Name => Model.Name;
        public string? Comment => Model.Comment;
        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        public bool NowLoading
        {
            get => this._nowLoading;
            private set => SetProperty(ref this._nowLoading, value);
        }

        public bool NowMainExecuting
        {
            get => this._nowMainExecuting;
            set => SetProperty(ref this._nowMainExecuting, value);
        }


        protected virtual bool CanExecuteMain => true;

        public ObservableCollection<string>? ExecuteKeyGestures { get; private set; }
        public bool HasExecuteKeyGestures { get; set; }

        #endregion

        #region command

        public ICommand LoadCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                LoadAsync();
            }
        ));

        public ICommand UnloadCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UnloadAsync();
            }
        ));

        public ICommand ExecuteMainCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ExecuteMainAsync();
            },
            () => CanExecuteMain
        ));

        public ICommand CustomizeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenCustomizeView(Screen);
            }
        ));

        #endregion

        #region function

        protected abstract Task LoadImplAsync();
        protected abstract Task UnloadImplAsync();

        private Task LoadAsync()
        {
            NowLoading = true;
            return LoadImplAsync().ContinueWith(_ => {
                var keys = KeyGestureGuide.GetLauncherItemKeys(LauncherItemId);
                ExecuteKeyGestures = new ObservableCollection<string>(keys);
                HasExecuteKeyGestures = ExecuteKeyGestures.Any();

                RaisePropertyChanged(nameof(HasExecuteKeyGestures));
                RaisePropertyChanged(nameof(ExecuteKeyGestures));

                NowLoading = false;
            });
        }

        private Task UnloadAsync()
        {
            return UnloadImplAsync();
        }

        protected abstract Task ExecuteMainImplAsync();

        protected Task ExecuteMainAsync()
        {
            if(CanExecuteMain) {
                NowMainExecuting = true;
                return ExecuteMainImplAsync().ContinueWith(_ => {
                    NowMainExecuting = false;
                });
            }
            return Task.CompletedTask;
        }

        protected abstract object GetIcon(IconKind iconKind);

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(this._mainIcon is IDisposable mainIcon) {
                        mainIcon.Dispose();
                        this._mainIcon = null;
                    }
                    if(this._tooltipIcon is IDisposable tooltipIcon) {
                        tooltipIcon.Dispose();
                        this._tooltipIcon = null;
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId => Model.LauncherItemId;

        #endregion
    }

    public static class LauncherItemViewModelFactory
    {
        #region function

        public static LauncherDetailViewModelBase Create(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
        {
            switch(model.Kind) {
                case LauncherItemKind.File:
                    return new LauncherFileViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                //case LauncherItemKind.StoreApp:
                //    return new LauncherStoreAppViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Addon:
                    return new LauncherAddonViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                //case LauncherItemKind.Separator:
                //    return new LauncherSeparatorViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }
}
