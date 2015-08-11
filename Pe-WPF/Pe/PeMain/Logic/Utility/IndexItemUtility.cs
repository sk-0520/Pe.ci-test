namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
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
	}
}
