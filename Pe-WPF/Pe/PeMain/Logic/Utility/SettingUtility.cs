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
		static Guid[] debugGuidList = new[] {
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
		};

		public static bool CheckAccept(RunningInformationSettingModel model, INonProcess nonProcess)
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

		/// <summary>
		/// 実行情報の切り上げ。
		/// </summary>
		/// <param name="model"></param>
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

		public static LauncherGroupItemModel CreateLauncherGroup(LauncherGroupItemCollectionModel group, INonProcess nonProcess)
		{
			var result = CreateModelName(group, nonProcess.Language, "new/group-name");
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		public static TemplateIndexItemModel CreateTemplateIndexItem(TemplateIndexItemCollectionModel items, INonProcess nonProcess)
		{
			var result = CreateModelName(items, nonProcess.Language, "new/template-name");
			return result;
		}

		public static void InitializeToolbar(ToolbarItemModel model, Version previousVersion, INonProcess nonProcess)
		{
			if (model.FloatToolbar.WidthButtonCount <= 0) {
				model.FloatToolbar.WidthButtonCount = 1;
			}
			if (model.FloatToolbar.HeightButtonCount <= 0) {
				model.FloatToolbar.HeightButtonCount = 1;
			}
		}

		public static void InitializeWindowSaveSetting(WindowSaveSettingModel model, Version previousVersion, INonProcess nonProcess)
		{
			model.SaveCount = Constants.windowSaveCount.GetClamp(model.SaveCount);
			model.SaveIntervalTime = Constants.windowSaveIntervalTime.GetClamp(model.SaveIntervalTime);
		}

		public static void InitializeNoteSetting(NoteSettingModel model, Version previousVersion, INonProcess nonProcess)
		{
			if (model.ForeColor == default(Color)) {
				model.ForeColor = Constants.noteForeColor;
			}
			if (model.BackColor == default(Color)) {
				model.BackColor = Constants.noteBackColor;
			}
		}

		public static void InitializeClipboardSetting(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			setting.WaitTime = Constants.clipboardWaitTime.GetClamp(setting.WaitTime);

			if(setting.ItemsListWidth <= 0) {
				setting.ItemsListWidth = Constants.clipboardItemsListWidth;
			}
		}

		public static void InitializeTemplateSetting(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(setting.ItemsListWidth <= 0) {
				setting.ItemsListWidth = Constants.templateItemsListWidth;
			}
			if(setting.ReplaceListWidth <= 0) {
				setting.ReplaceListWidth = Constants.templateReplaceListWidth;
			}
		}

		/// <summary>
		/// 本体設定を補正。
		/// </summary>
		/// <param name="setting"></param>
		/// <param name="previousVersion"></param>
		/// <param name="nonProcess"></param>
		public static void InitializeMainSetting(MainSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);

			foreach(var toolbar in setting.Toolbar) {
				InitializeToolbar(toolbar, previousVersion, nonProcess);
			}

			InitializeNoteSetting(setting.Note, previousVersion, nonProcess);
			InitializeWindowSaveSetting(setting.WindowSave, previousVersion, nonProcess);
			InitializeClipboardSetting(setting.Clipboard, previousVersion, nonProcess);
			InitializeTemplateSetting(setting.Template, previousVersion, nonProcess);
		}

		public static void InitializeLauncherItemSetting(LauncherItemSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);

			// --------------------------------
			if(!setting.Items.Any()) {
				setting.Items.Add(new LauncherItemModel() {
					Id = debugGuidList[0],
					Name = "name1",
					LauncherKind = LauncherKind.File,
					Command = @"C:\Windows\System32\mspaint.exe"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = debugGuidList[1],
					Name = "name2",
					LauncherKind = LauncherKind.File,
					Command = @"%windir%\system32\calc.exe"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = debugGuidList[2],
					Name = "name3",
					LauncherKind = LauncherKind.Command,
					Command = @"ping"
				});
				setting.Items.Add(new LauncherItemModel() {
					Id = debugGuidList[3],
					Name = "name4",
					LauncherKind = LauncherKind.File,
					Command = @"c:\"
				});
			}
			// --------------------------------
		}

		public static void InitializeLauncherGroupSetting(LauncherGroupSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);
			CheckUtility.EnforceNotNull(nonProcess);

			if(!setting.Groups.Any()) {
				var initGroup = CreateLauncherGroup(setting.Groups, nonProcess);
				//------------------------
				initGroup.LauncherItems = new CollectionModel<Guid>(new[] {
					debugGuidList[0],
					debugGuidList[1],
					debugGuidList[2],
					debugGuidList[3],
				});
				//------------------------
				setting.Groups.Add(initGroup);
			}
		}

		public static void IncrementLauncherItem(LauncherItemModel launcherItem, string option, string workDirPath, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(launcherItem);
		}

		public static void InitializeNoteIndexSetting(NoteIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);
			foreach (var noteIndex in setting.Items) {
				InitializeNoteIndexItem(noteIndex, previousVersion, nonProcess);
			}
		}

		public static void InitializeNoteIndexItem(NoteIndexItemModel indexItem, Version previousVersion, INonProcess appNonProcess)
		{
			CheckUtility.EnforceNotNull(indexItem);
			if (double.IsNaN(indexItem.Font.Size) || indexItem.Font.Size <= 0) {
				indexItem.Font.Size = Constants.noteFontSize.median;
			}
			indexItem.Font.Size = Constants.noteFontSize.GetClamp(indexItem.Font.Size);
		}

		public static void InitializeTemplateIndexSetting(TemplateIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);
		}

		public static void InitializeClipboardIndexSetting(ClipboardIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(setting);
		}

	}
}
