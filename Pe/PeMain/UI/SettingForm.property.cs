/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 12/16/2013
 * 時刻: 22:57
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_property.
	/// </summary>
	public partial class SettingForm
	{
		/// <summary>
		/// 使用言語データ
		/// </summary>
		public Language Language { get; private set; }
		
		public MainSetting MainSetting { get; private set; }
	}
}
