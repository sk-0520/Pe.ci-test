using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Addon
{
    /// <summary>
    /// 入力からファイル検索を行う。
    /// <para>もうちと簡単にやるなら Pe.Core から IconLoader を参照するべきかなぁ。</para>
    /// </summary>
    internal class FileFinderCommandFinder: ICommandFinder, IDisposable
    {
        #region define

        private  class PathItem
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
            AddonExecutor = parameter.AddonExecutor;
            ImageLoader = parameter.ImageLoader;
            DispatcherWrapper = parameter.DispatcherWrapper;
        }

        #region property

        private ILogger Logger { get; }
        private IAddonExecutor AddonExecutor { get; }
        private IImageLoader ImageLoader { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private List<PathItem> PathItems { get; } = new List<PathItem>(512);

        /// <summary>
        /// 隠しファイルを列挙するか。
        /// </summary>
        private bool IncludeHiddenFile { get; set; }
        /// <summary>
        /// PATHの通っている実行ファイルを列挙するか。
        /// </summary>
        private bool IncludePath { get; set; }
        /// <summary>
        /// パスからの列挙において列挙する上限数。
        /// <para>0 で制限しない。</para>
        /// </summary>
        private int MaximumPathItem { get; set; }
        /// <summary>
        /// パス検索を有効にする入力文字数(以上)。
        /// </summary>
        private int PathEnabledInputCharCount { get; set; }

        #endregion

        #region function

        private string GetDriveName(DriveInfo drive)
        {
            if(drive.DriveType == DriveType.CDRom || drive.DriveType == DriveType.Removable) {
                return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", drive.Name, drive.DriveType);
            } else {
                return string.Format(CultureInfo.InvariantCulture, "{0} {1} ({2})", drive.VolumeLabel, drive.Name, drive.DriveType);
            }
        }

        private bool IsPath(string path)
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

        private (string directoryPath, string filePattern) SplitPath(string path)
        {
            if(Path.EndsInDirectorySeparator(path)) {
                return (Path.TrimEndingDirectorySeparator(path), string.Empty);
            }

            return (Path.GetDirectoryName(path)!, Path.GetFileName(path)!);
        }

        private string ConvertSearchPattern(string filePattern)
        {
            if(string.IsNullOrWhiteSpace(filePattern)) {
                return "*";
            }

            if(filePattern.LastIndexOfAny(new[] { '*', '?' }) == -1) {
                return filePattern + "*";
            }

            return filePattern;
        }

        private IEnumerable<ICommandItem> GetOwnerAndChildren(string directoryPath, string filePattern, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!Directory.Exists(directoryPath)) {
                yield break;
            }

            if(string.IsNullOrWhiteSpace(filePattern)) {
                var ownerItem = new FileFinderCommandItem(directoryPath, ImageLoader, AddonExecutor);
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
                var item = new FileFinderCommandItem(fullPath, ImageLoader, AddonExecutor);

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

        /// <inheritdoc cref="ICommandFinder.IsInitialized"/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc cref="ICommandFinder.Initialize"/>
        public void Initialize()
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
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

            IsInitialized = true;
        }

        /// <inheritdoc cref="ICommandFinder.EnumerateCommandItemsAsync(string, Regex, IHitValuesCreator, CancellationToken)"/>
#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        public async IAsyncEnumerable<ICommandItem> EnumerateCommandItemsAsync(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, [EnumeratorCancellation] CancellationToken cancellationToken)
#pragma warning restore CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                // 未入力状態ならドライブ一覧を返す
                var drives = DriveInfo.GetDrives();
                foreach(var drive in drives) {
                    var driveName = GetDriveName(drive);
                    var item = new FileFinderCommandItem(drive.RootDirectory.FullName, ImageLoader, AddonExecutor);
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

                        var item = new FileFinderCommandItem(pathItem.Path, pathItem.CommandName, ImageLoader, AddonExecutor);
                        item.HeaderValues.AddRange(values);
                        item.DescriptionValues.Add(new HitValue("%PATH%", false));
                        item.Score = hitValuesCreator.CalcScore(pathItem.CommandName, values) - 3;

                        yield return item;
                    }
                }
            }

            yield break;
        }

        /// <inheritdoc cref="ICommandFinder.Refresh(IPluginContext)"/>
        public void Refresh(IPluginContext pluginContext)
        {
            FileFinderSetting? setting;
            if(!pluginContext.Storage.Persistence.Normal.TryGet<FileFinderSetting>(FileFinderConstants.MainSettengKey, out setting)) {
                setting = new FileFinderSetting();
            }

            IncludeHiddenFile = setting.IncludeHiddenFile;
            IncludePath = setting.IncludePath;
            MaximumPathItem = setting.MaximumPathItem;
            PathEnabledInputCharCount = setting.PathEnabledInputCharCount;
        }

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
