/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/21
 * 時刻: 20:08
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of AcceptForm_language.
	/// </summary>
	partial class AcceptForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			this.AcceptButton = null;
			
			this.selectUpdateCheck.SetLanguage(CommonData.Language);
			this.selectUpdateCheckRC.SetLanguage(CommonData.Language);
			
			var acceptFilePath = Path.Combine(Literal.ApplicationLanguageDirPath, CommonData.Language.AcceptFileName);
			var acceptFileSource = File.ReadAllText(acceptFilePath);
			var acceptMap = new Dictionary<string,string>() {
				{"WEB", Literal.AboutWebURL },
				{"DEVELOPMENT", Literal.AboutDevelopmentURL },
				{"MAIL", Literal.AboutMailAddress },
				{"DISCUSSION", Literal.DiscussionURL },
				{"HELP", Literal.HelpDocumentURI },
			};
			var acceptFileReplaced = acceptFileSource.ReplaceRangeFromDictionary("${", "}", acceptMap);
			this.webDocument.DocumentStream = new MemoryStream(Encoding.Unicode.GetBytes(acceptFileReplaced));
		}
	}
}
