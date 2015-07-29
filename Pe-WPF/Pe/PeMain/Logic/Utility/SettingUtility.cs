namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	/// <summary>
	/// 設定データを上手いことなんやかんやする。
	/// </summary>
	public static class SettingUtility
	{
		static Guid[] guidList = new[] {
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
		};
		public static bool CheckAccept(RunningInformationSettingModel model, INonProcess appNonProcess)
		{
			if(!model.Accept) {
				// 完全に初回
				appNonProcess.Logger.Debug("first");
				return false;
			}

			if(model.LastExecuteVersion == null) {
				// 何らかの理由で前回実行時のバージョン格納されていない
				appNonProcess.Logger.Debug("last version == null");
				return false;
			}

			if(model.LastExecuteVersion < Constants.acceptVersion) {
				// 前回バージョンから強制定期に使用許諾が必要
				appNonProcess.Logger.Debug("last version < accept version");
				return false;
			}

			return true;
		}


		public static void IncrementRunningInformation(RunningInformationSettingModel model)
		{
			CheckUtility.EnforceNotNull(model);

			model.LastExecuteVersion = Constants.assemblyVersion;
			model.ExecuteCount += 1;
		}

		static TModel CreateModelName<TModel>(IEnumerable<TModel> items, ILanguage language, string nameKey)
			where TModel: IName, new()
		{
			var newName = language[nameKey];

			var result = new TModel();
			if (items != null || items.Any()) {
				newName = TextUtility.ToUniqueDefault(newName, items.Select(g => g.Name));
			}
			result.Name = newName;

			return result;
		}

		public static LauncherGroupItemModel CreateLauncherGroup(LauncherGroupItemCollectionModel group, INonProcess appNonProcess)
		{
			////var newGroupId = appNonProcess.Language["new/group-id"];
			//var newGroupName = appNonProcess.Language["new/group-name"];

			//var result = new LauncherGroupItemModel();
			//if(group != null || group.Any()) {
			//	//newGroupId = TextUtility.ToUnique(
			//	//	newGroupId,
			//	//	group.Keys,
			//	//	(s, i) => string.Format("{0}_{1}", s, i)
			//	//);
			//	newGroupName = TextUtility.ToUniqueDefault(newGroupName, group.Select(g => g.Name));
			//}
			////result.Id = newGroupId;
			//result.Name = newGroupName;

			//return result;
			var result = CreateModelName(group, appNonProcess.Language, "new/group-name");
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		/// <param name="appNonProcess"></param>
		/// <returns></returns>
		public static TemplateIndexItemModel CreateTemplateIndexItem(TemplateIndexItemCollectionModel items, INonProcess appNonProcess)
		{
			var result = CreateModelName(items, appNonProcess.Language, "new/template-name");
			return result;
		}

		public static void InitializeToolbar(ToolbarItemModel model, INonProcess appNonProcess)
		{
			if (model.FloatToolbar.WidthButtonCount <= 0) {
				model.FloatToolbar.WidthButtonCount = 1;
			}
			if (model.FloatToolbar.HeightButtonCount <= 0) {
				model.FloatToolbar.HeightButtonCount = 1;
			}
		}

		public static void InitializeWindowSaveSetting(WindowSaveSettingModel model, INonProcess appNonProcess)
		{
			model.SaveCount = Constants.windowSaveCount.GetClamp(model.SaveCount);
			model.SaveIntervalTime = Constants.windowSaveIntervalTime.GetClamp(model.SaveIntervalTime);
		}

		public static void InitializeNoteSetting(NoteSettingModel model, INonProcess appNonProcess)
		{
			if (model.ForeColor == default(Color)) {
				model.ForeColor = Constants.noteForeColor;
			}
			if (model.BackColor == default(Color)) {
				model.BackColor = Constants.noteBackColor;
			}
		}

		public static void InitializeClipboardSetting(ClipboardSettingModel setting, INonProcess appNonProcess)
		{
			setting.WaitTime = Constants.clipboardWaitTime.GetClamp(setting.WaitTime);

			if(setting.ItemsListWidth <= 0) {
				setting.ItemsListWidth = Constants.clipboardItemsListWidth;
			}
		}

		public static void InitializeTemplateSetting(TemplateSettingModel setting, INonProcess appNonProcess)
		{
			if(setting.ItemsListWidth <= 0) {
				setting.ItemsListWidth = Constants.templateItemsListWidth;
			}
			if(setting.ReplaceListWidth <= 0) {
				setting.ReplaceListWidth = Constants.templateReplaceListWidth;
			}
		}


		public static void InitializeMainSetting(MainSettingModel setting, INonProcess appNonProcess)
		{
			CheckUtility.EnforceNotNull(setting);

			foreach(var toolbar in setting.Toolbar) {
				InitializeToolbar(toolbar, appNonProcess);
			}

			InitializeNoteSetting(setting.Note, appNonProcess);
			InitializeWindowSaveSetting(setting.WindowSave, appNonProcess);
			InitializeClipboardSetting(setting.Clipboard, appNonProcess);
			InitializeTemplateSetting(setting.Template, appNonProcess);
		}

		public static void InitializeLauncherItemSetting(LauncherItemSettingModel setting, INonProcess appNonProcess)
		{
			CheckUtility.EnforceNotNull(setting);

			// --------------------------------
			if(!setting.Items.Any()) {
				setting.Items.Add(new LauncherItemModel() {
					Id = guidList[0],
					Name = "name1",
					LauncherKind = LauncherKind.File,
					Command = @"C:\Windows\System32\mspaint.exe"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = guidList[1],
					Name = "name2",
					LauncherKind = LauncherKind.File,
					Command = @"%windir%\system32\calc.exe"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = guidList[2],
					Name = "name3",
					LauncherKind = LauncherKind.Command,
					Command = @"ping"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = guidList[3],
					Name = "name4",
					LauncherKind = LauncherKind.File,
					Command = @"c:\"
				});
			}
			// --------------------------------
		}

		public static void InitializeLauncherGroupSetting(LauncherGroupSettingModel setting, INonProcess appNonProcess)
		{
			CheckUtility.EnforceNotNull(setting);
			CheckUtility.EnforceNotNull(appNonProcess);

			if(!setting.Groups.Any()) {
				var initGroup = CreateLauncherGroup(setting.Groups, appNonProcess);
				//------------------------
				initGroup.LauncherItems = new CollectionModel<Guid>(new[] {
					guidList[0],
					guidList[1],
					guidList[2],
					guidList[3],
				});
				//------------------------
				setting.Groups.Add(initGroup);
			}
		}

		public static void IncrementLauncherItem(LauncherItemModel launcherItem, string option, string workDirPath, INonProcess appNonProcess)
		{
			CheckUtility.EnforceNotNull(launcherItem);
		}

	}
}
