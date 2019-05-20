using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.ViewModel.IconViewer;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon;
using System.Windows.Input;
using Prism.Commands;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
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

    public abstract class LauncherDetailViewModelBase : SingleModelViewModelBase<LauncherItemElement>
    {
        #region variable

        bool _nowLoading;

        #endregion

        public LauncherDetailViewModelBase(LauncherItemElement model, Screen screen, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Screen = screen;
            LauncherToolbarTheme = launcherToolbarTheme;
            Icon = new LauncherIconViewModel(model.Icon, dispatcherWapper, Logger.Factory);
        }

        #region property

        protected Screen Screen { get; }
        protected ILauncherToolbarTheme LauncherToolbarTheme { get; }
        public LauncherIconViewModel Icon { get; }

        public string Name => Model.Name;
        public string Comment => Model.Comment;

        public bool NowLoading
        {
            get => this._nowLoading;
            set => SetProperty(ref this._nowLoading, value);
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
            return InitializeImplAsync();
        }

        protected abstract Task ExecuteMainImplAsync();

        protected Task ExecuteMainAsync()
        {
            if(CanExecuteMain) {
                return ExecuteMainImplAsync();
            }
            return Task.CompletedTask;
        }

        #endregion

        #region SingleModelViewModelBase
        #endregion
    }

    public static class LauncherItemViewModelFactory
    {
        #region function

        public static LauncherDetailViewModelBase Create(LauncherItemElement model, Screen screen, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
        {
            switch(model.Kind) {
                case LauncherItemKind.File:
                    return new LauncherFileViewModel(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Command:
                    return new LauncherCommandViewModel(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Script:
                    return new LauncherScriptViewModel(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Directory:
                    return new LauncherDirectoryViewModel(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Embedded:
                    return new LauncherEmbeddedViewModel(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Separator:
                    return new LauncherSeparatorViewModel(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }
}
