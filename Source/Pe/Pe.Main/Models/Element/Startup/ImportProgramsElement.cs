using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Startup
{
    public class ImportProgramsElement: ServiceLocatorElementBase
    {
        public ImportProgramsElement(LauncherItemConfiguration launcherItemConfiguration, IMainDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IWindowManager windowManager, IIdFactory idFactory, IDiContainer diContainer, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            LauncherItemConfiguration = launcherItemConfiguration;
            DatabaseBarrier = databaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            WindowManager = windowManager;
            IdFactory = idFactory;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private LauncherItemConfiguration LauncherItemConfiguration { get; }

        private IMainDatabaseBarrier DatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IWindowManager WindowManager { get; }
        private IIdFactory IdFactory { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        public ObservableCollection<ProgramElement> ProgramItems { get; } = new ObservableCollection<ProgramElement>();

        public bool IsRegisteredLauncher { get; private set; }

        #endregion

        #region function

        private IReadOnlyCollection<FileInfo> GetFiles(DirectoryInfo directory)
        {
            ThrowIfDisposed();

            List<FileInfo> result = new List<FileInfo>();
            var files = directory.EnumerateFiles();
            result.AddRange(files);

            var subDirs = directory.EnumerateDirectories();
            foreach(var subDir in subDirs) {
                try {
                    var subFiles = GetFiles(subDir);
                    result.AddRange(subFiles);
                } catch(UnauthorizedAccessException ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
            }

            return result;
        }

        private IReadOnlyList<Regex> GetAutoImportExcludeRegexItems()
        {
            return LauncherItemConfiguration.AutoImportExcludePatterns
                .Select(i => new Regex(i, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace, Timeout.InfiniteTimeSpan))
                .ToList()
            ;
        }

        public async Task LoadProgramsAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var autoImportExcludeRegexItems = GetAutoImportExcludeRegexItems();

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
                .Select(f => new ProgramElement(f, autoImportExcludeRegexItems, DispatcherWrapper, LoggerFactory))
            ;

            foreach(var element in elements) {
                await element.InitializeAsync(cancellationToken);
                ProgramItems.Add(element);
            }

        }

        private void Import()
        {
            ThrowIfDisposed();

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            // ap ファイルからランチャーデータ作って
            var importItems = ProgramItems
                .Where(i => i.IsImport)
                .Select(i => launcherFactory.FromFile(i.FileInfo, true))
                .Where(i => !string.IsNullOrWhiteSpace(i.File.Path)) // 共有ドライブとかね
                .Select(i => new {
                    Data = i,
                    Tags = launcherFactory.GetTags(new FileInfo(i.File.Path)).ToList(),
                })
                .ToList()
            ;

            var groupNames = DatabaseBarrier.ReadData(c => {
                var launcherGroupsDao = new LauncherGroupsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return launcherGroupsDao.SelectAllLauncherGroupNames().ToList();
            });
            var groupName = TextUtility.ToUniqueDefault(Properties.Resources.String_LauncherGroup_ImportItem_Name, groupNames, StringComparison.CurrentCultureIgnoreCase);
            var group = launcherFactory.CreateGroupData(groupName, LauncherGroupKind.Normal);

            using(var context = DatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherTagsDao = new LauncherTagsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherFilesDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                //TODO: db 今現在グループが一つでランチャーアイテムが登録されていなければ消してしまって

                foreach(var importItem in importItems) {
                    // db ランチャーアイテム突っ込んで
                    launcherItemsDao.InsertLauncherItem(importItem.Data.Item, DatabaseCommonStatus.CreateCurrentAccount());

                    // ランチャー種別で突っ込むデータ追加して
                    switch(importItem.Data.Item.Kind) {
                        case LauncherItemKind.File:
                            launcherFilesDao.InsertFile(importItem.Data.Item.LauncherItemId, importItem.Data.File, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherRedoItemsEntityDao.InsertRedoItem(importItem.Data.Item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());
                            break;

                        default:
                            throw new NotImplementedException();
                    }

                    // db タグ突っ込んで
                    launcherTagsDao.InsertTags(importItem.Data.Item.LauncherItemId, importItem.Tags, DatabaseCommonStatus.CreateCurrentAccount());
                }

                // db グループ作る
                var launcherGroupsDao = new LauncherGroupsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var groupStep = launcherFactory.GroupItemStep;
                group.Sequence = launcherGroupsDao.SelectMaxSequence() + groupStep;
                launcherGroupsDao.InsertNewGroup(group, DatabaseCommonStatus.CreateCurrentAccount());

                var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var currentMaxSequence = launcherGroupItemsDao.SelectMaxSequence(group.LauncherGroupId);
                launcherGroupItemsDao.InsertNewItems(group.LauncherGroupId, importItems.Select(i => i.Data.Item.LauncherItemId), currentMaxSequence + launcherFactory.GroupItemsStep, launcherFactory.GroupItemsStep, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }
            IsRegisteredLauncher = true;
        }

        public Task ImportAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.Run(() => Import(), cancellationToken);
            //Import();
            //return Task.CompletedTask;
        }

        #endregion

        #region ContextElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace("not impl");
            return Task.CompletedTask;
        }

        #endregion
    }
}
