/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.UI;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of Pe_variable.
	/// </summary>
	partial class App
	{
		private NotifyIcon _notifyIcon;
		//private ContextMenu _contextMenu;
		private AppContextMenuStrip _contextMenu;
		private MessageWindow _messageWindow;
		private LogForm _logForm;
		private ClipboardForm _clipboardForm;
		
		private List<NoteForm> _noteWindowList = new List<NoteForm>();
		
		private CommonData _commonData;
		private bool _pause;
		
		private Dictionary<Screen, ToolbarForm> _toolbarForms = new Dictionary<Screen, ToolbarForm>();
		
		private WindowListItem _tempWindowListItem;
		List<WindowListItem> _windowListItems = new List<WindowListItem>();
		//private List<WindowListItem> _windowListItemList = new List<WindowListItem>();
		
		System.Timers.Timer _windowTimer;

		Listener _listener;

		HashSet<Form> _otherWindows = new HashSet<Form>();
	}
}
