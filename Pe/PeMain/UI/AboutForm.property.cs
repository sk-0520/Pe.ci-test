/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:55
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of AboutForm_property.
	/// </summary>
	partial class AboutForm
	{
		CommonData CommonData { get; set; }
		public bool CheckUpdate { get; private set; }

		List<ComponentInfo> ComponentInfoList { get; set; }

		string Separator { get { return "____________"; } }
	}
}
