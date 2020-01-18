/*
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class IndexItemUtility
    {
        const CompressionLevel defaultCompressionLevel = CompressionLevel.Fastest;

        /// <summary>
        /// インデックスデータ種別からインデックスのボディファイルのファイル種別を取得する。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <returns></returns>
        public static SerializeFileType GetBodyFileType(IndexKind indexKind)
        {
            var map = new Dictionary<IndexKind, SerializeFileType>() {
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
        public static string GetBodyFileName(IndexKind indexKind, SerializeFileType fileType, Guid guid)
        {
            var ext = new Dictionary<SerializeFileType, string>() {
                { SerializeFileType.Json,   Constants.ExtensionJsonFile },
                { SerializeFileType.Binary, Constants.ExtensionBinaryFile },
            };

            var map = new Dictionary<string, string>() {
                { Constants.keyGuidName, guid.ToString(Constants.FormatGuidFileName) },
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
        /// <param name="parentDirectoryPath"></param>
        /// <returns></returns>
        static string GetBodyFilePath(IndexKind indexKind, SerializeFileType fileType, Guid guid, string parentDirectoryPath)
        {
            var fileName = GetBodyFileName(indexKind, fileType, guid);
            var path = Path.Combine(parentDirectoryPath, fileName);

            return path;
        }
        /// <summary>
        /// インデックスのボディファイルパスを取得。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="fileType"></param>
        /// <param name="guid"></param>
        /// <param name="variableConstants"></param>
        /// <returns></returns>
        public static string GetBodyFilePath(IndexKind indexKind, SerializeFileType fileType, Guid guid, VariableConstants variableConstants)
        {
            var dirPath = IndexItemUtility.GetBodyFileParentDirectory(indexKind, variableConstants);
            var path = GetBodyFilePath(indexKind, fileType, guid, dirPath);

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
            var path = Path.Combine(dir, Constants.BodyArchiveFileName);
            return path;
        }

        static bool CompareArchiveEntryName(ZipArchiveEntry entry, string name)
        {
            return string.Compare(entry.FullName, name, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// ボディファイルは物理ファイルか。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <param name="parentDirectoryPath"></param>
        /// <returns></returns>
        static bool ExistisRealBodyFile(IndexKind indexKind, Guid guid, string parentDirectoryPath)
        {
            if(!Directory.Exists(parentDirectoryPath)) {
                return false;
            }

            var fileType = GetBodyFileType(indexKind);
            var path = Environment.ExpandEnvironmentVariables(GetBodyFilePath(indexKind, fileType, guid, parentDirectoryPath));

            return File.Exists(path);
        }

        static bool ExistisArchiveBodyFile(IndexKind indexKind, Guid guid, IndexBodyArchive archive, string parentDirectoryPath)
        {
            if(ExistisRealBodyFile(indexKind, guid, parentDirectoryPath)) {
                return false;
            }

            if(!archive.EnabledArchive) {
                return false;
            }

            //if(!Directory.Exists(parentDirectoryPath)) {
            //    return false;
            //}

            var entryName = GetBodyFileName(indexKind, guid);
            return archive.Body.Entries.Any(z => CompareArchiveEntryName(z, entryName));
        }

        static bool RemoveRealBodyFile(IndexKind indexKind, Guid guid, string parentDirectoryPath, ILogger logger)
        {
            var path = Environment.ExpandEnvironmentVariables(GetBodyFilePath(indexKind, GetBodyFileType(indexKind), guid, parentDirectoryPath));
            try {
                File.Delete(path);
                return true;
            } catch(Exception ex) {
                logger.Error(ex);
                return false;
            }
        }

        static bool RemoveArchiveBodyFile(IndexKind indexKind, Guid guid, IndexBodyArchive archive, ILogger logger)
        {
            if(!archive.EnabledArchive) {
                return false;
            }

            var entryName = GetBodyFileName(indexKind, guid);
            // 同じパスで複数ファイルが格納されている可能性あり
            var targetEntryList = archive.Body.Entries
                .Where(z => CompareArchiveEntryName(z, entryName))
                .ToArray()
            ;
            try {
                foreach(var entry in targetEntryList) {
                    entry.Delete();
                }
                return true;
            } catch(Exception ex) {
                logger.Error(ex);
                return false;
            }
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
            var parentDir = Environment.ExpandEnvironmentVariables(GetBodyFileParentDirectory(indexKind, appNonProcess.VariableConstants));
            if(ExistisRealBodyFile(indexKind, guid, parentDir)) {
                return RemoveRealBodyFile(indexKind, guid, parentDir, appNonProcess.Logger);
            } else if(ExistisArchiveBodyFile(indexKind, guid, archive, parentDir)) {
                var result = RemoveArchiveBodyFile(indexKind, guid, archive, appNonProcess.Logger);
                if(result) {
                    archive.Flush();
                }
                return result;
            } else {
                return false;
            }
        }

        static IList<Guid> GetUnindexedGuid<TItemModel>(IndexKind indexKind, IndexItemCollectionModel<TItemModel> items, IEnumerable<Guid> guids)
            where TItemModel : IndexItemModelBase
        {
            var itemList = items.ToList();
            var removeTargetList = new List<Guid>();
            foreach(var guid in guids) {
                var targetIndex = itemList.FindIndex(i => i.Id == guid);
                if(targetIndex == -1) {
                    removeTargetList.Add(guid);
                } else {
                    itemList.RemoveAt(targetIndex);
                }
            }

            return removeTargetList;
        }

        static bool CheckGuidParse(string s)
        {
            Guid temp;
            return Guid.TryParse(s, out temp);
        }

        ///// <summary>
        ///// インデックスデータをGC。
        ///// </summary>
        ///// <typeparam name="TItemModel"></typeparam>
        ///// <param name="indexKind"></param>
        ///// <param name="items"></param>
        ///// <param name="archive"></param>
        ///// <param name="appNonProcess"></param>
        //public static void GarbageCollectionBody<TItemModel>(IndexKind indexKind, IndexItemCollectionModel<TItemModel> items, IndexBodyArchive archive, DateTime archiveBaseTimestamp, TimeSpan archiveTimespan, long archiveFileSize, IAppNonProcess appNonProcess)
        //    where TItemModel : IndexItemModelBase
        //{
        //    var parentDirPath = Environment.ExpandEnvironmentVariables(GetBodyFileParentDirectory(indexKind, appNonProcess.VariableConstants));
        //    if(!Directory.Exists(parentDirPath)) {
        //        return;
        //    }
        //    // 実ファイルは存在するがインデックスに存在しないものを破棄
        //    var searchPattern = "*" + Path.GetExtension(GetBodyFileName(indexKind, GetBodyFileType(indexKind), Guid.Empty));
        //    var fileNames = Directory
        //        .EnumerateFiles(parentDirPath, searchPattern, SearchOption.TopDirectoryOnly)
        //        .Select(s => Path.GetFileNameWithoutExtension(s))
        //        .Where(s => CheckGuidParse(s))
        //        .Select(s => new Guid(s))
        //    ;
        //    var removeTargetList = GetUnindexedGuid(indexKind, items, fileNames);
        //    foreach(var removeFileGuid in removeTargetList) {
        //        RemoveRealBodyFile(indexKind, removeFileGuid, parentDirPath, appNonProcess.Logger);
        //    }

        //    // 一時データ削除
        //    AppUtility.GarbageCollectionTemporaryFile(parentDirPath, appNonProcess.Logger);

        //    // データアーカイブ
        //    GarbageCollectionBodyArchive(indexKind, items, archive, archiveBaseTimestamp, archiveTimespan, archiveFileSize, appNonProcess);
        //}

        /// <summary>
        /// アーカイブのGC。
        /// </summary>
        /// <typeparam name="TItemModel"></typeparam>
        /// <param name="indexKind"></param>
        /// <param name="items"></param>
        /// <param name="archive"></param>
        /// <param name="archiveTimeSpan"></param>
        /// <param name="fileSize"></param>
        /// <param name="appNonProcess"></param>
        static void GarbageCollectionBodyArchive<TItemModel>(IndexKind indexKind, IndexItemCollectionModel<TItemModel> items, IndexBodyArchive archive, DateTime archiveBaseTime, TimeSpan archiveTimeSpan, long fileSize, IAppNonProcess appNonProcess)
            where TItemModel : IndexItemModelBase
        {
            if(archive.EnabledArchive) {
                // アーカイブには存在するがインデックスに存在しないものを破棄
                var guids = archive.Body.Entries
                    .Select(e => Path.GetFileNameWithoutExtension(e.FullName))
                    .Where(s => CheckGuidParse(s))
                    .Select(s => new Guid(s))
                ;

                var removeTargetList = GetUnindexedGuid(indexKind, items, guids);
                int removedFileCount = 0;
                foreach(var guid in removeTargetList) {
                    var removed = RemoveArchiveBodyFile(indexKind, guid, archive, appNonProcess.Logger);
                    if(removed) {
                        removedFileCount += 1;
                    }
                }
                appNonProcess.Logger.Debug($"{indexKind} remove[index]: {removedFileCount}/{removeTargetList.Count}");
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
                var archiveItemCount = 0;
                foreach(var item in oldItems) {
                    var itemName = GetBodyFileName(indexKind, GetBodyFileType(indexKind), item.Id);

                    var itemPath = Environment.ExpandEnvironmentVariables(GetBodyFilePath(indexKind, item.Id, appNonProcess.VariableConstants));
                    if(!File.Exists(itemPath)) {
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
                    var entry = archive.Body.CreateEntry(itemName, defaultCompressionLevel);
                    entry.LastWriteTime = item.History.UpdateTimestamp;
                    using(var stream = new BinaryWriter(entry.Open())) {
                        stream.Write(buffer);
                    }
                    archiveItemCount += 1;
                }
                archive.Flush();
                appNonProcess.Logger.Debug($"{indexKind} archive: {archiveItemCount}/{oldItems.Length}");
                int removedFileCount = 0;
                foreach(var path in removePathList) {
                    try {
                        File.Delete(path);
                        removedFileCount += 1;
                    } catch(Exception ex) {
                        appNonProcess.Logger.Warning(ex);
                    }
                }
                appNonProcess.Logger.Debug($"{indexKind} remove[archive]: {removedFileCount}/{removePathList.Count}");
            }
        }

        static TIndexBody LoadRealBodyFile<TIndexBody>(IndexKind indexKind, Guid guid, string parentDirectoryPath, ILogger logger)
            where TIndexBody : IndexBodyItemModelBase, new()
        {
            var fileType = GetBodyFileType(indexKind);
            var path = Environment.ExpandEnvironmentVariables(GetBodyFilePath(indexKind, fileType, guid, parentDirectoryPath));

            var result = SerializeUtility.LoadSetting<TIndexBody>(path, fileType, logger);
            return result;
        }

        static TIndexBody LoadArchiveBodyFile<TIndexBody>(IndexKind indexKind, Guid guid, IndexBodyArchive archive, ILogger logger)
            where TIndexBody : IndexBodyItemModelBase, new()
        {
            if(!archive.EnabledArchive) {
                // アーカイブはまだ作成されていない
                return new TIndexBody();
            }
            var fileType = GetBodyFileType(indexKind);
            var entoryName = GetBodyFileName(indexKind, fileType, guid);
            var entry = archive.Body.GetEntry(entoryName);
            if(entry == null) {
                // アーカイブに存在しない
                return new TIndexBody();
            }
            using(var stream = entry.Open()) {
                var result = SerializeUtility.LoadSetting<TIndexBody>(stream, fileType, logger);
                return result;
            }
        }

        /// <summary>
        /// ボディファイルを読み込む。
        /// <para>実ファイルをアーカイブより優先する。</para>
        /// </summary>
        /// <typeparam name="TIndexBody"></typeparam>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <param name="archive"></param>
        /// <param name="appNonProcess"></param>
        /// <returns></returns>
        internal static TIndexBody LoadBody<TIndexBody>(IndexKind indexKind, Guid guid, IndexBodyArchive archive, IAppNonProcess appNonProcess)
            where TIndexBody : IndexBodyItemModelBase, new()
        {
            var parentDir = Environment.ExpandEnvironmentVariables(GetBodyFileParentDirectory(indexKind, appNonProcess.VariableConstants));
            TIndexBody result;
            //var isCreate = false;
            if(ExistisRealBodyFile(indexKind, guid, parentDir)) {
                result = LoadRealBodyFile<TIndexBody>(indexKind, guid, parentDir, appNonProcess.Logger);
            } else if(ExistisArchiveBodyFile(indexKind, guid, archive, parentDir)) {
                result = LoadArchiveBodyFile<TIndexBody>(indexKind, guid, archive, appNonProcess.Logger);
            } else {
                // なんもない
                result = new TIndexBody();
                //isCreate = true;
            }

            //switch(indexKind) {
            //    case IndexKind.Clipboard:
            //        SettingUtility.InitializeClipboardBodyItem(result as ClipboardBodyItemModel, isCreate, appNonProcess);
            //        break;

            //    case IndexKind.Template:
            //        SettingUtility.InitializeTemplateBodyItem(result as TemplateBodyItemModel, isCreate, appNonProcess);
            //        break;

            //    case IndexKind.Note:
            //        SettingUtility.InitializeNoteBodyItem(result as NoteBodyItemModel, isCreate, appNonProcess);
            //        break;

            //    default:
            //        throw new NotSupportedException();
            //}

            return result;
        }

        static void SaveRealBodyFile<TIndexBody>(TIndexBody indexBody, Guid guid, string parentDirectoryPath, ILogger logger)
            where TIndexBody : IndexBodyItemModelBase
        {
            var fileType = IndexItemUtility.GetBodyFileType(indexBody.IndexKind);
            var path = Environment.ExpandEnvironmentVariables(IndexItemUtility.GetBodyFilePath(indexBody.IndexKind, fileType,  guid, parentDirectoryPath));
            var bodyItem = (TIndexBody)indexBody;
            SerializeUtility.SaveSetting(path, bodyItem, fileType, true, logger);
        }
        // BUGS: タイムスタンプのことなーんも考えてなかった
        static void SaveArchiveBodyFile<TIndexBody>(TIndexBody indexBody, Guid guid, IndexBodyArchive archive, ILogger logger)
            where TIndexBody : IndexBodyItemModelBase
        {
            var fileType = GetBodyFileType(indexBody.IndexKind);
            var entryName = GetBodyFileName(indexBody.IndexKind, fileType, guid);
            CheckUtility.Enforce(archive.EnabledArchive);
            var oldEntry = archive.Body.GetEntry(entryName);
            if(oldEntry != null) {
                oldEntry.Delete();
            }

            var entry = archive.Body.CreateEntry(entryName, defaultCompressionLevel);
            using(var stream = entry.Open()) {
                SerializeUtility.SaveSetting(stream, indexBody, fileType, logger);
            }
        }


        /// <summary>
        /// ボディファイルを保存。
        /// </summary>
        /// <typeparam name="TIndexBody"></typeparam>
        /// <param name="indexBody"></param>
        /// <param name="guid"></param>
        /// <param name="appNonProcess"></param>
        internal static void SaveBody<TIndexBody>(TIndexBody indexBody, Guid guid, IndexBodyArchive archive, IndexBodyKind indexBodyKind, IAppNonProcess appNonProcess)
            where TIndexBody : IndexBodyItemModelBase
        {
            indexBody.PreviousVersion = (Version)Constants.ApplicationVersionNumber.Clone();

            var parentDir = Environment.ExpandEnvironmentVariables(GetBodyFileParentDirectory(indexBody.IndexKind, appNonProcess.VariableConstants));
            switch(indexBodyKind) {
                case IndexBodyKind.Default:
                    if(ExistisRealBodyFile(indexBody.IndexKind, guid, parentDir)) {
                        goto IndexBodyKind_File;
                    } else if(ExistisArchiveBodyFile(indexBody.IndexKind, guid, archive, parentDir)) {
                        goto IndexBodyKind_Archive;
                    } else {
                        goto IndexBodyKind_File;
                    }
                    throw new NotImplementedException("到達不可");

                case IndexBodyKind.File: IndexBodyKind_File:
                    SaveRealBodyFile(indexBody, guid, parentDir, appNonProcess.Logger);
                    break;

                case IndexBodyKind.Archive: IndexBodyKind_Archive:
                    if(!archive.EnabledArchive) {
                        archive.OpenArchiveFile(indexBody.IndexKind, appNonProcess.VariableConstants);
                    }
                    SaveArchiveBodyFile(indexBody, guid, archive, appNonProcess.Logger);
                    archive.Flush();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
