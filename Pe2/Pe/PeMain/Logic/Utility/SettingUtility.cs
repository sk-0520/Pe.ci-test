namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// 設定データを上手いことなんやかんやする。
	/// </summary>
	public static class SettingUtility
	{
		public static void IncrementRunningInformation(RunningInformationItemModel model)
		{
			CheckUtility.EnforceNotNull(model);

			model.LastExecuteVersion = Constants.assemblyVersion;
			model.ExecuteCount += 1;
		}

		public static LauncherGroupItemModel CreateLauncherGroup(LauncherGroupItemCollectionModel group, LanguageManager language)
		{
			var newGroupId = language["new/group-id"];
			var newGroupName = language["new/group-id"];

			var result = new LauncherGroupItemModel();
			if(group != null || group.Any()) {
				newGroupId = TextUtility.ToUnique(
					newGroupId,
					group.Keys,
					(s, i) => string.Format("{0}_{1}", s, i)
				);
				newGroupName = TextUtility.ToUniqueDefault(newGroupId, group.Select(g => g.Name));
			}
			result.Id = newGroupId;
			result.Name = newGroupName;

			return result;
		}
	}
}
