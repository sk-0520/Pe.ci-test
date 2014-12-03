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
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.Skin;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class NoteForm
	{
		Dictionary<SkinNoteCommand, SkinButtonState> _commandStateMap;
		NoteBindItem _bindItem;
		bool _initialized = true;
		bool _changed = false;
		string _prevTitle;
		string _prevBody;
		//Color _prevForeColor;
		//Color _prevBackColor;
	}
}
