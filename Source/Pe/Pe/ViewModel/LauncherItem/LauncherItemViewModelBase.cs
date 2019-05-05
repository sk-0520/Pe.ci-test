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

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public abstract class LauncherItemViewModelBase : SingleModelViewModelBase<LauncherItemElement>
    {
        #region variable

        bool _nowLoading;

        #endregion

        public LauncherItemViewModelBase(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            LauncherToolbarTheme = launcherToolbarTheme;
            Icon = new LauncherIconViewModel(model.Icon, dispatcherWapper, Logger.Factory);
        }

        #region property

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

        #endregion

        #region function

        protected abstract Task InitializeAsyncImpl();

        Task InitializeAsync()
        {
            return InitializeAsyncImpl();
        }

        protected abstract Task ExecuteMainImplAsync();

        Task ExecuteMainAsync()
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

        public static LauncherItemViewModelBase Create(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
        {
            switch(model.Kind) {
                case LauncherItemKind.File:
                    return new LauncherFileItemViewModel(model, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Command:
                    return new LauncherCommandItemViewModel(model, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Script:
                    return new LauncherScriptItemViewModel(model, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Directory:
                    return new LauncherDirectoryItemViewModel(model, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Embedded:
                    return new LauncherEmbeddedItemViewModel(model, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Separator:
                    return new LauncherSeparatorItemViewModel(model, dispatcherWapper, launcherToolbarTheme, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }
}
