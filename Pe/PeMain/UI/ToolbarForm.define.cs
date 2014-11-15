/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/05
 * 時刻: 2:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_define.
	/// </summary>
	partial class ToolbarForm
	{
		const string menuNameMainPosDesktopFloat = "desktop_float";
		const string menuNameMainPosDesktopTop = "desktop_top";
		const string menuNameMainPosDesktopBottom = "desktop_bottom";
		const string menuNameMainPosDesktopLeft = "desktop_left";
		const string menuNameMainPosDesktopRight = "desktop_right";
		const string menuNameMainTopmost = "topmost";
		const string menuNameMainAutoHide = "autohide";
			
		const string menuNameExecute = "exec";
		const string menuNameExecuteEx = "ex";
		const string menuNamePath = "path";
		const string menuNameFiles = "ls";
		
		const string menuNamePath_openParentDir = "open_parent_dir";
		const string menuNamePath_openWorkDir = "open_work_dir";
		const string menuNamePath_copyCommand = "copy_command";
		const string menuNamePath_copyParentDir = "copy_parrent_dir";
		const string menuNamePath_copyWorkDir = "copy_work_dir";
		const string menuNamePath_property = "property";
		
		enum DropType
		{
			None,
			Files,
			Button
		}
		
		struct DropData
		{
			public DropType DropType { get; set; }
			public ToolStripItem ToolStripItem { get; set; }
			public LauncherItem LauncherItem  { get; set; }
			public IEnumerable<string> Files { get; set; }
			public ToolStripItem SrcToolStripItem { get; set; }
		}
		
	}
}

