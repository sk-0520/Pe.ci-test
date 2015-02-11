namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System.Diagnostics;
	using System.IO;
	using System.IO.Compression;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;

	/// <summary>
	/// シリアライズ・デシリアライズを行う。
	/// </summary>
	public static class Serializer
	{
		
		/// <summary>
		/// XMLデシリアイズ。
		/// </summary>
		/// <param name="path">読み込むファイルパス</param>
		/// <param name="failToNew">読み込み失敗時にデフォルトコンストラクタで生成するか</param>
		/// <returns>読み込んだデータ</returns>
		public static T LoadXmlFile<T>(string path, bool failToNew)
			where T: new()
		{
			if(File.Exists(path)) {
				var serializer = new XmlSerializer(typeof(T));
				using(var stream = new XmlTextReader(new FileStream(path, FileMode.Open))) {
					return (T)serializer.Deserialize(stream);
				}
			}
			if(failToNew) {
				return new T();
			} else {
				return default(T);
			}
		}
		
		/// <summary>
		/// XMLデシリアイズ。
		/// </summary>
		/// <typeparam name="T">型。</typeparam>
		/// <param name="buffer">XML文字列。</param>
		/// <returns></returns>
		public static T LoadXMLString<T>(string buffer)
		{
			var serializer = new XmlSerializer(typeof(T));
			using(var stream = new XmlTextReader(new MemoryStream(Encoding.Unicode.GetBytes(buffer)))) {
				return (T)serializer.Deserialize(stream);
			}
		}

		/// <summary>
		/// XMLシリアライズ。
		/// </summary>
		/// <param name="saveData">保存データ</param>
		/// <param name="savePath">保存ファイルパス</param>
		public static void SaveXmlFile<T>(T saveData, string savePath)
		{
			Debug.Assert(saveData != null);
			FileUtility.MakeFileParentDirectory(savePath);

			using(var stream = new FileStream(savePath, FileMode.Create)) {
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stream, saveData);
			}
		}

		public static T LoadCompressFile<T>(string loadPath, bool failToNew)
			where T: new()
		{
			if(File.Exists(loadPath)) {
				using(var loadStream = new FileStream(loadPath, FileMode.Open))
				using(var compressStream = new GZipStream(loadStream, CompressionMode.Decompress))
				using(var stream = new XmlTextReader(compressStream)) {
					var serializer = new XmlSerializer(typeof(T));
					return (T)serializer.Deserialize(stream);
				}
			}
			
			if(failToNew) {
				return new T();
			} else {
				return default(T);
			}
		}
		public static void SaveCompressFile<T>(T saveData, string savePath)
		{
			Debug.Assert(saveData != null);
			FileUtility.MakeFileParentDirectory(savePath);

			using(var saveStream = new FileStream(savePath, FileMode.Create))
			using(var compressStream = new GZipStream(saveStream, CompressionLevel.Fastest)) {
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(compressStream, saveData);
			}
		}
	}
}
