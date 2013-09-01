/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 19:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	public class HistoryItemData: ItemData
	{
		const string TagWorkDirectory = "workdirectory";
		const string TagOptionCommand = "optioncommand";
		const string AttributeExecuteCount = "count";
		
		public HistoryItemData()
		{
			WorkDirectory = new List<string>();
			OptionCommand = new List<string>();
		}
		public override string Name { get { return "history"; } }
		
		public int ExecuteCount { get; set; }
		public List<string> WorkDirectory { get; private set; }
		public List<string> OptionCommand { get; private set; }
		
		public override void Clear()
		{
			base.Clear();
			
			ExecuteCount = 0;
			WorkDirectory.Clear();
			OptionCommand.Clear();
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributeExecuteCount, ExecuteCount.ToString());
			
			var workDirElement = xml.CreateElement(TagWorkDirectory);
			foreach(var dir in WorkDirectory) {
				var dataElement = xml.CreateElement(DataElement.Tag);
				dataElement.SetAttribute(DataElement.Attribute, dir);
				workDirElement.AppendChild(dataElement);
			}
			result.AppendChild(workDirElement);
			
			var optionElement = xml.CreateElement(TagOptionCommand);
			foreach(var opt in OptionCommand) {
				var dataElement = xml.CreateElement(DataElement.Tag);
				dataElement.SetAttribute(DataElement.Attribute, opt);
				optionElement.AppendChild(dataElement);
			}
			result.AppendChild(optionElement);
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			int outInt;
			var unsafeExecuteCount = element.GetAttribute(AttributeExecuteCount);
			if(int.TryParse(unsafeExecuteCount, out outInt)) {
				ExecuteCount = outInt;
			}
			
			var workDirElement = element[TagWorkDirectory];
			if(workDirElement != null) {
				foreach(XmlElement dataElement in workDirElement.GetElementsByTagName(DataElement.Tag)) {
					var dir = dataElement.GetAttribute(DataElement.Attribute);
					WorkDirectory.Add(dir);
				}
			}
			
			var optionElement = element[TagOptionCommand];
			if(optionElement != null) {
				foreach(XmlElement dataElement in optionElement.GetElementsByTagName(DataElement.Tag)) {
					var option = dataElement.GetAttribute(DataElement.Attribute);
					OptionCommand.Add(option);
				}
			}
		}
	}
}
