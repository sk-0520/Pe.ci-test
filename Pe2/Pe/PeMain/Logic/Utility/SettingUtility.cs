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
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	/// <summary>
	/// 設定データを上手いことなんやかんやする。
	/// </summary>
	public static class SettingUtility
	{
		public static bool CheckAccept(RunningInformationItemModel model, ILogger logger)
		{
			if(!model.Accept) {
				// 完全に初回
				logger.Debug("first");
				return false;
			}

			if(model.LastExecuteVersion == null) {
				// 何らかの理由で前回実行時のバージョン格納されていない
				logger.Debug("last version == null");
				return false;
			}

			if(model.LastExecuteVersion < Constants.acceptVersion) {
				// 前回バージョンから強制定期に使用許諾が必要
				logger.Debug("last version < accept version");
				return false;
			}

			return true;
		}


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

		public static void InitializeMainSetting(MainSettingModel setting, ILogger logger)
		{
			CheckUtility.EnforceNotNull(setting);

			foreach(var toolbar in setting.Toolbar.Items) {
				if(toolbar.FloatToolbar.WidthButtonCount <= 0) {
					toolbar.FloatToolbar.WidthButtonCount = 1;
				}
				if(toolbar.FloatToolbar.HeightButtonCount <= 0) {
					toolbar.FloatToolbar.HeightButtonCount = 1;
				}
			}
		}

		public static void InitializeLauncherItemSetting(LauncherItemSettingModel setting, ILogger logger)
		{
			CheckUtility.EnforceNotNull(setting);

			// --------------------------------
			if(!setting.Items.Any()) {
				setting.Items.Add(new LauncherItemModel() {
					Id = "test1",
					Name = "name1",
					LauncherKind = LauncherKind.File,
					Command = @"C:\Windows\System32\mspaint.exe"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = "test2",
					Name = "name2",
					LauncherKind = LauncherKind.File,
					Command = @"%windir%\system32\calc.exe"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = "test3",
					Name = "name3",
					LauncherKind = LauncherKind.Command,
					Command = @"ping"
				});
			}
			// --------------------------------
		}

		public static void InitializeLauncherGroupSetting(LauncherGroupSettingModel setting, LanguageManager language, ILogger logger)
		{
			CheckUtility.EnforceNotNull(setting);
			CheckUtility.EnforceNotNull(language);

			if(!setting.Groups.Any()) {
				var initGroup = CreateLauncherGroup(setting.Groups, language);
				//------------------------
				initGroup.LauncherItems.AddRange(new[] {
					"test1",
					"test2",
					"test3",
				});
				//------------------------
				setting.Groups.Add(initGroup);
			}
		}

	}
}
