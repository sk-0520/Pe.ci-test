/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;

    public static class IndexItemUtility
    {
        /// <summary>
        /// インデックスデータ種別からインデックスのボディファイルのファイル種別を取得する。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <returns></returns>
        public static FileType GetBodyFileType(IndexKind indexKind)
        {
            var map = new Dictionary<IndexKind, FileType>() {
                { IndexKind.Note, Constants.fileTypeNoteBody },
                { IndexKind.Template, Constants.fileTypeTemplateBody },
                { IndexKind.Clipboard, Constants.fileTypeClipboardBody },
            };

            return map[indexKind];
        }

        /// <summary>
        /// インデックスデータ種別からボディファイルの配置ディレクトリを取得する。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="variableConstants"></param>
        /// <returns></returns>
        public static string GetBodyFileParentDirectory(IndexKind indexKind, VariableConstants variableConstants)
        {
            var map = new Dictionary<IndexKind, string>() {
                { IndexKind.Note, variableConstants.UserSettingNoteDirectoryPath },
                { IndexKind.Template, variableConstants.UserSettingTemplateDirectoryPath },
                { IndexKind.Clipboard, variableConstants.UserSettingClipboardDirectoryPath},
            };

            return map[indexKind];
        }

        /// <summary>
        /// インデックスのボディファイルの名前を取得する。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="fileType"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetBodyFileName(IndexKind indexKind, FileType fileType, Guid guid)
        {
            var ext = new Dictionary<FileType, string>() {
                { FileType.Json,   Constants.ExtensionJsonFile },
                { FileType.Binary, Constants.ExtensionBinaryFile },
            };

            var map = new Dictionary<string, string>() {
                { Constants.keyGuidName, guid.ToString("D") },
                { Constants.keyIndexExt, ext[fileType]},
            };
            return Constants.indexBodyBaseFileName.ReplaceFromDictionary(map);
        }
        /// <summary>
        /// インデックスのボディファイルの名前を取得する。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetBodyFileName(IndexKind indexKind, Guid guid)
        {
            return GetBodyFileName(indexKind, GetBodyFileType(indexKind), guid);
        }

        /// <summary>
        /// インデックスのボディファイルパスを取得。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="fileType"></param>
        /// <param name="guid"></param>
        /// <param name="variableConstants"></param>
        /// <returns></returns>
        public static string GetBodyFilePath(IndexKind indexKind, FileType fileType, Guid guid, VariableConstants variableConstants)
        {
            var dirPath = IndexItemUtility.GetBodyFileParentDirectory(indexKind, variableConstants);
            var fileName = IndexItemUtility.GetBodyFileName(indexKind, fileType, guid);
            var path = Path.Combine(dirPath, fileName);

            return path;
        }
        /// <summary>
        /// インデックスのボディファイルパスを取得。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <param name="variableConstants"></param>
        /// <returns></returns>
        public static string GetBodyFilePath(IndexKind indexKind, Guid guid, VariableConstants variableConstants)
        {
            return GetBodyFilePath(indexKind, GetBodyFileType(indexKind), guid, variableConstants);
        }

        /// <summary>
        /// インデックスのボディファイル格納アーカイブパスを取得。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="variableConstants"></param>
        /// <returns></returns>
        public static string GetBodyArchiveFilePath(IndexKind indexKind, VariableConstants variableConstants)
        {
            var dir = GetBodyFileParentDirectory(indexKind, variableConstants);
            var path = Path.Combine(dir, Constants.bodyArchiveFileName);
            return path;
        }

        /// <summary>
        /// ボディファイルを削除。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <param name="archive"></param>
        /// <param name="appNonProcess"></param>
        /// <returns></returns>
        public static bool RemoveBody(IndexKind indexKind, Guid guid, IndexBodyArchive archive, IAppNonProcess appNonProcess)
        {
            var path = IndexItemUtility.GetBodyFilePath(indexKind, guid, appNonProcess.VariableConstants);
            var expandedPath = Environment.ExpandEnvironmentVariables(path);
            try {
                File.Delete(path);
                return true;
            } catch(Exception ex) {
                appNonProcess.Logger.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// インデックスデータをGC。
        /// </summary>
        /// <typeparam name="TItemModel"></typeparam>
        /// <param name="indexKind"></param>
        /// <param name="items"></param>
        /// <param name="archive"></param>
        /// <param name="appNonProcess"></param>
        public static void GarbageCollectionBody<TItemModel>(IndexKind indexKind, IndexItemCollectionModel<TItemModel> items, IndexBodyArchive archive, IAppNonProcess appNonProcess)
            where TItemModel : IndexItemModelBase
        {
            var parentDirPath = Environment.ExpandEnvironmentVariables(GetBodyFileParentDirectory(indexKind, appNonProcess.VariableConstants));
            if(!Directory.Exists(parentDirPath)) {
                return;
            }
            // 実ファイルは存在するがインデックスに存在しないものを破棄
            var searchPattern = "*" + Path.GetExtension(GetBodyFileName(indexKind, GetBodyFileType(indexKind), Guid.Empty));
            var fileNameList = Directory
                .EnumerateFiles(parentDirPath, searchPattern, SearchOption.TopDirectoryOnly)
                .Select(s => Path.GetFileName(s))
            ;
            var itemList = items.ToList();
            var removeTargetList = new List<Guid>();
            foreach(var fileName in fileNameList) {
                var guidName = Path.GetFileNameWithoutExtension(fileName);
                var targetIndex = itemList.FindIndex(i => string.Compare(i.Id.ToString(), guidName, true) == 0);
                if(targetIndex == -1) {
                    removeTargetList.Add(new Guid(guidName));
                } else {
                    itemList.RemoveAt(targetIndex);
                }
            }

            foreach(var removeFileGuid in removeTargetList) {
                RemoveBody(indexKind, removeFileGuid, archive, appNonProcess);
            }

            // 一時データ削除
            AppUtility.GarbageCollectionTemporaryFile(parentDirPath, appNonProcess.Logger);

            // データアーカイブ
            var timestamp = DateTime.Now;
            GarbageCollectionBodyArchive(indexKind, items, archive, timestamp, Constants.bodyArchiveTimeSpan, Constants.bodyArchiveFileSize, appNonProcess);
        }

        /// <summary>
        /// アーカイブのGC。
        /// </summary>
        /// <typeparam name="TItemModel"></typeparam>
        /// <param name="indexKind"></param>
        /// <param name="items"></param>
        /// <param name="archive"></param>
        /// <param name="archiveTimestamp"></param>
        /// <param name="fileSize"></param>
        /// <param name="appNonProcess"></param>
        static void GarbageCollectionBodyArchive<TItemModel>(IndexKind indexKind, IndexItemCollectionModel<TItemModel> items, IndexBodyArchive archive, DateTime archiveBaseTime, TimeSpan archiveTimeSpan, int fileSize, IAppNonProcess appNonProcess)
            where TItemModel : IndexItemModelBase
        {
            //var archivePath = Environment.ExpandEnvironmentVariables(GetBodyArchiveFilePath(indexKind, appNonProcess.VariableConstants));
            if(archive.EnabledArchive) {
                // アーカイブには存在するがインデックスに存在しないものを破棄
                // TODO: 共通化できそうなので今は未実装
                //var itemIds = items
                //    .Select(i => GetBodyFileName(indexKind, GetBodyFileType(indexKind), i.Id))
                //    .ToArray()
                //;
            }

            var targetTime = archiveBaseTime - archiveTimeSpan;
            var oldItems = items
                .Where(i => i.History.UpdateTimestamp < targetTime)
                .ToArray()
            ;
            if(oldItems.Any()) {
                if(!archive.EnabledArchive) {
                    archive.OpenArchiveFile(indexKind, appNonProcess.VariableConstants);
                }
                var removePathList = new List<string>(oldItems.Length);
                foreach(var item in oldItems) {
                    var itemName = GetBodyFileName(indexKind, GetBodyFileType(indexKind), item.Id);

                    var itemPath = Environment.ExpandEnvironmentVariables(GetBodyFilePath(indexKind, item.Id, appNonProcess.VariableConstants));
                    if(File.Exists(itemPath)) {
                        // インデックスに存在するがファイルが存在しない物は無視する
                        // NOTE: 不整合を起こしていることになるが読み込み時に新規作成される実装のため該当データじたいは不正ではない
                        continue;
                    }

                    var fileInfo = new FileInfo(itemPath);
                    if(fileInfo.Length > fileSize) {
                        // 指定サイズより大きい場合は除外する
                        continue;
                    }
                    removePathList.Add(itemPath);
                    var oldEntry = archive.Body.GetEntry(itemName);
                    if(oldEntry != null) {
                        // 既に存在していれば削除しておく
                        oldEntry.Delete();
                    }
                    var buffer = FileUtility.ToBinary(itemPath);
                    var entry = archive.Body.CreateEntry(itemName, CompressionLevel.NoCompression);
                    using(var stream = new BinaryWriter(entry.Open())) {
                        stream.Write(buffer);
                    }
                }
                archive.Flush();
                foreach(var path in removePathList) {
                    try {
                        File.Delete(path);
                    } catch(Exception ex) {
                        appNonProcess.Logger.Warning(ex);
                    }
                }
            }
        }

        /// <summary>
        /// ボディファイルを読み込む。
        /// </summary>
        /// <typeparam name="TIndexBody"></typeparam>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <param name="archive"></param>
        /// <param name="appNonProcess"></param>
        /// <returns></returns>
        public static TIndexBody LoadBody<TIndexBody>(IndexKind indexKind, Guid guid, IndexBodyArchive archive, IAppNonProcess appNonProcess)
            where TIndexBody : IndexBodyItemModelBase, new()
        {
            var fileType = IndexItemUtility.GetBodyFileType(indexKind);
            var path = Environment.ExpandEnvironmentVariables(IndexItemUtility.GetBodyFilePath(indexKind, guid, appNonProcess.VariableConstants));
            if(File.Exists(path)) {
                // 実ファイルが存在すれば実ファイルを優先する
                var result = AppUtility.LoadSetting<TIndexBody>(path, fileType, appNonProcess.Logger);
                return result;
            } else {
                // アーカイブから取得するがアーカイブにもなければ初期値を返す
                CheckUtility.DebugEnforceNotNull(archive);
                if(archive.Body == null) {
                    // アーカイブはまだ作成されていない
                    return new TIndexBody();
                }
                var entoryName = IndexItemUtility.GetBodyFileName(indexKind, fileType, guid);
                var entry = archive.Body.GetEntry(entoryName);
                if(entry == null) {
                    // アーカイブに存在しない
                    return new TIndexBody();
                }
                using(var stream = entry.Open()) {
                    var result = AppUtility.LoadSetting<TIndexBody>(stream, fileType, appNonProcess.Logger);
                    return result;
                }
            }
        }

        /// <summary>
        /// ボディファイルを保存。
        /// </summary>
        /// <typeparam name="TIndexBody"></typeparam>
        /// <param name="indexBody"></param>
        /// <param name="guid"></param>
        /// <param name="appNonProcess"></param>
        public static void SaveBody<TIndexBody>(TIndexBody indexBody, Guid guid, IAppNonProcess appNonProcess)
            where TIndexBody : IndexBodyItemModelBase
        {
            var fileType = IndexItemUtility.GetBodyFileType(indexBody.IndexKind);
            var path = Environment.ExpandEnvironmentVariables(IndexItemUtility.GetBodyFilePath(indexBody.IndexKind, guid, appNonProcess.VariableConstants));
            var bodyItem = (TIndexBody)indexBody;
            AppUtility.SaveSetting(path, bodyItem, fileType, true, appNonProcess.Logger);
        }


    }
}
