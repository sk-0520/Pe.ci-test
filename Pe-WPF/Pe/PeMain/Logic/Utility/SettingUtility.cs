namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using Implement = ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement;

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

		public static bool IsIllegalPlusNumber(double number)
		{
			return double.IsNaN(number) || number <= 0;
		}

		public static bool IsIllegalPlusNumber(int number)
		{
			return number <= 0;
		}

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

		public static void InitializeToolbarSetting(ToolbarSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeToolbarSetting.Correction(setting, previousVersion, nonProcess);

			foreach(var toolbar in setting.Items) {
				InitializeToolbar(toolbar, previousVersion, nonProcess);
			}
		}

		public static void InitializeToolbar(ToolbarItemModel model, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeToolbar.Correction(model, previousVersion, nonProcess);

			if(IsIllegalPlusNumber(model.FloatToolbar.WidthButtonCount)) {
				model.FloatToolbar.WidthButtonCount = 1;
			}
			if(IsIllegalPlusNumber(model.FloatToolbar.HeightButtonCount)) {
				model.FloatToolbar.HeightButtonCount = 1;
			}

			model.HideWaitTime = Constants.toolbarHideWaitTime.GetClamp(model.HideWaitTime);
			model.HideAnimateTime = Constants.toolbarHideAnimateTime.GetClamp(model.HideAnimateTime);
		}

		public static void InitializeWindowSaveSetting(WindowSaveSettingModel model, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeWindowSaveSetting.Correction(model, previousVersion, nonProcess);

			model.SaveCount = Constants.windowSaveCount.GetClamp(model.SaveCount);
			model.SaveIntervalTime = Constants.windowSaveIntervalTime.GetClamp(model.SaveIntervalTime);
		}

		public static void InitializeNoteSetting(NoteSettingModel model, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeNoteSetting.Correction(model, previousVersion, nonProcess);

			if (model.ForeColor == default(Color)) {
				model.ForeColor = Constants.noteForeColor;
			}
			if (model.BackColor == default(Color)) {
				model.BackColor = Constants.noteBackColor;
			}
		}

		public static void InitializeClipboardSetting(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeClipboardSetting.Correction(setting, previousVersion, nonProcess);

			setting.WaitTime = Constants.clipboardWaitTime.GetClamp(setting.WaitTime);

			if (IsIllegalPlusNumber(setting.ItemsListWidth)) {
				setting.ItemsListWidth = Constants.clipboardItemsListWidth;
			}

			if (IsIllegalPlusNumber(setting.WindowWidth)) {
				setting.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
			}
			if (IsIllegalPlusNumber(setting.WindowHeight)) {
				setting.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
			}

		}

		public static void InitializeTemplateSetting(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeTemplateSetting.Correction(setting, previousVersion, nonProcess);

			if(IsIllegalPlusNumber(setting.ItemsListWidth)) {
				setting.ItemsListWidth = Constants.templateItemsListWidth;
			}
			if(IsIllegalPlusNumber(setting.ReplaceListWidth)) {
				setting.ReplaceListWidth = Constants.templateReplaceListWidth;
			}

			if (IsIllegalPlusNumber(setting.WindowWidth)) {
				setting.WindowWidth = Constants.templateDefaultWindowSize.Width;
			}
			if (IsIllegalPlusNumber(setting.WindowHeight)) {
				setting.WindowHeight = Constants.templateDefaultWindowSize.Height;
			}
		}

		public static void InitializeCommandSetting(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeCommandSetting.Correction(setting, previousVersion, nonProcess);

			setting.WindowWidth = Constants.commandWindowWidth.GetClamp(setting.WindowWidth);
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

			InitializeLoggingSetting(setting.Logging, previousVersion, nonProcess);
			InitializeToolbarSetting(setting.Toolbar, previousVersion, nonProcess);
			InitializeNoteSetting(setting.Note, previousVersion, nonProcess);
			InitializeWindowSaveSetting(setting.WindowSave, previousVersion, nonProcess);
			InitializeClipboardSetting(setting.Clipboard, previousVersion, nonProcess);
			InitializeTemplateSetting(setting.Template, previousVersion, nonProcess);
			InitializeCommandSetting(setting.Command, previousVersion, nonProcess);
		}

		public static void InitializeLoggingSetting(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeLoggingSetting.Correction(setting, previousVersion, nonProcess);

			if(IsIllegalPlusNumber(setting.WindowWidth)) {
				setting.WindowWidth = Constants.loggingDefaultWindowSize.Width;
			}
			if(IsIllegalPlusNumber(setting.WindowHeight)) {
				setting.WindowHeight = Constants.loggingDefaultWindowSize.Height;
			}
		}

		public static void InitializeLauncherItemSetting(LauncherItemSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeLauncherItemSetting.Correction(setting, previousVersion, nonProcess);

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
			Implement.InitializeLauncherGroupSetting.Correction(setting, previousVersion, nonProcess);

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
			Implement.InitializeNoteIndexSetting.Correction(setting, previousVersion, nonProcess);

			CheckUtility.EnforceNotNull(setting);
			foreach (var noteIndex in setting.Items) {
				InitializeNoteIndexItem(noteIndex, previousVersion, nonProcess);
			}
		}

		public static void InitializeNoteIndexItem(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeNoteIndexItem.Correction(indexItem, previousVersion, nonProcess);

			CheckUtility.EnforceNotNull(indexItem);
			if (IsIllegalPlusNumber(indexItem.Font.Size)) {
				indexItem.Font.Size = Constants.noteFontSize.median;
			}

			indexItem.Font.Size = Constants.noteFontSize.GetClamp(indexItem.Font.Size);
			
			if (string.IsNullOrWhiteSpace(indexItem.Font.Family)) {
				indexItem.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
			}

			if (IsIllegalPlusNumber(indexItem.WindowWidth)) {
				indexItem.WindowWidth = Constants.noteDefualtSize.Width;
			}
			if (IsIllegalPlusNumber(indexItem.WindowHeight)) {
				indexItem.WindowHeight = Constants.noteDefualtSize.Height;
			}

		}

		public static void InitializeTemplateIndexSetting(TemplateIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeTemplateIndexSetting.Correction(setting, previousVersion, nonProcess);
			CheckUtility.EnforceNotNull(setting);
		}

		public static void InitializeClipboardIndexSetting(ClipboardIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeClipboardIndexSetting.Correction(setting, previousVersion, nonProcess);
			CheckUtility.EnforceNotNull(setting);
		}

	}
}
