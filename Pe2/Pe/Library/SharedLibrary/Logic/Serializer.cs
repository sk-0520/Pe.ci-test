namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 設定の入出力。
	/// </summary>
	public static class Serializer
	{
		/// <summary>
		/// XMLストリーム読み込み。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T LoadXml<T>(Stream stream)
			where T: ModelBase, new()
		{
			using(var xmlReader = XmlReader.Create(stream)) {
				var serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(xmlReader);
			}
		}

		/// <summary>
		/// XMLファイル読み込み。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static T LoadXml<T>(string filePath)
			where T: ModelBase, new()
		{
			using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				return LoadXml<T>(stream);
			}
		}

		/// <summary>
		/// XMLストリーム書き出し。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="model"></param>
		public static void SaveXml<T>(Stream stream, T model)
			 where T: ModelBase
		{
			var xmlSetting = new XmlWriterSettings() {
				Encoding = new System.Text.UTF8Encoding(),
			};
			using(var xmlWriter = XmlWriter.Create(stream, xmlSetting)) {
				var serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(xmlWriter, model);
			}
		}
		/// <summary>
		/// XMLファイル書き出し。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <param name="model"></param>
		public static void SaveXml<T>(string filePath, T model)
			where T: ModelBase
		{
			using(var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
				SaveXml(stream, model);
			}

		}
	}
}
