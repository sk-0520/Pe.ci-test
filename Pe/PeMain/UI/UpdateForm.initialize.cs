/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Net;
using PeUtility;

namespace PeMain.UI
{
	partial class UpdateForm
	{
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);
			
			byte[] httpData = null;
			using(var web = new WebClient()) {
				httpData = web.DownloadData(Literal.ChangeLogURL);
			}
			this.webUpdate.DocumentStream = new MemoryStream(httpData);
		}
	}
}
