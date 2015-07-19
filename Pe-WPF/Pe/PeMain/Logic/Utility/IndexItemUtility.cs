namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	public static class IndexItemUtility
	{
		public static string GetIndexBodyFileName(IndexKind indexKind, Guid guid)
		{
			var map = new Dictionary<string, string>() {
				{ Constants.keyGuidName, guid.ToString() },
			};
			return Constants.indexBodyBaseFileName.ReplaceFromDictionary(map);
		}
	}
}
