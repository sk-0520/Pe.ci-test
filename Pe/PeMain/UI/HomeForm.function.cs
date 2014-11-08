namespace PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;

	using PeUtility;
	using PeMain.Data;

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
		
		IEnumerable<string> SplitLauncherItemText(string text)
		{
			return text.SplitLines().Select(s => s.Trim()).Where(s => !s.StartsWith("#", StringComparison.Ordinal));
		}
		
		void MakeDefaultLauncherItem()
		{
			var path = Literal.ApplicationDefaultLauncherItemPath;
			var srcBuffer = File.ReadAllText(path);
			var lngBuffer = CommonData.Language.ReplaceAll(srcBuffer);
			var xml = XElement.Parse(lngBuffer);
			
			// アイテム取得
			var defaultItemList = new List<LauncherItem>();
			var itemElements = xml.Elements("items").Elements("LauncherItem");
			foreach(var itemElement in itemElements) {
				var item = Serializer.LoadString<LauncherItem>(itemElement.ToString());
				var isAdd = false;
				// データの補正
				if(item.LauncherType.IsIn(LauncherType.File, LauncherType.Directory)) {
					// コマンド修正
					foreach(var command in SplitLauncherItemText(item.Command)) {
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
					// アイコン修正
					foreach(var iconPath in SplitLauncherItemText(item.IconItem.Path)) {
						var expandPath = Environment.ExpandEnvironmentVariables(iconPath);
						if(File.Exists(expandPath)) {
							item.IconItem.Path = iconPath;
						}
					}
				}
				
				if(isAdd) {
					defaultItemList.Add(item);
				}
			}
			
			// ユーザーのシステムから取得
			var allUsersFiles =
				Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu))
				.Concat(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms)))
				;
			var nowUserFiles =
				Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu))
				.Concat(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Programs)))
				;
			var mergeFiles = nowUserFiles
				.Concat(allUsersFiles)
				.Where(s => PathUtility.IsShortcutPath(s) || PathUtility.IsExecutePath(s))
				.Distinct()
				;
			
			var userItemList = new List<LauncherItem>(mergeFiles.Count());
			foreach(var mergeFile in mergeFiles) {
				try {
					var item = LauncherItem.FileLoad(mergeFile, false);
					userItemList.Add(item);
				}catch(Exception ex) {
					// #68 暫定回避
					CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
				}
			}
			
			// アイテムマージ
			// TODO: マージに何も考えてない
			var mergeItemList = defaultItemList.Union(userItemList).ToArray();
			
			// グループ作成
			var toolbarGroupItemList = new List<ToolbarGroupItem>();
			var groupElements = xml.Elements("groups").Elements("group");
			foreach(var groupElement in groupElements) {
				var groupName = groupElement.Attribute("Name").Value;
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = groupName;
				var useGroup = false;
				foreach(var itemName in groupElement.Elements("item").Select(node => node.Attribute("Name").Value)) {
					if(mergeItemList.Any(item => item.Name == itemName)) {
						toolbarGroupItem.ItemNames.Add(itemName);
						useGroup = true;
					}
				}
				if(useGroup) {
					toolbarGroupItemList.Add(toolbarGroupItem);
				}
			}
			// ユーザーのシステムからグループ構築
			if(userItemList.Any()) {
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = CommonData.Language["default/group/programs"];
				foreach(var name in userItemList.Select(item => item.Name)) {
					toolbarGroupItem.ItemNames.Add(name);
				}
				toolbarGroupItemList.Add(toolbarGroupItem);
			}
			
			// とうろく！
			ItemFinded = true;
			Serializer.SaveFile(mergeItemList, "Z:\\a.xml");
			Serializer.SaveFile(toolbarGroupItemList, "Z:\\b.xml");
		}
	}
}
