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

using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace PeMain.Data
{
	/// <summary>
	/// ホットキー設定。
	/// </summary>
	[Serializable]
	public class HotKeySetting: Item
	{
		public HotKeySetting()
		{
			Key = Keys.None;
			Modifiers = MOD.None;
			
			IsRegistered = false;
		}
		
		/// <summary>
		/// キー
		/// </summary>
		public Keys Key { get; set; }
		/// <summary>
		/// 装飾キー。
		/// </summary>
		public MOD  Modifiers { get; set; }
		
		/// <summary>
		/// 登録済みか。
		/// </summary>
		[XmlIgnore()]
		public bool IsRegistered { get; set; }
		
		/// <summary>
		/// 有効なキー設定か。
		/// </summary>
		public bool Enabled
		{
			get
			{
				return Key != Keys.None && Modifiers != MOD.None;
			}
		}
		
		public Keys GetShorcutKey()
		{
			if(Enabled) {
				var mod = Keys.None;
				if((Modifiers & MOD.MOD_ALT) == MOD.MOD_ALT) {
					mod |= Keys.Alt;
				}
				if((Modifiers & MOD.MOD_CONTROL) == MOD.MOD_CONTROL) {
					mod |= Keys.Control;
				}
				if((Modifiers & MOD.MOD_SHIFT) == MOD.MOD_SHIFT) {
					mod |= Keys.Shift;
				}
				if((Modifiers & MOD.MOD_WIN) == MOD.MOD_WIN) {
					mod |= Keys.LWin | Keys.RWin;
				}
				
				return Key | mod;
			} else {
				return Keys.None;
			}
		}
	}
}
