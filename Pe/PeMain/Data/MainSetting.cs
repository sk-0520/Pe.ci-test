/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 15:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Linq;
using System.Windows.Forms;
using PeUtility;

namespace PeMain.Data
{
	/// <summary>
	/// 実行情報。
	/// </summary>
	[Serializable]
	public class RunningInfo: Item
	{
		/// <summary>
		/// 自動アップデートチェック。
		/// </summary>
		public bool CheckUpdate { get; set; }
		/// <summary>
		/// RC版もアップデーチェック対象とする。
		/// </summary>
		public bool CheckUpdateRC { get; set; }
		/// <summary>
		/// Peの実行許可。
		/// </summary>
		public bool Running { get; set; }
		
		public ushort VersionMajor { get; set; }
		public ushort VersionMinor { get; set; }
		public ushort VersionRevision { get; set; }
		public ushort VersionBuild { get; set; }
		
		public void SetDefaultVersion()
		{
			var version = Application.ProductVersion.Split('.').Map(s => ushort.Parse(s)).ToArray();
			VersionMajor = version[0];
			VersionMinor = version[1];
			VersionRevision = version[2];
			VersionBuild = version[3];
		}
	}
	
	/// <summary>
	/// 設定統括
	/// </summary>
	[Serializable]
	public class MainSetting: DisposableItem, IDisposable
	{
		public MainSetting()
		{
			RunningInfo = new RunningInfo();
			
			Log = new LogSetting();
			SystemEnv = new SystemEnvSetting();
			Launcher = new LauncherSetting();
			Command = new CommandSetting();
			Toolbar = new ToolbarSetting();
			Note = new NoteSetting();
		}
		
		public RunningInfo RunningInfo { get; set; }
		
		/// <summary>
		/// 使用言語。
		/// </summary>
		public string LanguageName { get; set; }
		/// <summary>
		/// ランチャアイテム統括。
		/// </summary>
		public LauncherSetting Launcher { get; set; }
		/// <summary>
		/// ログ設定
		/// </summary>
		public LogSetting Log { get; set; }
		/// <summary>
		/// システム環境セッティング
		/// </summary>
		public SystemEnvSetting SystemEnv { get; set; }
		/// <summary>
		/// コマンドランチャ設定。
		/// </summary>
		public CommandSetting Command { get; set; }
		/// <summary>
		/// ツールバー
		/// </summary>
		public ToolbarSetting Toolbar { get; set; }
		/// <summary>
		/// ノード
		/// </summary>
		public NoteSetting Note { get; set; }
		
		public override void Dispose()
		{
			Command.ToDispose();
		}
	}
}
