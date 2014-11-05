/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeUtility;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class UpdateForm
	{
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);
			WebBrowserUtility.AttachmentNewWindow(this.webUpdate);
		}
	}
}
