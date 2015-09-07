namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using Forms = ContentTypeTextNet.Pe.PeMain.Data;
	using Data = ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using System.Xml;
	using System.IO;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using System.Windows.Input;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Library.SharedLibrary.Model;
	using System.Windows;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	internal static class ConvertFormsSetting
	{
		#region compatible

		/// <summary>
		/// XMLストリーム読み込み。
		/// <para>Serializableを使用。</para>
		/// <para>http://stackoverflow.com/questions/2209443/c-sharp-xmlserializer-bindingfailure</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T LoadXmlSerializeFromStream<T>(Stream stream)
			where T: new()
		{
			using(var reader = new XmlTextReader(stream)) {
				//var serializer = new XmlSerializer(typeof(T));
				var serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
				var result = (T)serializer.Deserialize(reader);
				return result;
			}
		}

		/// <summary>
		/// XMLファイル読み込み。
		/// <para>Serializableを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T LoadXmlSerializeFromFile<T>(string filePath)
			where T: new()
		{
			using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				return LoadXmlSerializeFromStream<T>(stream);
			}
		}

		static void ConvertPairColor(ColorPairItemModel dst, Data.ColorPairItem src)
		{
			dst.BackColor = DrawingUtility.Convert(src.Back.Color);
			dst.ForeColor = DrawingUtility.Convert(src.Fore.Color);
		}

		static void ConvertFont(FontModel dst, Data.FontSetting src)
		{
			dst.Family = src.Family;
			dst.Bold = src.Bold;
			dst.Italic = src.Italic;
			dst.Size = DrawingUtility.ConvertFontSizeFromDrawing(src.Height);
		}

		static void ConvertHotKey(HotKeyModel dst, Data.HotKeySetting src)
		{
			dst.Key = FormsConverter.GetKey(src);
			dst.ModifierKeys = FormsConverter.GetModifierKeys(src);
		}

		static ContentTypeTextNet.Library.SharedLibrary.Define.IconScale ConvertIconScale(ContentTypeTextNet.Pe.Library.Skin.IconScale src)
		{
			var raw = (int)src;
			return (ContentTypeTextNet.Library.SharedLibrary.Define.IconScale)raw;
		}

		static LauncherItemModel ConvertLauncherItem(Data.LauncherItem srcItem, INonProcess nonProcess)
		{
			var dstItem = new LauncherItemModel();
			SettingUtility.InitializeLauncherItem(dstItem, null, nonProcess);

			var launcherTypeMap = new Dictionary<LauncherType, LauncherKind>() {
				{ LauncherType.File, LauncherKind.File },
				{ LauncherType.Directory, LauncherKind.File },
				{ LauncherType.Command, LauncherKind.Command },
			};

			dstItem.Name = srcItem.Name;
			LauncherKind outLauncherKind;
			if(launcherTypeMap.TryGetValue(srcItem.LauncherType, out outLauncherKind)) {
				if (srcItem.LauncherType == LauncherType.Directory) {
					var map = new Dictionary<string, string>() {
						{ LanguageKey.formsConvertLauncherItemName, srcItem.Name },
						{ LanguageKey.formsConvertLauncherItemType, srcItem.LauncherType.ToString() },
						{ LanguageKey.formsConvertLauncherItemTypeFile, LanguageUtility.GetTextFromEnum(outLauncherKind, nonProcess.Language) },
					};
					nonProcess.Logger.Warning(nonProcess.Language["forms-convert/launcher-item/LauncherType/dir-file-convert", map], srcItem);
				}
				dstItem.LauncherKind = outLauncherKind;
			} else {
				var map = new Dictionary<string, string>() {
					{ LanguageKey.formsConvertLauncherItemName, srcItem.Name },
					{ LanguageKey.formsConvertLauncherItemType, srcItem.LauncherType.ToString() },
				};
				nonProcess.Logger.Warning(nonProcess.Language["forms-convert/launcher-item/LauncherType/not-support", map], srcItem);
				return null;
			}

			return dstItem;
		}

		#endregion

		public static void Convert(ContentTypeTextNet.Pe.PeMain.Data.CommonData commonData)
		{
			var mainSettingFilePath = Environment.ExpandEnvironmentVariables(commonData.VariableConstants.FormsUserSettingMainSettinFilePath);
			var launcherItemsFilePath = Environment.ExpandEnvironmentVariables(commonData.VariableConstants.FormsUserSettingLauncherItemsSettinFilePath);
			var mainSetting = LoadXmlSerializeFromFile<Forms.MainSetting>(mainSettingFilePath);
			var launcherItems = LoadXmlSerializeFromFile<HashSet<Forms.LauncherItem>>(launcherItemsFilePath);

			commonData.MainSetting = new MainSettingModel();
			SettingUtility.InitializeMainSetting(commonData.MainSetting, null, commonData.NonProcess);
			ConvertMainSetting(commonData.MainSetting, mainSetting, commonData.NonProcess);
			ConvertLauncherItems(commonData.MainSetting.Toolbar, commonData.LauncherGroupSetting, commonData.LauncherItemSetting, mainSetting.Toolbar, launcherItems, commonData.NonProcess);
		}

		static void ConvertRunningSetting(RunningInformationSettingModel dstSetting, Data.RunningSetting srcSetting, INonProcess nonProcess)
		{
			dstSetting.Accept = srcSetting.Running;
			dstSetting.CheckUpdateRelease = srcSetting.CheckUpdate;
			dstSetting.CheckUpdateRC = srcSetting.CheckUpdateRC;
			dstSetting.LastExecuteVersion = new Version(srcSetting.VersionMajor, srcSetting.VersionMinor, srcSetting.VersionBuild, srcSetting.VersionRevision);
			dstSetting.ExecuteCount = srcSetting.ExecuteCount;
		}

		static void ConvertLoggingSetting(LoggingSettingModel dstSetting, Data.LogSetting srcSetting, INonProcess nonProcess)
		{
			dstSetting.AddShow = srcSetting.AddShow;
			dstSetting.ShowTriggerDebug = srcSetting.AddShowTrigger.HasFlag(LogType.Debug);
			dstSetting.ShowTriggerInformation = srcSetting.AddShowTrigger.HasFlag(LogType.Information);
			dstSetting.ShowTriggerWarning = srcSetting.AddShowTrigger.HasFlag(LogType.Warning);
			dstSetting.ShowTriggerError = srcSetting.AddShowTrigger.HasFlag(LogType.Error);
			dstSetting.IsVisible = srcSetting.Visible;
			dstSetting.WindowLeft = srcSetting.Point.X;
			dstSetting.WindowTop = srcSetting.Point.Y;
			dstSetting.WindowWidth = srcSetting.Size.Width;
			dstSetting.WindowHeight = srcSetting.Size.Height;
		}

		static void ConvertSystemEnvironmentSetting(SystemEnvironmentSettingModel dstSetting, Data.SystemEnvironmentSetting srcSetting, INonProcess nonProcess)
		{
			ConvertHotKey(dstSetting.ExtensionHotkey, srcSetting.ExtensionShowHotKey);
			ConvertHotKey(dstSetting.HideFileHotkey, srcSetting.HiddenFileShowHotKey);
		}

		static void ConvertWindowSaveSetting(WindowSaveSettingModel dstSetting, Data.MainSetting srcMainSetting, INonProcess nonProcess)
		{
			dstSetting.IsEnabled = srcMainSetting.WindowSaveCount > 0;
			dstSetting.SaveCount = srcMainSetting.WindowSaveCount;
			dstSetting.SaveIntervalTime = srcMainSetting.WindowSaveTime;
		}

		static void ConvertSteamSetting(StreamSettingModel dstSetting, Data.StreamSetting srcSetting, INonProcess nonProcess)
		{
			ConvertPairColor(dstSetting.OutputColor, srcSetting.GeneralColor);
			ConvertPairColor(dstSetting.ErrorColor, srcSetting.ErrorColor);
			ConvertFont(dstSetting.Font, srcSetting.FontSetting);
		}

		static void ConvertCommandSetting(CommandSettingModel dstSetting, Data.CommandSetting srcSetting, INonProcess nonProcess)
		{
			ConvertFont(dstSetting.Font, srcSetting.FontSetting);
			ConvertHotKey(dstSetting.ShowHotkey, srcSetting.HotKey);
			dstSetting.HideTime = srcSetting.HiddenTime;
			dstSetting.IconScale = ConvertIconScale(srcSetting.IconScale);
			dstSetting.WindowWidth = srcSetting.Width;
			dstSetting.FindFile = srcSetting.EnabledFindFile;
			dstSetting.FindTag = srcSetting.EnabledFindTag;
		}

		static void ConvertNoteSetting(NoteSettingModel dstSetting, Data.NoteSetting srcSetting, INonProcess nonProcess)
		{
			ConvertFont(dstSetting.Font, srcSetting.CaptionFontSetting);
			ConvertHotKey(dstSetting.CreateHotKey, srcSetting.CreateHotKey);
			ConvertHotKey(dstSetting.CompactHotKey, srcSetting.CompactHotKey);
			ConvertHotKey(dstSetting.HideHotKey, srcSetting.HiddenHotKey);
			ConvertHotKey(dstSetting.ShowFrontHotKey, srcSetting.ShowFrontHotKey);
		}

		static void ConvertClipboardSetting(ClipboardSettingModel dstSetting, Data.ClipboardSetting srcSetting, INonProcess nonProcess)
		{
			dstSetting.IsEnabled = srcSetting.Enabled;
			dstSetting.IsEnabledApplicationCopy = srcSetting.EnabledApplicationCopy;
			var captureMap = new Dictionary<Data.ClipboardType, ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType>() {
				{ Data.ClipboardType.Text, ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType.Text },
				{ Data.ClipboardType.Rtf, ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType.Rtf },
				{ Data.ClipboardType.Html, ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType.Html },
				{ Data.ClipboardType.Image, ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType.Image },
				{ Data.ClipboardType.File, ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType.Files },
			};
			dstSetting.CaptureType = captureMap
				.Where(p => srcSetting.EnabledTypes.HasFlag(p.Key))
				.Select(p => p.Value)
				.Aggregate(ContentTypeTextNet.Pe.Library.PeData.Define.ClipboardType.None, (a, b) => a | b)
			;
			dstSetting.DuplicationCount = srcSetting.ClipboardRepeated;
			dstSetting.SaveCount = srcSetting.Limit;
			dstSetting.WaitTime = srcSetting.WaitTime;
			dstSetting.UsingClipboard = srcSetting.OutputUsingClipboard;
			dstSetting.ItemsListWidth = srcSetting.StackListWidth;
			// ホットキーはクリップボードで使用してテンプレートは設定しない
			ConvertHotKey(dstSetting.ToggleHotKey, srcSetting.ToggleHotKeySetting);
			ConvertFont(dstSetting.Font, srcSetting.TextFont);
			dstSetting.IsVisible = srcSetting.Visible;
			dstSetting.IsTopmost = srcSetting.TopMost;
			dstSetting.WindowLeft = srcSetting.Location.X;
			dstSetting.WindowTop = srcSetting.Location.Y;
			dstSetting.WindowWidth= srcSetting.Size.Width;
			dstSetting.WindowHeight = srcSetting.Size.Height;
		}

		static void ConvertTemplateSetting(TemplateSettingModel dstSetting, Data.ClipboardSetting srcSetting, INonProcess nonProcess)
		{
			dstSetting.ItemsListWidth = srcSetting.StackListWidth;
			dstSetting.ReplaceListWidth = srcSetting.TemplateListWidth;
			ConvertFont(dstSetting.Font, srcSetting.TextFont);
			// 微妙にずらしとく
			dstSetting.WindowLeft = srcSetting.Location.X + SystemParameters.IconWidth;
			dstSetting.WindowTop = srcSetting.Location.Y + SystemParameters.IconHeight;
			dstSetting.WindowWidth = srcSetting.Size.Width;
			dstSetting.WindowHeight = srcSetting.Size.Height;
		}

		static void ConvertMainSetting(MainSettingModel dstMainSetting, Data.MainSetting srcMainSetting, INonProcess nonProcess)
		{
			dstMainSetting.Language.Name = srcMainSetting.LanguageName;
			ConvertRunningSetting(dstMainSetting.RunningInformation, srcMainSetting.Running, nonProcess);
			ConvertLoggingSetting(dstMainSetting.Logging, srcMainSetting.Log, nonProcess);
			ConvertSystemEnvironmentSetting(dstMainSetting.SystemEnvironment, srcMainSetting.SystemEnvironment, nonProcess);
			ConvertWindowSaveSetting(dstMainSetting.WindowSave, srcMainSetting, nonProcess);
			ConvertSteamSetting(dstMainSetting.Stream, srcMainSetting.Stream, nonProcess);
			ConvertCommandSetting(dstMainSetting.Command, srcMainSetting.Command, nonProcess);
			ConvertNoteSetting(dstMainSetting.Note, srcMainSetting.Note, nonProcess);
			ConvertClipboardSetting(dstMainSetting.Clipboard, srcMainSetting.Clipboard, nonProcess);
			ConvertTemplateSetting(dstMainSetting.Template, srcMainSetting.Clipboard, nonProcess);
		}


		/// <summary>
		/// ツールバー設定、ランチャーアイテム、グループのGuidがかかわる部分を変換。
		/// </summary>
		/// <param name="toolbarSetting"></param>
		/// <param name="launcherGroupSetting"></param>
		/// <param name="launcherItemSetting"></param>
		/// <param name="nonProcess"></param>
		static void ConvertLauncherItems(ToolbarSettingModel dstToolbarSetting, LauncherGroupSettingModel dstLauncherGroupSetting, LauncherItemSettingModel dstLauncherItemSetting, Data.ToolbarSetting srcToolbarSetting, HashSet<Data.LauncherItem> srcLauncherItems, INonProcess nonProcess)
		{
			// ランチャーアイテム変換とそのマッピング
			var items = new Dictionary<Data.LauncherItem, LauncherItemModel>();
			foreach (var srcItem in srcLauncherItems) {
				var dstItem = ConvertLauncherItem(srcItem, nonProcess);
				items[srcItem] = dstItem;
			}


			// グループ変換とそのマッピング
			var groups = new Dictionary<Data.ToolbarGroupItem, LauncherGroupItemModel>();

		}
	}
}
