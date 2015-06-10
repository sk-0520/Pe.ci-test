namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class AppLanguageManager: LanguageManager
	{
		public AppLanguageManager(LanguageCollectionModel model, string languageFilePath)
			:base(model, languageFilePath)
		{
			BaseFileName = Path.GetFileNameWithoutExtension(LanguageFilePath);
			BaseDirectoryPath = Path.GetDirectoryName(LanguageFilePath);
		}

		#region property

		public string BaseFileName { get; private set; }
		public string BaseDirectoryPath { get; private set; }
		public string AcceptDocumentFilePath { get { return Path.Combine(BaseDirectoryPath, BaseFileName + Constants.languageAcceptDocumentFileName); } }

		#endregion 

		#region LanguageManager

		protected override IDictionary<string, string> GetSystemMap(DateTime dateTime)
		{
			var baseMap = base.GetSystemMap(dateTime);
			baseMap.Add("APPLICATION", Constants.programName);
			baseMap.Add("APPLICATION:VERSION", Constants.applicationVersion.ToString());
			baseMap.Add("APPLICATION:REVISION", Constants.applicationRevision);
			return baseMap;
		}

		#endregion
	}
}
