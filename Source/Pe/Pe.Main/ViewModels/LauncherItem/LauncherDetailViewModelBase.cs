using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using System.Windows.Input;
using Prism.Commands;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Data;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;

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

    public abstract class LauncherDetailViewModelBase : SingleModelViewModelBase<LauncherItemElement>, ILauncherItemId
    {
        #region define

        protected enum IconKind
        {
            Main,
            Tooltip,
        }

        #endregion

        #region variable

        bool _nowLoading;
        bool _nowMainExecuting;

        object? _mainIcon;
        object? _tooltipIcon;

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

        public string ExecuteKeyGesture => KeyGestureGuide.GetLauncherItemKey(LauncherItemId);
        public bool HasExecuteKeyGesture => !string.IsNullOrEmpty(ExecuteKeyGesture);

        #endregion

        #region command

        public ICommand InitializeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                InitializeAsync();
            }
        ));

        public ICommand UninitializeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UninitializeAsync();
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

        protected abstract Task InitializeImplAsync();
        protected abstract Task UninitializeImplAsync();

        Task InitializeAsync()
        {
            NowLoading = true;
            return InitializeImplAsync().ContinueWith(_ => {
                NowLoading = false;
            });
        }

        Task UninitializeAsync()
        {
            return UninitializeImplAsync();
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
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

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
