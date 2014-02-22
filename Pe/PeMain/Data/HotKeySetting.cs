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
using System.Xml.Serialization;
using PI.Windows;

namespace PeMain.Data
{
	/// <summary>
	/// Description of HotkeySetting.
	/// </summary>
	[Serializable]
	public class HotKeySetting
	{
		public HotKeySetting()
		{
			Key = Keys.None;
			Modifiers = MOD.None;
			
			Resisted = false;
		}
		
		public Keys Key { get; set; }
		public MOD  Modifiers { get; set; }
		
		[XmlIgnore()]
		public bool Resisted { get; set; }
		
		public bool Enabled
		{
			get
			{
				return Key != Keys.None && Modifiers != MOD.None;
			}
		}
	}
}
