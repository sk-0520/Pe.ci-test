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

using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_variable.
	/// </summary>
	public partial class Pe
	{
		private NotifyIcon _notifyIcon;
		private ContextMenu _notificationMenu;
		private MessageWindow _messageWindow;
		private LogForm _logForm;
		
		private CommonData _commonData;
		
		private Dictionary<Screen, ToolbarForm> _toolbarForms = new Dictionary<Screen, ToolbarForm>();
	}
}
