/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 22:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;

using ContentTypeTextNet.Pe.Application.Data;

namespace ContentTypeTextNet.Pe.Application.UI
{
	partial class EnvRemoveControl
	{
		public void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		
		public void SetItem(IList<string> items)
		{
			this._event = false;
			try {
				var lines = string.Join(Environment.NewLine, items.Where(s => !string.IsNullOrEmpty(s)));
				this.inputEnv.Text = lines;
			} finally {
				this._event = true;
			}
		}
		
		public void Clear()
		{
			this.inputEnv.Clear();
		}
	}
}

