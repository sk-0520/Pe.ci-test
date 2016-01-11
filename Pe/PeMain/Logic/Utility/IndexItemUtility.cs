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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;

    public static class IndexItemUtility
    {
        public static string GetBodyFileName(IndexKind indexKind, FileType fileType, Guid guid)
        {
            var ext = new Dictionary<FileType, string>() {
                { FileType.Json,   Constants.ExtensionJsonFile },
                { FileType.Binary, Constants.ExtensionBinaryFile },
            };

            var map = new Dictionary<string, string>() {
                { Constants.keyGuidName, guid.ToString() },
                { Constants.keyIndexExt, ext[fileType]},
            };
            return Constants.indexBodyBaseFileName.ReplaceFromDictionary(map);
        }

        public static FileType GetBodyFileType(IndexKind indexKind)
        {
            switch(indexKind) {
                case IndexKind.Note:
                    return Constants.fileTypeNoteBody;

                case IndexKind.Template:
                    return Constants.fileTypeTemplateBody;

                case IndexKind.Clipboard:
                    return Constants.fileTypeClipboardBody;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetBodyParentDirectory(IndexKind indexKind, VariableConstants variableConstants)
        {
            switch(indexKind) {
                case IndexKind.Note:
                    return variableConstants.UserSettingNoteDirectoryPath;

                case IndexKind.Template:
                    return variableConstants.UserSettingTemplateDirectoryPath;

                case IndexKind.Clipboard:
                    return variableConstants.UserSettingClipboardDirectoryPath;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// インデックスのボディファイルパスを取得。
        /// </summary>
        /// <param name="indexKind"></param>
        /// <param name="guid"></param>
        /// <param name="variableConstants"></param>
        /// <returns>環境変数展開済みファイルパス。</returns>
        public static string GetBodyFilePath(IndexKind indexKind, Guid guid, VariableConstants variableConstants)
        {
            var dirPath = IndexItemUtility.GetBodyParentDirectory(indexKind, variableConstants);
            var fileType = IndexItemUtility.GetBodyFileType(indexKind);
            var fileName = IndexItemUtility.GetBodyFileName(indexKind, fileType, guid);
            var path = Environment.ExpandEnvironmentVariables(Path.Combine(dirPath, fileName));

            return path;
        }

        public static bool RemoveBody(IndexKind indexKind, Guid guid, IAppNonProcess appNonProcess)
        {
            var path = IndexItemUtility.GetBodyFilePath(indexKind, guid, appNonProcess.VariableConstants);
            try {
                File.Delete(path);
                return true;
            } catch(Exception ex) {
                appNonProcess.Logger.Error(ex);
                return false;
            }
        }

        public static void GarbageCollectionBody<TItemModel>(IndexKind indexKind, IndexItemCollectionModel<TItemModel> items, IAppNonProcess appNonProcess)
            where TItemModel : IndexItemModelBase
        {
            var parentDirPath = Environment.ExpandEnvironmentVariables(GetBodyParentDirectory(indexKind, appNonProcess.VariableConstants));
            if(!Directory.Exists(parentDirPath)) {
                return;
            }
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
                RemoveBody(indexKind, removeFileGuid, appNonProcess);
            }

            // 一時データ削除
            AppUtility.GarbageCollectionTemporaryFile(parentDirPath, appNonProcess.Logger);
        }

        public static TIndexBody LoadBody<TIndexBody>(IndexKind indexKind, Guid guid, IAppNonProcess appNonProcess)
            where TIndexBody : IndexBodyItemModelBase, new()
        {
            var fileType = IndexItemUtility.GetBodyFileType(indexKind);
            var path = IndexItemUtility.GetBodyFilePath(indexKind, guid, appNonProcess.VariableConstants);
            var result = AppUtility.LoadSetting<TIndexBody>(path, fileType, appNonProcess.Logger);

            return result;
        }

        public static void SaveBody<TIndexBody>(TIndexBody indexBody, Guid guid, IAppNonProcess appNonProcess)
            where TIndexBody : IndexBodyItemModelBase
        {
            var fileType = IndexItemUtility.GetBodyFileType(indexBody.IndexKind);
            var path = IndexItemUtility.GetBodyFilePath(indexBody.IndexKind, guid, appNonProcess.VariableConstants);
            var bodyItem = (TIndexBody)indexBody;
            AppUtility.SaveSetting(path, bodyItem, fileType, true, appNonProcess.Logger);
        }


    }
}
