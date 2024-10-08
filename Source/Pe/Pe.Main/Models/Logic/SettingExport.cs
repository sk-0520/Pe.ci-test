using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public record class SettingGroup(
        LauncherGroupId GroupId,
        string GroupName,
        IReadOnlyList<SettingGroupItem> Items
    );

    public record class SettingGroupItem(
        LauncherItemId LauncherItemId,
        string LauncherItemName,
        LauncherItemKind LauncherItemKind
    );

    public record class SettingLauncherItem(
        LauncherItemId LauncherItemId,
        string LauncherItemName,
        LauncherItemKind LauncherItemKind,
        string IconPath,
        long IconIndex,
        string FilePath,
        string FileOption,
        string FileWorkDirectory,
        string Comment
    );

    public record class SettingNote(
        NoteId NoteId,
        string Title,
        string ScreenName,
        string ForegroundColor,
        string BackgroundColor,
        bool IsLocked,
        bool IsTopmost,
        bool IsCompact,
        NoteContentKind ContentKind,
        NoteHiddenMode HiddenMode,
        NoteCaptionPosition CaptionPosition,
        Encoding Encoding,
        string Content
    );

    public class SettingExporter
    {
        public SettingExporter(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            TemporaryDatabaseBarrier = temporaryDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private ITemporaryDatabaseBarrier TemporaryDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        #endregion

        #region function

        public IReadOnlyList<SettingGroup> GetGroups()
        {
            using(var transaction = MainDatabaseBarrier.WaitRead()) {
                var dao = new SettingExporterDomainDao(transaction.Context, DatabaseStatementLoader, transaction.Implementation, LoggerFactory);
                return dao.SelectSettingGroups().ToArray();
            }
        }

        public IReadOnlyList<SettingLauncherItem> GetLauncherItems()
        {
            using(var transaction = MainDatabaseBarrier.WaitRead()) {
                var dao = new SettingExporterDomainDao(transaction.Context, DatabaseStatementLoader, transaction.Implementation, LoggerFactory);
                return dao.SelectSettingLauncherItems().ToArray();
            }
        }

        public IReadOnlyList<SettingNote> GetNotes()
        {
            using(var transaction = MainDatabaseBarrier.WaitRead()) {
                var dao = new SettingExporterDomainDao(transaction.Context, DatabaseStatementLoader, transaction.Implementation, LoggerFactory);
                return dao.SelectSettingNotes().ToArray();
            }
        }

        #endregion
    }
}
