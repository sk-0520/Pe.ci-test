/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/21/2014
 * 時刻: 10:09
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ContentTypeTextNet.Pe.Application.Logic;

namespace ContentTypeTextNet.Pe.Application.UI
{
	/// <summary>
	/// Description of AcceptForm_initialize.
	/// </summary>
	partial class AcceptForm
	{
		void Initialize()
		{
			WebBrowserUtility.AttachmentNewWindow(this.webDocument);
		}
	}
}
