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
		}
	}
}
