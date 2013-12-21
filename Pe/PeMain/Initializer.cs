/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/21
 * 時刻: 23:40
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Xml.Serialization;

using PeMain.Setting;

namespace PeMain
{
	/// <summary>
	/// Description of Initializer.
	/// </summary>
	public static class Initializer
	{
		/// <summary>
		/// 本体設定ファイル初期化
		/// </summary>
		/// <param name="settingPath"></param>
		public static MainSetting GetMainSetting(string mainSettingPath)
		{
			if(File.Exists(mainSettingPath)) {
				var serializer = new XmlSerializer(typeof(MainSetting));
				using(var stream = new FileStream(mainSettingPath, FileMode.Open)) {
					return (MainSetting)serializer.Deserialize(stream);
				}
			}
			
			return new MainSetting();
		}
		/// <summary>
		/// 言語ファイル初期化
		/// </summary>
		/// <param name="languagePath"></param>
		public static Language GetLanguage(string languagePath)
		{
			if(File.Exists(languagePath)) {
				var serializer = new XmlSerializer(typeof(Language));
				using(var stream = new FileStream(languagePath, FileMode.Open)) {
					return (Language)serializer.Deserialize(stream);
				}
			}
			
			return null;
		}
	}
}
