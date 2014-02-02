/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 12/16/2013
 * 時刻: 22:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_variable.
	/// </summary>
	public partial class SettingForm
	{
		HashSet<LauncherItem> _launcherItems = null;
		FontSetting _commandFont = null;
		FontSetting _toolbarFont = null;
		LauncherItem _launcherSelectedItem = null;
		
		TabPage _nowSelectedTabPage = null;
		ImageList _imageToolbarItemGroup = null;
		bool _launcherItemEvent = false;
		
		/*
		Point _toolbarLocation;
		Size _toolbarSize;
		*/
		Dictionary<string,Rectangle> _toolbarFloat = new Dictionary<string, Rectangle>();
		
	}
}
