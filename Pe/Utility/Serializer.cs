using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	public static class Serializer
	{
		
		/// <summary>
		/// デシリアイズ。
		/// </summary>
		/// <param name="path">読み込むファイルパス</param>
		/// <param name="failToNew">読み込み失敗時にデフォルトコンストラクタで生成するか</param>
		/// <returns>読み込んだデータ</returns>
		public static T LoadFile<T>(string path, bool failToNew)
			where T: new()
		{
			if(File.Exists(path)) {
				var serializer = new XmlSerializer(typeof(T));
				using(var stream = new FileStream(path, FileMode.Open)) {
					return (T)serializer.Deserialize(stream);
				}
			}
			if(failToNew) {
				return new T();
			} else {
				return default(T);
			}
		}
		
		public static T LoadString<T>(string buffer)
		{
			var serializer = new XmlSerializer(typeof(T));
			using(var stream = new MemoryStream(Encoding.Unicode.GetBytes(buffer))) {
				return (T)serializer.Deserialize(stream);
			}
		}

		/// <summary>
		/// シリアライズ。
		/// </summary>
		/// <param name="saveData">保存データ</param>
		/// <param name="savePath">保存ファイルパス</param>
		public static void SaveFile<T>(T saveData, string savePath)
		{
			Debug.Assert(saveData != null);
			FileUtility.MakeFileParentDirectory(savePath);

			using(var stream = new FileStream(savePath, FileMode.Create)) {
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stream, saveData);
			}
		}
	}
}
