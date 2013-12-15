/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 15:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain.Setting
{
	/// <summary>
	/// 設定統括
	/// </summary>
	[Serializable]
	public class MainSetting: Item, IDisposable
	{
		public MainSetting()
		{
			LauncherSet = new LauncherSet();
			Command = new CommandSetting();
		}
		
		/// <summary>
		/// 使用言語。
		/// </summary>
		public string Language { get; set; }
		/// <summary>
		/// ランチャアイテム統括。
		/// </summary>
		public LauncherSet LauncherSet { get; set; }
		/// <summary>
		/// コマンドランチャ設定。
		/// </summary>
		public CommandSetting Command { get; set; }
		
		public void Dispose()
		{
			if(Command != null) {
				Command.Dispose();
			}
		}
	}
}
