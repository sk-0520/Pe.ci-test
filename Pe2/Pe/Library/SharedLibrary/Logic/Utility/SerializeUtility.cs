namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 設定の入出力。
	/// </summary>
	public static class SerializeUtility
	{
		/// <summary>
		/// DataContract属性を保持しているか。
		/// <para>http://stackoverflow.com/questions/221687/can-you-use-where-to-require-an-attribute-in-c</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		static bool HasDataContract<T>()
		{
			var results = typeof(T).GetCustomAttributes(typeof(DataContractAttribute), true);
			return results != null && results.Any();
		}

		/// <summary>
		/// Serializable属性を保持しているか。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		static bool HasSerializable<T>()
		{
			var results = typeof(T).GetCustomAttributes(typeof(SerializableAttribute), false);
			return results != null && results.Any();
		}

		/// <summary>
		/// ファイル入力用ストリームを作成。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		static FileStream CreateReadFileStream(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		/// <summary>
		/// ファイル出力用ストリームを作成。
		/// <para>親ディレクトリが必要なら勝手に作る。</para>
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		static FileStream CreateWriteFileStream(string path)
		{
			FileUtility.MakeFileParentDirectory(path);
			return new FileStream(path, FileMode.Create, FileAccess.Write);
		}

		/// <summary>
		/// XMLストリーム読み込み。
		/// <para>DataContractを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T LoadXmlDataFromStream<T>(Stream stream)
			where T: ModelBase, new()
		{
			if(!HasDataContract<T>()) {
				throw new InvalidOperationException(typeof(T).ToString());
			}

			using(var xmlReader = XmlReader.Create(stream)) {
				var serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(xmlReader);
			}
		}

		/// <summary>
		/// XMLファイル読み込み。
		/// <para>DataContractを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static T LoadXmlDataFromFile<T>(string filePath)
			where T: ModelBase, new()
		{
			using(var stream = CreateReadFileStream(filePath)) {
				return LoadXmlDataFromStream<T>(stream);
			}
		}

		/// <summary>
		/// XMLストリーム読み込み。
		/// <para>Serializableを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T LoadXmlSerializeFromStream<T>(Stream stream)
			where T: ModelBase, new()
		{
			if(!HasSerializable<T>()) {
				throw new InvalidOperationException(typeof(T).ToString());
			}

			using(var reader = new XmlTextReader(stream)) {
				var serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(reader);
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
			where T: ModelBase, new()
		{
			using(var stream = CreateReadFileStream(filePath)) {
				return LoadXmlSerializeFromStream<T>(stream);
			}
		}


		/// <summary>
		/// XMLストリーム書き出し。
		/// <para>DataContractを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="model"></param>
		public static void SaveXmlDataToStream<T>(Stream stream, T model)
			 where T: ModelBase
		{
			Debug.Assert(model != null);

			if(!HasDataContract<T>()) {
				throw new InvalidOperationException(typeof(T).ToString());
			}

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
		/// <para>DataContractを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <param name="model"></param>
		public static void SaveXmlDataToFile<T>(string filePath, T model)
			where T: ModelBase
		{
			using(var stream = CreateWriteFileStream(filePath)) {
				SaveXmlDataToStream(stream, model);
			}
		}

		/// <summary>
		/// XMLストリーム書き出し。
		/// <para>Serializableを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="model"></param>
		public static void SaveXmlSerializeToStream<T>(Stream stream, T model)
			where T: ModelBase
		{
			Debug.Assert(model != null);

			if(!HasSerializable<T>()) {
				throw new InvalidOperationException(typeof(T).ToString());
			}

			var serializer = new XmlSerializer(typeof(T));
			serializer.Serialize(stream, model);
		}

		/// <summary>
		/// XMLファイル書き出し。
		/// <para>Serializableを使用。</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <param name="model"></param>
		public static void SaveXmlSerializeToFile<T>(string filePath, T model)
			where T: ModelBase
		{
			using(var stream = CreateWriteFileStream(filePath)) {
				SaveXmlSerializeToStream(stream, model);
			}
		}
	}
}
