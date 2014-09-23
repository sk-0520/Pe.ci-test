/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PeMain.Logic;

namespace PeMain.UI
{
	public partial class StreamForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.Language != null);
			
			var map = new Dictionary<string, string>() {
				{ SystemLanguageName.itemName, LauncherItem.Name },
			};
			
			UIUtility.SetDefaultText(this, CommonData.Language, map);
			
			this.tabStream_pageStream.SetLanguage(CommonData.Language);
			this.tabStream_pageProcess.SetLanguage(CommonData.Language);
			this.tabStream_pageProperty.SetLanguage(CommonData.Language);
			
			this.toolStream_itemSave.SetLanguage(CommonData.Language);
			this.toolStream_itemClear.SetLanguage(CommonData.Language);
			this.toolStream_itemRefresh.SetLanguage(CommonData.Language);
			this.toolStream_itemTopmost.SetLanguage(CommonData.Language);
			this.toolStream_itemKill.SetLanguage(CommonData.Language);
		}
	}
}
