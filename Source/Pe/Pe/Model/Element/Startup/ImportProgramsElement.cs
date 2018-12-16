using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Startup
{
    public class ImportProgramsElement : ElementBase
    {
        public ImportProgramsElement(IMainDatabaseBarrier databaseBarrier, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            DatabaseBarrier = databaseBarrier;
        }

        #region property

        IDatabaseBarrier DatabaseBarrier { get; }
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
            var importItems = ProgramItems
                .Where(i => i.IsImport)
                .ToList()
            ;

            // ap ファイルからランチャーデータ作って
            // db ランチャーアイテム突っ込んで
            // db タグ突っ込んで
            // db グループ作る
            // ap アイコン読み込んで
            // db アイコン突っ込む
            foreach(var importItem in importItems) {
            }

        }

        public Task ImportAsync()
        {
            return Task.Run(() => Import());
        }


        #endregion
    }
}
