/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/21
 * 時刻: 23:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;

using IWshRuntimeLibrary;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// ファイル関連の共通処理。
	/// </summary>
	public static class FileUtility
	{
		/// <summary>
		/// ファイルをバイナリとして読み込む。
		/// 
		/// File.ReadAllBytes は開いているファイルを読めないのでこちらを使用する。
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <param name="startIndex">読み出し位置</param>
		/// <param name="readLength">読み出しサイズ</param>
		/// <returns></returns>
		public static byte[] ToBinary(string filePath, int startIndex, int readLength)
		{
			byte[] buffer;

			using (var stream = new BinaryReader(new FileStream(filePath,  FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
				buffer = new byte[readLength];
				stream.Read(buffer, startIndex, readLength);
			}

			return buffer;
		}
		/// <summary>
		/// ファイルをバイナリとして読み込む。
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <returns></returns>
		public static byte[] ToBinary(string filePath)
		{
			var fileInfo = new FileInfo(filePath);
			return ToBinary(filePath, 0, (int)fileInfo.Length);
		}
		
		/// <summary>
		/// ファイルパスを元にディレクトリを作成
		/// </summary>
		/// <param name="path">ファイルパス</param>
		/// <returns>ディレクトリパス</returns>
		public static string MakeFileParentDirectory(string path)
		{
			var dirPath = Path.GetDirectoryName(path);
			var dirInfo = Directory.CreateDirectory(dirPath);
			return dirInfo.FullName;
		}
		
		/// <summary>
		/// ファイル・ディレクトリ問わずに存在するか
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool Exists(string path)
		{
			return System.IO.File.Exists(path) || Directory.Exists(path);
		}
	}
}
