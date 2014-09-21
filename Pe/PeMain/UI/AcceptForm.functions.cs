/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/21/2014
 * 時刻: 10:09
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of AcceptForm_functions.
	/// </summary>
	partial class AcceptForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplyLanguage();
		}
		
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			var acceptFilePath = Path.Combine(Literal.PeLanguageDirPath, CommonData.Language.AcceptFileName);
			//this.webDocument.DocumentText = File.ReadAllText(acceptFilePath);;
			this.webDocument.Navigate(acceptFilePath);
		}
	}
}
