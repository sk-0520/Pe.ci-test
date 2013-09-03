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
		/*
		const string AttributeLauncherType = "type";
		const string AttributeProcessWatcher = "pswatch";
		const string AttributeGetStdOutput = "getstdout";
		const string AttributeLauncherCommand = "command";
		const string AttributeWorkDirectory = "dir";
		const string AttributeOptionCommand = "option";
		const string AttributeLauncherApplicationShow = "show";
		*/
		
		public LauncherItem()
		{
			ExecuteItemData = new ExecuteItemData();
			IconItemData = new IconItemData();
			HistoryItemData = new HistoryItemData();
			TagItemData = new TagItemData();
			TimestampItemData = new TimestampItemData();
		}
		/*
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
		//*/
		/// <summary>
		/// 実行データ
		/// </summary>
		public ExecuteItemData ExecuteItemData { get; private set; }
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
			
			ExecuteItemData.Clear();
			IconItemData.Clear();
			HistoryItemData.Clear();
			TagItemData.Clear();
			TimestampItemData.Clear();
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			/*
			result.SetAttribute(AttributeLauncherType, LauncherType.ToString());
			result.SetAttribute(AttributeProcessWatcher, ProcessWatcher.ToString());
			result.SetAttribute(AttributeGetStdOutput, GetStdOutput.ToString());
			result.SetAttribute(AttributeLauncherCommand, LauncherCommand);
			result.SetAttribute(AttributeWorkDirectory, WorkDirectory);
			result.SetAttribute(AttributeOptionCommand, OptionCommand);
			result.SetAttribute(AttributeLauncherApplicationShow, LauncherApplicationShow.ToString());
			*/
			result.AppendChild(ExecuteItemData.ToXmlElement(xml, expArg));
			result.AppendChild(IconItemData.ToXmlElement(xml, expArg));
			result.AppendChild(HistoryItemData.ToXmlElement(xml, expArg));
			result.AppendChild(TagItemData.ToXmlElement(xml, expArg));
			result.AppendChild(TimestampItemData.ToXmlElement(xml, expArg));

			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			/*
			var unsafeLauncherType = element.GetAttribute(AttributeLauncherType);
			var unsafeProcessWatcher = element.GetAttribute(AttributeProcessWatcher);
			var unsafeGetStdOutput = element.GetAttribute(AttributeGetStdOutput);
			var unsafeLauncherCommand = element.GetAttribute(AttributeLauncherCommand);
			var unsafeWorkDirectory = element.GetAttribute(AttributeWorkDirectory);
			var unsafeOptionCommand = element.GetAttribute(AttributeOptionCommand);
			var unsafeLauncherApplicationShow = element.GetAttribute(AttributeLauncherApplicationShow);
			
			Logic.LauncherType outLauncherType;
			Logic.LauncherApplicationShow outLauncherApplicationShow;
			bool outBool;
			
			if(Logic.LauncherType.TryParse(unsafeLauncherType, out outLauncherType)) {
				LauncherType = outLauncherType;
			}
			if(bool.TryParse(unsafeProcessWatcher, out outBool)) {
				ProcessWatcher = outBool;
			}
			if(bool.TryParse(unsafeGetStdOutput, out outBool)) {
				GetStdOutput = outBool;
			}
			
			LauncherCommand = unsafeLauncherCommand;
			WorkDirectory = unsafeWorkDirectory;
			OptionCommand = unsafeOptionCommand;
			
			if(Logic.LauncherApplicationShow.TryParse(unsafeLauncherApplicationShow, out outLauncherApplicationShow)) {
				LauncherApplicationShow = outLauncherApplicationShow;
			}
			*/
			
			var executeItemDataElement = element[ExecuteItemData.Name];
			if(executeItemDataElement != null) {
				ExecuteItemData.FromXmlElement(executeItemDataElement, impArg);
			}
			
			var iconDataElement = element[IconItemData.Name];
			if(iconDataElement != null) {
				IconItemData.FromXmlElement(iconDataElement, impArg);
			}
			
			var historyItemData = element[HistoryItemData.Name];
			if(historyItemData != null) {
				HistoryItemData.FromXmlElement(historyItemData, impArg);
			}

			var tagItemData = element[TagItemData.Name];
			if(tagItemData != null) {
				TagItemData.FromXmlElement(tagItemData, impArg);
			}
			
			var timestampItemData = element[TimestampItemData.Name];
			if(timestampItemData != null) {
				TimestampItemData.FromXmlElement(timestampItemData, impArg);
			}
		}
	}
}
