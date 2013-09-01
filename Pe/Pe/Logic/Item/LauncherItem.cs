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
using System.Xml;

using Pe.IF;
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
	
	public class LauncherItem: TitleItem
	{
		
		public LauncherItem()
		{
			IconItemData = new IconItemData();
			HistoryItemData = new HistoryItemData();
			TagItemData = new TagItemData();
			TimestampItemData = new TimestampItemData();
		}
		
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
		/// アイコン
		/// </summary>
		public IconItemData IconItemData { get; private set; }
		/// <summary>
		/// 過去分
		/// </summary>
		public HistoryItemData HistoryItemData { get; private set; }
		/// <summary>
		/// タグ
		/// </summary>
		public TagItemData TagItemData { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public TimestampItemData TimestampItemData { get; set; }
		
		public override void Clear()
		{
			base.Clear();
			
			IconItemData.Clear();
			HistoryItemData.Clear();
			TagItemData.Clear();
			TimestampItemData.Clear();
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			// TODO: 未実装
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			// TODO: 未実装
		}
		
	}
	
	
}
