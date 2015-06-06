namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text.RegularExpressions;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

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

			using(var stream = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
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
			return File.Exists(path) || Directory.Exists(path);
		}

		/// <summary>
		/// 指定条件で不要ファイルを削除。
		/// </summary>
		/// <param name="baseDirPath">対象ファイル群のディレクトリ。</param>
		/// <param name="targetWildcard">対象ファイル群をワイルドカードで指定。</param>
		/// <param name="orderBy">リストアップしたファイル群のソート順。真で昇順。</param>
		/// <param name="enableCount">リストアップしたファイル群の上位から残すファイル数。</param>
		/// <param name="catchException">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
		/// <returns>削除ファイル数。baseDirPathが存在しない場合は -1。</returns>
		public static int RotateFiles(string baseDirPath, string targetWildcard, bool orderByAsc, int enableCount, Func<Exception, bool> catchException)
		{
			if (Directory.Exists(baseDirPath)) {
				var archiveList = Directory.EnumerateFiles(baseDirPath, targetWildcard)
					.Where(File.Exists)
					.IfOrderByAsc(p => Path.GetFileName(p), orderByAsc)
					.Skip(enableCount - 1)
				;

				var removeCount = 0;
				foreach (var path in archiveList) {
					try {
						File.Delete(path);
						removeCount += 1;
					} catch (Exception ex) {
						if (!catchException(ex)) {
							break;
						}
					}
				}
				return removeCount;
			} else {
				return -1;
			}
		}

		static ZipArchiveEntry WriteArchive(ZipArchive archive, string path, string baseDirPath)
		{
			var entryPath = path.Substring(baseDirPath.Length);
			while (entryPath.First() == Path.DirectorySeparatorChar) {
				entryPath = entryPath.Substring(1);
			}

			var entry = archive.CreateEntry(entryPath);

			using (var entryStream = new BinaryWriter(entry.Open())) {
				var buffer = ToBinary(path);
				entryStream.Write(buffer);
			}

			return entry;
		}

		/// <summary>
		/// 指定パスにZIP形式でアーカイブを作成。
		/// </summary>
		/// <param name="saveFilePath">保存先パス。</param>
		/// <param name="basePath">基準とするディレクトリパス。</param>
		/// <param name="targetFiles">取り込み対象パス。ディレクトリを指定した場合は、以下のファイル・ディレクトリをすべてその対象とする</param>
		public static void CreateZipFile(string saveFilePath, string basePath, IEnumerable<string> targetFiles)
		{
			using (var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
				foreach (var filePath in targetFiles) {
					if (File.Exists(filePath)) {
						WriteArchive(zip, filePath, basePath);
					} else if (Directory.Exists(filePath)) {
						var list = Directory.EnumerateFiles(filePath, "*", SearchOption.AllDirectories);
						foreach (var f in list) {
							WriteArchive(zip, f, basePath);
						}
					}
				}
			}
		}


	}
}
