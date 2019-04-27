using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.ViewModel.IconViewer;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public abstract class LauncherItemViewModelBase : SingleModelViewModelBase<LauncherItemElement>
    {
        public LauncherItemViewModelBase(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Icon = new LauncherIconViewModel(model.Icon, dispatcherWapper, Logger.Factory);
        }

        #region property

        public string Name => Model.Name;
        public string Comment => Model.Comment;
        public LauncherIconViewModel Icon { get; }

        #endregion

        #region command


        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase
        #endregion
    }

    public static class LauncherItemViewModelFactory
    {
        #region function

        public static LauncherItemViewModelBase Create(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
        {
            switch(model.Kind) {
                case LauncherItemKind.File:
                    return new LauncherFileItemViewModel(model, dispatcherWapper, loggerFactory);

                case LauncherItemKind.Command:
                    return new LauncherCommandItemViewModel(model, dispatcherWapper, loggerFactory);

                case LauncherItemKind.Embedded:
                    return new LauncherEmbeddedItemViewModel(model, dispatcherWapper, loggerFactory);

                case LauncherItemKind.Separator:
                    return new LauncherSeparatorItemViewModel(model, dispatcherWapper, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }
}
