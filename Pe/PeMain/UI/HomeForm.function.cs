using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	partial class HomeForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplySetting()
		{
			ApplyLanguage();
		}
		
		IEnumerable<string> SplitLauncherItem(string s)
		{
			return s.Split('\t');
		}
		
		void MakeDefaultLauncherItem()
		{
			var path = Literal.ApplicationDefaultLauncherItemPath;
			var srcBuffer = File.ReadAllText(path);
			var lngBuffer = CommonData.Language.ReplaceAll(srcBuffer);
			var xml = XElement.Parse(lngBuffer);
			
			// アイテム取得
			var itemList = new List<LauncherItem>();
			var itemElements = xml.Elements("items").Elements("LauncherItem");
			foreach(var itemElement in itemElements) {
				var item = Serializer.LoadString<LauncherItem>(itemElement.ToString());
				var isAdd = false;
				// データの補正
				if(item.LauncherType.IsIn(LauncherType.File, LauncherType.Directory)) {
					// コマンド修正
					foreach(var command in SplitLauncherItem(item.Command)) {
						var expandPath = Environment.ExpandEnvironmentVariables(command);
						if(item.LauncherType == LauncherType.File) {
							if(File.Exists(expandPath)) {
								item.Command = command;
								isAdd = true;
								break;
							}
						} else {
							Debug.Assert(item.LauncherType == LauncherType.Directory);
							if(Directory.Exists(expandPath)) {
								item.Command = command;
								isAdd = true;
								break;
							}
						}
					}
				}
				
				if(isAdd) {
					itemList.Add(item);
				}
			}
			
			ItemFinded = true;
			Serializer.SaveFile(itemList, "Z:\\a.xml");
		}
	}
}
