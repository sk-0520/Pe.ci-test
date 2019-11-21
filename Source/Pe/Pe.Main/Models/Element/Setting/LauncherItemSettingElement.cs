using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemSettingElement : ElementBase
    {
        public LauncherItemSettingElement(ObservableCollection<Guid> launcherItemIds, ObservableCollection<LauncherIconElement> launcherIconElements, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemIds = launcherItemIds;
            LauncherIconElements = launcherIconElements;
            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }

        #region property
        public ObservableCollection<Guid> LauncherItemIds { get; }
        public ObservableCollection<LauncherIconElement> LauncherIconElements { get; }
        IOrderManager OrderManager { get; }
        IClipboardManager ClipboardManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }

        public ObservableCollection<LauncherItemCustomizeSettingElement> CustomizeItems { get; } = new ObservableCollection<LauncherItemCustomizeSettingElement>();

        #endregion

        #region function

        public void RemoveItem(Guid launcherItemId)
        {
            var item = CustomizeItems.First(i => i.LauncherItemId == launcherItemId);
            CustomizeItems.Remove(item);
            LauncherItemIds.Remove(item.LauncherItemId);
            item.Dispose();
        }

        public Guid CreateItem(LauncherItemKind kind)
        {
            var newItemId = IdFactory.CreateLauncherItemId();
            LauncherItemIds.Add(newItemId);



            return newItemId;
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            foreach(var launcherItemId in LauncherItemIds) {
                var iconElement = LauncherIconElements.First(i => i.LauncherItemId == launcherItemId);
                var customizeElement = new LauncherItemCustomizeSettingElement(launcherItemId, iconElement, ClipboardManager, LoggerFactory);
                customizeElement.Initialize();
                CustomizeItems.Add(customizeElement);
            }
        }

        #endregion
    }
}
