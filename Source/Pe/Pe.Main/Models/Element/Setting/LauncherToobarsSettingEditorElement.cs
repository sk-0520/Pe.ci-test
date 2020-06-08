using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherToobarsSettingEditorElement : SettingEditorElementBase
    {
        public LauncherToobarsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
            AllLauncherGroups = allLauncherGroups;
        }

        #region property

        public ObservableCollection<LauncherGroupSettingEditorElement> AllLauncherGroups { get; }
        public ObservableCollection<LauncherToobarSettingEditorElement> Toolbars { get; } = new ObservableCollection<LauncherToobarSettingEditorElement>();

        #endregion

        #region function
        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            IReadOnlyList<Guid> launcherToolbarIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var ids = launcherToolbarsEntityDao.SelectAllLauncherToolbarIds();
                launcherToolbarIds = ids.ToList();
            }

            var toolbars = new List<LauncherToobarSettingEditorElement>();
            foreach(var launcherToolbarId in launcherToolbarIds) {
                var element = new LauncherToobarSettingEditorElement(launcherToolbarId, AllLauncherGroups, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
                toolbars.Add(element);
            }

            foreach(var toolbar in toolbars) {
                toolbar.Initialize();
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

        protected override void SaveImpl(IDatabaseCommandsPack commandPack)
        {
            foreach(var toolbar in Toolbars) {
                toolbar.Save(commandPack);
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
