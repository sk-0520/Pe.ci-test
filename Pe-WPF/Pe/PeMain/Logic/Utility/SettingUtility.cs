namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	/// <summary>
	/// 設定データを上手いことなんやかんやする。
	/// </summary>
	public static class SettingUtility
	{
		public static bool CheckAccept(RunningInformationItemModel model, INonProcess nonProcess)
		{
			if(!model.Accept) {
				// 完全に初回
				nonProcess.Logger.Debug("first");
				return false;
			}

			if(model.LastExecuteVersion == null) {
				// 何らかの理由で前回実行時のバージョン格納されていない
				nonProcess.Logger.Debug("last version == null");
				return false;
			}

			if(model.LastExecuteVersion < Constants.acceptVersion) {
				// 前回バージョンから強制定期に使用許諾が必要
				nonProcess.Logger.Debug("last version < accept version");
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

		public static LauncherGroupItemModel CreateLauncherGroup(LauncherGroupItemCollectionModel group, INonProcess nonProcess)
		{
			var newGroupId = nonProcess.Language["new/group-id"];
			var newGroupName = nonProcess.Language["new/group-id"];

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

		public static void InitializeMainSetting(MainSettingModel setting, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);

			foreach(var toolbar in setting.Toolbar) {
				if(toolbar.FloatToolbar.WidthButtonCount <= 0) {
					toolbar.FloatToolbar.WidthButtonCount = 1;
				}
				if(toolbar.FloatToolbar.HeightButtonCount <= 0) {
					toolbar.FloatToolbar.HeightButtonCount = 1;
				}
			}
		}

		public static void InitializeLauncherItemSetting(LauncherItemSettingModel setting, INonProcess nonProcess)
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
				setting.Items.Add(new LauncherItemModel() {
					Id = "test4",
					Name = "name4",
					LauncherKind = LauncherKind.File,
					Command = @"c:\"
				});
			}
			// --------------------------------
		}

		public static void InitializeLauncherGroupSetting(LauncherGroupSettingModel setting, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);
			CheckUtility.EnforceNotNull(nonProcess);

			if(!setting.Groups.Any()) {
				var initGroup = CreateLauncherGroup(setting.Groups, nonProcess);
				//------------------------
				initGroup.LauncherItems = new ObservableCollection<string>(new[] {
					"test1",
					"test2",
					"test3",
					"test4",
				});
				//------------------------
				setting.Groups.Add(initGroup);
			}
		}

		public static void IncrementLauncherItem(LauncherItemModel launcherItem, string option, string workDirPath, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(launcherItem);
		}

	}
}
