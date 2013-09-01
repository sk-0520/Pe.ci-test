/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 0:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using ShareLib;

namespace Pe.Logic
{
	/// <summary>
	/// ランチャー種別
	/// </summary>
	public enum LauncherType
	{
		/// <summary>
		/// 実行ファイル
		/// </summary>
		Application,
		/// <summary>
		/// システムでのコマンド
		/// </summary>
		SystemCommand,
		/// <summary>
		/// Peの保持するコマンド
		/// </summary>
		PeCommand
	}
	
	public enum LauncherApplicationShow
	{
		Default,
		Normal,
		Minimum,
		Maxim,
	}
	
	public class LauncherItem: Item
	{
		const string AttributeName = "name";
		
		public LauncherItem()
		{
			HistoryWorkDirectoryList = new List<string>();
			HistoryOptionCommandList = new List<string>();
			Tags = new List<string>();
			
			ResistTimestamp = DateTime.MinValue;
			UpdateTimestamp = DateTime.MinValue;
		}
		
		/// <summary>
		/// アイテム名
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// ランチャー種別
		/// </summary>
		public LauncherType LauncherType { get; set; }
		/// <summary>
		/// アイテム実行時のプロセスを監視する
		/// </summary>
		public bool ProcessWatcher { get; set; }
		/// <summary>
		/// 標準出力を受け取る
		/// </summary>
		public bool GetStdOutput { get; set; }
		/// <summary>
		/// アイテムの実行内容
		/// 
		/// LauncherTypeの内容により変化する
		/// </summary>
		public string LauncherCommand { get; set; }
		/// <summary>
		/// 標準で使用する作業ディレクトリ
		/// </summary>
		public string WorkDirectory { get; set; }
		/// <summary>
		/// 標準で使用するオプション
		/// </summary>
		public string OptionCommand { get; set; }
		/// <summary>
		/// ランチャーアイテムの実行時の表示状態
		/// </summary>
		public LauncherApplicationShow LauncherApplicationShow { get; set; }
		/// <summary>
		/// アイコンパス
		/// </summary>
		public string IconPath { get; set; }
		/// <summary>
		/// アイコンインデックス
		/// </summary>
		public int IconIndex { get; set; }
		/// <summary>
		/// アイテム実行回数
		/// </summary>
		public uint HistoryExecuteCount { get; set; }
		/// <summary>
		/// アイテムの登録済み作業ディレクトリ以外で使用された作業ディレクトリ
		/// </summary>
		public List<string> HistoryWorkDirectoryList { get; private set; }
		/// <summary>
		/// アイテムの登録済みオプション以外で使用されたオプション
		/// </summary>
		public List<string> HistoryOptionCommandList { get; private set; }
		/// <summary>
		/// タグ
		/// </summary>
		public List<string> Tags { get; private set; }
		/// <summary>
		/// 登録日時
		/// </summary>
		public DateTime ResistTimestamp { get; set; }
		/// <summary>
		/// 更新日時
		/// </summary>
		public DateTime UpdateTimestamp { get; set; }
		
		public override System.Xml.XmlElement ToXmlElement(System.Xml.XmlDocument xml, Pe.IF.ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			
			
			return result;
		}
		
	}
	
	
}
