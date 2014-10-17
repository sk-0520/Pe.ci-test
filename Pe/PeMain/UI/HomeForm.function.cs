using System;
using System.Collections.Generic;
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
		
		void MakeDefaultLauncherItem()
		{
			var path = Literal.ApplicationDefaultLauncherItemPath;
			var xml = XElement.Load(path);
			
			// アイテム取得
			var itemList = new List<LauncherItem>();
			var itemElements = xml.Elements("items").Elements("LauncherItem");
			foreach(var itemElement in itemElements) {
				var item = Serializer.LoadString<LauncherItem>(itemElement.ToString());
			}
		}
	}
}
