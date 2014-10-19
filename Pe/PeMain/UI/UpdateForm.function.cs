/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

using PeMain.Data;

namespace PeMain.UI
{
	partial class UpdateForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplyLanguage();
			ApplySetting();
			
			ApplyUpdate();
		}
		
		void ApplySetting()
		{
			if(UpdateData.Info.IsRcVersion) {
				this.labelVersion.BorderStyle = BorderStyle.FixedSingle;
				this.labelVersion.ForeColor = Color.Red;
				this.labelVersion.BackColor = Color.Black;
			}
		}
		
		void ApplyUpdate()
		{
			byte[] httpData = null;
			using(var web = new WebClient()) {
				var url = UpdateData.Info.IsRcVersion ? Literal.ChangeLogRcURL: Literal.ChangeLogURL; 
				httpData = web.DownloadData(url);
			}
			this.webUpdate.DocumentStream = new MemoryStream(httpData);
		}
	}
}
