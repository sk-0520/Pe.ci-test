/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 21:42
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

using PI.Windows;

namespace PeMain.Data
{
	/// <summary>
	/// Description of HotkeySetting.
	/// </summary>
	[Serializable]
	public class HotKeySetting
	{
		public HotkeySetting()
		{
			Key = Keys.None;
			Modifiers = MOD.None;
		}
		
		public Keys Key { get; set; }
		public MOD  Modifiers { get; set; }
	}
}
