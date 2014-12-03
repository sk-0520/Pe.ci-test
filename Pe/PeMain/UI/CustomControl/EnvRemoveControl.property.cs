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

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class EnvRemoveControl
	{
		public IEnumerable<string> Items
		{
			get
			{
				return this.inputEnv.Lines.Where(s => !string.IsNullOrWhiteSpace(s));
			}
		}
	}
}
