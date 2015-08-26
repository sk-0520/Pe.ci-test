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
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;

	public static class IndexItemUtility
	{
		public static string GetIndexBodyFileName(IndexKind indexKind, FileType fileType, Guid guid)
		{
			var ext = new Dictionary<FileType, string>() {
				{ FileType.Json,   Constants.extensionJsonFile },
				{ FileType.Binary, Constants.extensionBinaryFile },
			};

			var map = new Dictionary<string, string>() {
				{ Constants.keyGuidName, guid.ToString() },
				{ Constants.keyIndexExt, ext[fileType]},
			};
			return Constants.indexBodyBaseFileName.ReplaceFromDictionary(map);
		}

		public static FileType GetIndexBodyFileType(IndexKind indexKind)
		{
			switch (indexKind) {
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

		public static string GetIndexBodyParentDirectory(IndexKind indexKind, VariableConstants variableConstants)
		{
			switch (indexKind) {
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
		public static string GetIndexBodyFilePath(IndexKind indexKind, Guid guid, VariableConstants variableConstants)
		{
			var dirPath = IndexItemUtility.GetIndexBodyParentDirectory(indexKind, variableConstants);
			var fileType = IndexItemUtility.GetIndexBodyFileType(indexKind);
			var fileName = IndexItemUtility.GetIndexBodyFileName(indexKind, fileType, guid);
			var path = Environment.ExpandEnvironmentVariables(Path.Combine(dirPath, fileName));

			return path;
		}
	}
}
