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
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
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

		internal static bool IsIllegalPlusNumber(double number)
		{
			return double.IsNaN(number) || number <= 0;
		}

		internal static bool IsIllegalPlusNumber(int number)
		{
			return number <= 0;
		}

		internal static bool IsIllegalString(string s)
		{
			return s == null;
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

		#region create

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

		public static LauncherItemModel CreateLauncherItem(LauncherItemCollectionModel items, INonProcess nonProcess)
		{
			var result = CreateModelName(items, nonProcess.Language, "new/launcher-name");
			InitializeLauncherItem(result, null, nonProcess);
			return result;
		}

		public static LauncherGroupItemModel CreateLauncherGroup(LauncherGroupItemCollectionModel group, INonProcess nonProcess)
		{
			var result = CreateModelName(group, nonProcess.Language, "new/group-name");
			InitializeLauncherGroupItem(result, null, nonProcess);
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

		#endregion

		#region increment

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

		/// <summary>
		/// リスト構造の整理。
		/// </summary>
		/// <param name="list"></param>
		/// <param name="value"></param>
		static void IncrementList(CollectionModel<string> list, string value)
		{
			if(!string.IsNullOrEmpty(value)) {
				var index = list.FindIndex(s => s == value);
				if(index != -1) {
					list.RemoveAt(index);
				}
				list.Insert(0, value);
			}
		}

		public static void IncrementLauncherItem(LauncherItemModel launcherItem, string option, string workDirPath, INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(launcherItem);

			var dateTime = DateTime.Now;

			IncrementList(launcherItem.History.Options, option);
			IncrementList(launcherItem.History.WorkDirectoryPaths, workDirPath);
			
			launcherItem.History.ExecuteTimestamp = dateTime;
			launcherItem.History.ExecuteCount += 1;

			launcherItem.History.Update(dateTime);
		}

		#endregion

		#region initialize

		public static void InitializeLoggingSetting(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeLoggingSetting.Correction(setting, previousVersion, nonProcess);
		}

		private static void InitializeStreamSetting(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeStreamSetting.Correction(setting, previousVersion, nonProcess);
		}

		public static void InitializeToolbarSetting(ToolbarSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeToolbarSetting.Correction(setting, previousVersion, nonProcess);

			foreach(var toolbar in setting.Items) {
				InitializeToolbarItem(toolbar, previousVersion, nonProcess);
			}
		}

		public static void InitializeToolbarItem(ToolbarItemModel model, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeToolbarItem.Correction(model, previousVersion, nonProcess);
		}

		public static void InitializeWindowSaveSetting(WindowSaveSettingModel model, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeWindowSaveSetting.Correction(model, previousVersion, nonProcess);
		}

		public static void InitializeNoteSetting(NoteSettingModel model, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeNoteSetting.Correction(model, previousVersion, nonProcess);
		}

		public static void InitializeClipboardSetting(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeClipboardSetting.Correction(setting, previousVersion, nonProcess);
		}

		public static void InitializeTemplateSetting(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeTemplateSetting.Correction(setting, previousVersion, nonProcess);
		}

		public static void InitializeCommandSetting(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeCommandSetting.Correction(setting, previousVersion, nonProcess);
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
			InitializeStreamSetting(setting.Stream, previousVersion, nonProcess);
			InitializeToolbarSetting(setting.Toolbar, previousVersion, nonProcess);
			InitializeNoteSetting(setting.Note, previousVersion, nonProcess);
			InitializeWindowSaveSetting(setting.WindowSave, previousVersion, nonProcess);
			InitializeClipboardSetting(setting.Clipboard, previousVersion, nonProcess);
			InitializeTemplateSetting(setting.Template, previousVersion, nonProcess);
			InitializeCommandSetting(setting.Command, previousVersion, nonProcess);
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

			foreach(var item in setting.Items) {
				InitializeLauncherItem(item, previousVersion, nonProcess);
			}
		}

		public static void InitializeLauncherItem(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeLauncherItem.Correction(item, previousVersion, nonProcess);
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

			foreach(var item in setting.Groups) {
				InitializeLauncherGroupItem(item, previousVersion, nonProcess);
			}
		}

		public static void InitializeLauncherGroupItem(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeLauncherGroupItem.Correction(item, previousVersion, nonProcess);
		}

		public static void InitializeNoteIndexSetting(NoteIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeNoteIndexSetting.Correction(setting, previousVersion, nonProcess);

			CheckUtility.EnforceNotNull(setting);
			foreach(var noteIndex in setting.Items) {
				InitializeNoteIndexItem(noteIndex, previousVersion, nonProcess);
			}
		}

		public static void InitializeNoteIndexItem(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeNoteIndexItem.Correction(indexItem, previousVersion, nonProcess);
		}

		public static void InitializeTemplateIndexSetting(TemplateIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeTemplateIndexSetting.Correction(setting, previousVersion, nonProcess);
		}

		public static void InitializeClipboardIndexSetting(ClipboardIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			Implement.InitializeClipboardIndexSetting.Correction(setting, previousVersion, nonProcess);
		}

		#endregion
	}
}
