using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherToolbarsSettingEditorElement: SettingEditorElementBase
    {
        public LauncherToolbarsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            AllLauncherGroups = allLauncherGroups;
        }

        #region property

        public ObservableCollection<LauncherGroupSettingEditorElement> AllLauncherGroups { get; }
        public ObservableCollection<LauncherToolbarSettingEditorElement> Toolbars { get; } = new ObservableCollection<LauncherToolbarSettingEditorElement>();

        #endregion

        #region function
        #endregion

        #region SettingEditorElementBase

        protected override async Task LoadCoreAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<LauncherToolbarId> launcherToolbarIds;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var ids = launcherToolbarsEntityDao.SelectAllLauncherToolbarIds();
                launcherToolbarIds = ids.ToList();
            }

            var toolbars = new List<LauncherToolbarSettingEditorElement>();
            foreach(var launcherToolbarId in launcherToolbarIds) {
                var element = new LauncherToolbarSettingEditorElement(launcherToolbarId, AllLauncherGroups, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
                toolbars.Add(element);
            }

            foreach(var toolbar in toolbars) {
                await toolbar.InitializeAsync(cancellationToken);
            }
            // 1.プライマリを最上位
            var topToolbar = toolbars.First(i => i.Screen?.Primary ?? true);
            // 2.スクリーン名
            toolbars.Remove(topToolbar);
            var activeToolbars = toolbars
                .Where(i => i.Screen != null)
                .OrderBy(i => i.Screen!.DeviceName)
                .ToList()
            ;
            // 3.なんか適当に並べ替える
            var nonActiveToolbars = toolbars
                .Except(activeToolbars)
                .OrderBy(i => i.ScreenName)
                .ThenBy(i => i.LauncherToolbarId)
            ;

            Toolbars.Clear();
            Toolbars.Add(topToolbar);
            Toolbars.AddRange(activeToolbars);
            Toolbars.AddRange(nonActiveToolbars);
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            foreach(var toolbar in Toolbars) {
                toolbar.Save(contextsPack);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var item in Toolbars) {
                        item.Dispose();
                    }
                    Toolbars.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
