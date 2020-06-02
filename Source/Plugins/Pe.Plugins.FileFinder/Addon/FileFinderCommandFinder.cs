using System;
using System.Collections.Generic;
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
        public FileFinderCommandFinder(IAddonParameter parameter)
        {
            Logger = parameter.LoggerFactory.CreateLogger(GetType());
            DispatcherWrapper = parameter.DispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

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

        IEnumerable<ICommandItem> GetChildren(string description, string displayBase, string directoryPath, string filePattern, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!Directory.Exists(directoryPath)) {
                yield break;
            }


            var fileNameRegex = new Regex(Regex.Escape(filePattern).Replace("\\?", ".").Replace("\\*", ".*"), RegexOptions.IgnoreCase);
            var searchPattern = ConvertSearchPattern(filePattern);
            var dir = new DirectoryInfo(directoryPath);
            var files = dir.EnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);

            Logger.LogTrace("searchPattern = {0}", searchPattern);

            foreach(var file in files) {
                // この演算いやだなぁ

                var fullPath = file.Attributes.HasFlag(FileAttributes.Directory)
                    ? (Path.EndsInDirectorySeparator(file.FullName) ? file.FullName : file.FullName + Path.DirectorySeparatorChar)
                    : file.FullName
                ;
                var item = new FileFinderCommandItem(fullPath);

                if(string.IsNullOrWhiteSpace(filePattern)) {
                    item.HeaderValues.Add(new HitValue(file.Name, false));
                } else {
                    var values = hitValuesCreator.GetHitValues(file.Name, fileNameRegex);
                    item.HeaderValues.AddRange(values);
                }
                item.DescriptionValues.Add(new HitValue(description, false));

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

            var envStartIndex = inputValue.IndexOf('%');
            if(envStartIndex != -1) {
                var envNextIndex = inputValue.IndexOf('%', envStartIndex + 1);
                if(envNextIndex != -1) {
                    var expandedInputValue = Environment.ExpandEnvironmentVariables(inputValue);
                    var inputDirValue = inputValue.Substring(inputValue.LastIndexOf('%'));
                    if(IsPath(expandedInputValue)) {
                        var parts = SplitPath(expandedInputValue);
                        var items = GetChildren("ふぁいるぱす", inputDirValue, parts.directoryPath, parts.filePattern, hitValuesCreator, cancellationToken);
                        foreach(var item in items) {
                            yield return item;
                        }
                    }
                    yield break;
                }
            }

            // 通常のファイル検索
            if(Path.IsPathRooted(inputValue)) {
                if(IsPath(inputValue)) {
                    var parts = SplitPath(inputValue);
                    var items = GetChildren("ファイルパス", inputValue, parts.directoryPath, parts.filePattern, hitValuesCreator, cancellationToken);
                    foreach(var item in items) {
                        yield return item;
                    }
                }
                yield break;
            }

            // PATH からファイル検索

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
