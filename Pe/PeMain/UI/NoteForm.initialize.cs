/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Data;

namespace PeMain.UI
{
	partial class NoteForm
	{
		void Initialize()
		{
			this._commandStateMap = new Dictionary<NoteCommand, ButtonState>() {
				{ NoteCommand.Close, ButtonState.Normal },
				{ NoteCommand.Compact, ButtonState.Normal },
				{ NoteCommand.Topmost, ButtonState.Normal },
			};
		}
	}
}
