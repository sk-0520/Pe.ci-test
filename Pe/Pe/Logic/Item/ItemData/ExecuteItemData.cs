using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	public class ExecuteItemData: ItemData
	{
		const string AttributeLauncherType = "type";
		const string AttributeLauncherCommand = "command";
		const string TagPath = "path";
		const string AttributeWorkDirectory = "workdirectory";
		const string AttributeOptionCommand = "optioncommand";
		const string TagView = "view";
		const string AttributeLauncherApplicationShow = "show";
		const string TagSwitch ="switch";
		const string AttributeProcessWatcher ="pswatch";
		const string AttributeGetStdOutput ="getstdout";
		
		public ExecuteItemData()
		{
		}
		
		public override string Name { get { return "execute"; } }
		
		/// <summary>
		/// ランチャー種別
		/// </summary>
		public LauncherType LauncherType { get; set; }
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
		/// アイテム実行時のプロセスを監視する
		/// </summary>
		public bool ProcessWatcher { get; set; }
		/// <summary>
		/// 標準出力を受け取る
		/// </summary>
		public bool GetStdOutput { get; set; }
		
		
		public override void Clear()
		{
			base.Clear();
			
			LauncherType = default(Logic.LauncherType);
			ProcessWatcher = default(bool);
			GetStdOutput = default(bool);
			LauncherCommand = default(string);
			WorkDirectory = default(string);
			OptionCommand = default(string);
			LauncherApplicationShow = default(LauncherApplicationShow);
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributeLauncherType, LauncherType.ToString());
			result.SetAttribute(AttributeLauncherCommand, LauncherCommand);
			
			var pathElement = xml.CreateElement(TagPath);
			pathElement.SetAttribute(AttributeWorkDirectory, WorkDirectory);
			pathElement.SetAttribute(AttributeOptionCommand, OptionCommand);
			result.AppendChild(pathElement);
			
			var viewElement = xml.CreateElement(TagView);
			viewElement.SetAttribute(AttributeLauncherApplicationShow, LauncherApplicationShow.ToString());
			result.AppendChild(viewElement);

			var switchElement = xml.CreateElement(TagSwitch);
			switchElement.SetAttribute(AttributeProcessWatcher, ProcessWatcher.ToString());
			switchElement.SetAttribute(AttributeGetStdOutput, GetStdOutput.ToString());
			result.AppendChild(switchElement);
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var unsafeLauncherCommand = element.GetAttribute(AttributeLauncherCommand);
			var unsafeLauncherType = element.GetAttribute(AttributeLauncherType);
			Logic.LauncherType outLauncherType;
			
			LauncherCommand = unsafeLauncherCommand;
			if(Logic.LauncherType.TryParse(unsafeLauncherType, out outLauncherType)) {
				LauncherType = outLauncherType;
			}
			
			var pathElement = element[TagPath];
			if(pathElement != null) {
				var unsafeWorkDirectory = pathElement.GetAttribute(AttributeWorkDirectory);
				var unsafeOptionCommand = pathElement.GetAttribute(AttributeOptionCommand);
				
				WorkDirectory = unsafeWorkDirectory;
				OptionCommand = unsafeOptionCommand;
			}
			
			var viewElement = element[TagPath];
			if(viewElement != null) {
				var unsafeLauncherApplicationShow = viewElement.GetAttribute(AttributeLauncherApplicationShow);

				Logic.LauncherApplicationShow outLauncherApplicationShow;
				
				if(Logic.LauncherApplicationShow.TryParse(unsafeLauncherApplicationShow, out outLauncherApplicationShow)) {
					LauncherApplicationShow = outLauncherApplicationShow;
				}
			}
			
			var switchElement = element[TagSwitch];
			if(switchElement != null) {
				var unsafeProcessWatcher = element.GetAttribute(AttributeProcessWatcher);
				var unsafeGetStdOutput = element.GetAttribute(AttributeGetStdOutput);
				
				bool outBool;
				
				if(bool.TryParse(unsafeProcessWatcher, out outBool)) {
					ProcessWatcher = outBool;
				}
				if(bool.TryParse(unsafeGetStdOutput, out outBool)) {
					GetStdOutput = outBool;
				}
			}
		}
	}
}