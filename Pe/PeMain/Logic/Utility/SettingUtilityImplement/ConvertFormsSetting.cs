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
	//using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

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

		private static void ConvertSystemEnvironmentSetting(SystemEnvironmentSettingModel dstSetting, Data.SystemEnvironmentSetting srcSetting, INonProcess nonProcess)
		{
			dstSetting.ExtensionHotkey.Key = FormsConverter.GetKey(srcSetting.ExtensionShowHotKey);
			dstSetting.ExtensionHotkey.ModifierKeys = FormsConverter.GetModifierKeys(srcSetting.ExtensionShowHotKey);

			dstSetting.HideFileHotkey.Key = FormsConverter.GetKey(srcSetting.HiddenFileShowHotKey);
			dstSetting.HideFileHotkey.ModifierKeys = FormsConverter.GetModifierKeys(srcSetting.HiddenFileShowHotKey);
		}

		static void ConvertMainSetting(MainSettingModel dstMainSetting, Data.MainSetting srcMainSetting, INonProcess nonProcess)
		{
			ConvertRunningSetting(dstMainSetting.RunningInformation, srcMainSetting.Running, nonProcess);
			ConvertLoggingSetting(dstMainSetting.Logging, srcMainSetting.Log, nonProcess);
			ConvertSystemEnvironmentSetting(dstMainSetting.SystemEnvironment, srcMainSetting.SystemEnvironment, nonProcess);
		}

	}
}
