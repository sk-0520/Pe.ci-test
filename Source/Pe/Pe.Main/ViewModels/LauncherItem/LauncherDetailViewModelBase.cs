using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
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
        #region variable

        bool _nowLoading;
        bool _nowMainExecuting;

        #endregion

        public LauncherDetailViewModelBase(LauncherItemElement model, IScreen screen, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Screen = screen;
            LauncherToolbarTheme = launcherToolbarTheme;
            Icon = new LauncherIconViewModel(model.Icon, dispatcherWrapper, LoggerFactory);
        }

        #region property

        protected IScreen Screen { get; }
        protected ILauncherToolbarTheme LauncherToolbarTheme { get; }
        public LauncherIconViewModel Icon { get; }

        public string? Name => Model.Name;
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



        #endregion

        #region command

        public ICommand InitializeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                InitializeAsync();
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

        Task InitializeAsync()
        {
            NowLoading = true;
            return InitializeImplAsync().ContinueWith(_ => {
                NowLoading = false;
            });
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

    public static class LauncherItemViewModelFactory
    {
        #region function

        public static LauncherDetailViewModelBase Create(LauncherItemElement model, IScreen screen, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
        {
            switch(model.Kind) {
                case LauncherItemKind.File:
                    return new LauncherFileViewModel(model, screen, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.StoreApp:
                    return new LauncherStoreAppViewModel(model, screen, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Addon:
                    throw new NotImplementedException();

                case LauncherItemKind.Separator:
                    return new LauncherSeparatorViewModel(model, screen, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }
}
