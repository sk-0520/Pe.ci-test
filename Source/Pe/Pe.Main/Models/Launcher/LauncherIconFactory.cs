using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherIconFactory
    {
        public LauncherIconFactory(Guid launcherItemId, LauncherItemKind launcherItemKind, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            LauncherItemId = launcherItemId;
            LauncherItemKind = launcherItemKind;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        public Guid LauncherItemId { get; }
        public LauncherItemKind LauncherItemKind { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        #endregion

        #region function

        private IconImageLoaderBase CreateFileIconLoader(IDispatcherWrapper dispatcherWrapper)
        {
            return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
        }

        public object CreateIconSource(IDispatcherWrapper dispatcherWrapper)
        {
            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public object CreateView(IDispatcherWrapper dispatcherWrapper, bool useCache)
        {
            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        var loader = new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
                        return new IconViewerViewModel(loader, dispatcherWrapper, LoggerFactory) {
                            UseCache = useCache,
                        };
                    }

                default:
                    throw new NotImplementedException();
            }
        }


        #endregion
    }
}
