using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Startup
{
    public class ImportProgramsElement : ElementBase
    {
        public ImportProgramsElement(IMainDatabaseBarrier databaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            DatabaseBarrier = databaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        IDatabaseBarrier DatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        public ObservableCollection<ProgramElement> ProgramItems { get; } = new ObservableCollection<ProgramElement>();

        #endregion

        #region function

        IEnumerable<FileInfo> GetFiles(DirectoryInfo directory)
        {
            var subDirs = directory.EnumerateDirectories();
            IEnumerable<FileInfo> subFiles = new FileInfo[0];
            foreach(var subDir in subDirs) {
                try {
                    subFiles = GetFiles(subDir);
                } catch(UnauthorizedAccessException ex) {
                    Logger.Warning(ex);
                }
            }

            var files = directory.EnumerateFiles();
            return files.Concat(subFiles);
        }

        void LoadPrograms()
        {
            var dirPaths = new[] {
                Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
                Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                Environment.GetFolderPath(Environment.SpecialFolder.Programs),
            };
            var elements = dirPaths
                .Select(s => new DirectoryInfo(s))
                .SelectMany(d => GetFiles(d))
                .Where(f => PathUtility.IsShortcut(f.Name) || PathUtility.IsProgram(f.Name))
                .GroupBy(f => f.Name)
                .OrderBy(g => g.Key)
                .Select(g => g.First())
                .Select(f => new ProgramElement(f, Logger.Factory) {
                    IsImport = true,
                })
            ;

            foreach(var element in elements) {
                ProgramItems.Add(element);
            }

        }

        public Task LoadProgramsAsync()
        {
            return Task.Run(() => LoadPrograms());
        }

        void Import()
        {
            var launcherCreator = new LauncherCreator(Logger.Factory);

            // ap ファイルからランチャーデータ作って
            var importItems = ProgramItems
                .Where(i => i.IsImport)
                .Select(i => launcherCreator.FromFile(i.FileInfo, true))
                .Select(i => new {
                    Data = i,
                    Tags = launcherCreator.GetTags(new FileInfo(i.Command.Command)).ToList(),
                })
                .ToList()
            ;

            var group = launcherCreator.CreateGroupData("@GROUP");

            using(DatabaseBarrier.Locker.WaitWriteByDefaultTimeout())
            using(var transaction = DatabaseBarrier.Accessor.BeginTransaction()) {
                var launcherItemsDao = new LauncherItemsDao(transaction, StatementLoader, Logger.Factory);
                var launcherTagsDao = new LauncherTagsDao(transaction, StatementLoader, Logger.Factory);
                // db ランチャーアイテム突っ込んで
                foreach(var importItem in importItems) {
                    var codes = launcherItemsDao.SelectFuzzyCodes(importItem.Data.Code);
                    importItem.Data.Code = TextUtility.ToUnique(importItem.Data.Code, codes, StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}-{n}");
                    launcherItemsDao.InsertSimpleNew(importItem.Data);

                    // db タグ突っ込んで
                    launcherTagsDao.InsertNewTags(importItem.Data.LauncherItemId, importItem.Tags);
                }

                // db グループ作る
                var launcherGroupsDao = new LauncherGroupsDao(transaction, StatementLoader, Logger.Factory);
                launcherGroupsDao.InsertNewGroup(group);

                var launcherGroupItemsDao = new LauncherGroupItemsDao(transaction, StatementLoader, Logger.Factory);
                launcherGroupItemsDao.InsertNewItems(group.LauncherGroupId, importItems.Select(i => i.Data.LauncherItemId));

                transaction.Commit();
            }

        }

        public Task ImportAsync()
        {
            return Task.Run(() => Import());
        }


        #endregion
    }
}
