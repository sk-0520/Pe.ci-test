using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder.Addon
{
    /// <summary>
    /// 入力からファイル検索を行う。
    /// <para>もうちと簡単にやるなら Pe.Core から IconLoader を参照するべきかなぁ。</para>
    /// </summary>
    internal class FileFinderCommandFinder: ICommandFinder, IDisposable
    {
        #region define

        class PathItem
        {
            public PathItem(string path)
            {
                Path = path;
                CommandName = System.IO.Path.GetFileNameWithoutExtension(path)!;
            }

            #region property

            public string Path { get; }
            public string CommandName { get; }

            #endregion
        }

        #endregion


        public FileFinderCommandFinder(IAddonParameter parameter)
        {
            Logger = parameter.LoggerFactory.CreateLogger(GetType());
            DispatcherWrapper = parameter.DispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        List<PathItem> PathItems { get; } = new List<PathItem>(512);

        /// <summary>
        /// 隠しファイルを列挙するか。
        /// </summary>
        bool IncludeHiddenFile { get; } = true;
        /// <summary>
        /// PATHの通っている実行ファイルを列挙するか。
        /// </summary>
        bool IncludePath { get; } = true;
        /// <summary>
        /// パスからの列挙において列挙する上限数。
        /// <para>0 で制限しない。</para>
        /// </summary>
        int MaximumPathItem { get; } = 10;
        /// <summary>
        /// パス検索を有効にする入力文字数(以上)。
        /// </summary>
        int PathEnabledInputCharCount { get; } = 0;

        #endregion

        #region function

        string GetDriveName(DriveInfo drive)
        {
            if(drive.DriveType == DriveType.CDRom || drive.DriveType == DriveType.Removable) {
                return string.Format("{0} ({1})", drive.Name, drive.DriveType);
            } else {
                return string.Format("{0} {1} ({2})", drive.VolumeLabel, drive.Name, drive.DriveType);
            }
        }

        bool IsPath(string path)
        {
            if(path.Length < "C:\\".Length) {
                return false;
            }

            // BUGS: UNC は💩
            if(path[1] != Path.VolumeSeparatorChar) {
                return false;
            }

            return path.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) != -1;
        }

        (string directoryPath, string filePattern) SplitPath(string path)
        {
            if(Path.EndsInDirectorySeparator(path)) {
                return (Path.TrimEndingDirectorySeparator(path), string.Empty);
            }

            return (Path.GetDirectoryName(path)!, Path.GetFileName(path)!);
        }

        string ConvertSearchPattern(string filePattern)
        {
            if(string.IsNullOrWhiteSpace(filePattern)) {
                return "*";
            }

            if(filePattern.LastIndexOfAny(new[] { '*', '?' }) == -1) {
                return filePattern + "*";
            }

            return filePattern;
        }

        IEnumerable<ICommandItem> GetOwnerAndChildren(string directoryPath, string filePattern, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!Directory.Exists(directoryPath)) {
                yield break;
            }

            if(string.IsNullOrWhiteSpace(filePattern)) {
                var ownerItem = new FileFinderCommandItem(directoryPath);
                ownerItem.HeaderValues.Add(new HitValue(directoryPath, false));
                ownerItem.DescriptionValues.Add(new HitValue("ディレクトリ", false));
                yield return ownerItem;
            }

            var fileNameRegex = new Regex(Regex.Escape(filePattern).Replace("\\?", ".").Replace("\\*", ".*"), RegexOptions.IgnoreCase);
            var searchPattern = ConvertSearchPattern(filePattern);
            var dir = new DirectoryInfo(directoryPath);
            IEnumerable<FileSystemInfo>? files = null;
            try {
                files = dir.EnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
            } catch(UnauthorizedAccessException ex) {
                Logger.LogWarning(ex, "{0}, {1}", ex.Message, dir.FullName);
                yield break;
            }

            foreach(var file in files) {
                if(!IncludeHiddenFile) {
                    if(file.Attributes.HasFlag(FileAttributes.Hidden)) {
                        continue;
                    }
                }

                // この演算いやだなぁ

                var fullPath = file.Attributes.HasFlag(FileAttributes.Directory)
                    ? (Path.EndsInDirectorySeparator(file.FullName) ? file.FullName : file.FullName + Path.DirectorySeparatorChar)
                    : file.FullName
                ;
                var fullMatchValue = Path.Combine(directoryPath, file.Name);
                var item = new FileFinderCommandItem(fullPath);

                if(string.IsNullOrWhiteSpace(filePattern)) {
                    item.HeaderValues.Add(new HitValue(file.Name, false));
                } else {
                    var values = hitValuesCreator.GetHitValues(file.Name, fileNameRegex);
                    item.HeaderValues.AddRange(values);
                    item.Score = hitValuesCreator.CalcScore(file.Name, values);
                }
                item.DescriptionValues.Add(new HitValue("ファイルパス", false));

                yield return item;
            }
        }

        #endregion

        #region ICommandFinder

        /// <inheritdoc cref="ICommandFinder.IsInitialize"/>
        public bool IsInitialize { get; private set; }

        /// <inheritdoc cref="ICommandFinder.Initialize"/>
        public void Initialize()
        {
            if(IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            var path = Environment.GetEnvironmentVariable("PATH");
            if(!string.IsNullOrWhiteSpace(path)) {
                var executableExtensions = new[] { "exe", "bat", "com" };

                var extRegex = new Regex(@".*\." + string.Join("|", executableExtensions) + "$");
                var dirPaths = path
                    .Split(';')
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                ;

                foreach(var dirPath in dirPaths) {
                    try {
                        var dir = new DirectoryInfo(dirPath);
                        dir.Refresh();
                        if(!dir.Exists) {
                            Logger.LogInformation("存在しない PATH は無視: {0}", dir.FullName);
                            continue;
                        }
                        IEnumerable<FileInfo> files = dir.EnumerateFiles("*", SearchOption.TopDirectoryOnly);
                        foreach(var file in files) {
                            if(extRegex.IsMatch(file.Name)) {
                                var item = new PathItem(file.FullName);
                                PathItems.Add(item);
                            }
                        }
                    } catch(Exception ex) {
                        Logger.LogWarning(ex, ex.Message);
                    }
                }
            }

            IsInitialize = true;
        }

        /// <inheritdoc cref="ICommandFinder.ListupCommandItems(string, Regex, IHitValuesCreator, CancellationToken)"/>
        public IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                // 未入力状態ならドライブ一覧を返す
                var drives = DriveInfo.GetDrives();
                foreach(var drive in drives) {
                    var driveName = GetDriveName(drive);
                    var item = new FileFinderCommandItem(drive.RootDirectory.FullName);
                    item.HeaderValues.Add(new HitValue(driveName, false));
                    item.Score = hitValuesCreator.GetScore(ScoreKind.Initial, 1) - 10;

                    yield return item;
                }
                yield break;
            }

            var gotItems = false;
            var envStartIndex = inputValue.IndexOf('%');
            if(envStartIndex != -1) {
                var envNextIndex = inputValue.IndexOf('%', envStartIndex + 1);
                if(envNextIndex != -1) {
                    var expandedInputValue = Environment.ExpandEnvironmentVariables(inputValue);
                    if(IsPath(expandedInputValue)) {
                        var parts = SplitPath(expandedInputValue);
                        var items = GetOwnerAndChildren(parts.directoryPath, parts.filePattern, hitValuesCreator, cancellationToken);
                        foreach(var item in items) {
                            gotItems = true;
                            yield return item;
                        }
                    }
                }
            }

            // 通常のファイル検索
            if(!gotItems) {
                if(Path.IsPathRooted(inputValue)) {
                    if(IsPath(inputValue)) {
                        var parts = SplitPath(inputValue);
                        var items = GetOwnerAndChildren(parts.directoryPath, parts.filePattern, hitValuesCreator, cancellationToken);
                        foreach(var item in items) {
                            yield return item;
                        }
                    }
                }
            }

            // PATH からファイル検索
            if(IncludePath && PathEnabledInputCharCount <= inputValue.Length) {
                var count = 0;

                foreach(var pathItem in PathItems) {
                    if(cancellationToken.IsCancellationRequested) {
                        break;
                    }
                    var values = hitValuesCreator.GetHitValues(pathItem.CommandName, inputRegex);
                    if(values.Count != 0 && values.Any(i => i.IsHit)) {
                        if(MaximumPathItem != 0 && MaximumPathItem < ++count) {
                            break;
                        }

                        var item = new FileFinderCommandItem(pathItem.Path, pathItem.CommandName);
                        item.HeaderValues.AddRange(values);
                        item.DescriptionValues.Add(new HitValue("%PATH%", false));
                        item.Score = hitValuesCreator.CalcScore(pathItem.CommandName, values) - 3;

                        yield return item;
                    }
                }
            }

            yield break;
        }

        /// <inheritdoc cref="ICommandFinder.Refresh"/>
        public void Refresh()
        { }

        #endregion

        #region IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue) {
                if(disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                this.disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~FileCommandFinder()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
