/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/21
 * 時刻: 23:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using IWshRuntimeLibrary;

namespace PeUtility
{
	/// <summary>
	/// Description of FileUtility.
	/// </summary>
	public static class FileUtility
	{
		/// <summary>
		/// ファイルをバイナリとして読み込む
		/// </summary>
		/// <param name="filePath">展開済みファイルパス</param>
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
		public static bool IsExists(string path)
		{
			return System.IO.File.Exists(path) || Directory.Exists(path);
		}
	}

	public class ShortcutFile: IWshShortcut
	{
		IWshShortcut _shortcut;
		
		public ShortcutFile(string path)
		{
			Load(path);
		}
		
		public string FullName
		{
			get { return this._shortcut.FullName; }
		}
		
		public string TargetPath
		{
			get
			{
				var path = this._shortcut.TargetPath;
				var expandPath = Environment.ExpandEnvironmentVariables(path);
				if(FileUtility.IsExists(expandPath)) {
					return expandPath;
				}
				var dirPath = Path.GetDirectoryName(expandPath);
				var x86pfPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
				var x64pfPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				var x86Index = dirPath.IndexOf(x86pfPath);
				var x64Index = dirPath.IndexOf(x64pfPath);
				
				if(x86Index == 0) {
					// x86指定の場合にx64を考慮(なんかへんだこれ)
					var relationPath = dirPath.Substring(x86pfPath.Length);
					var x64TargetPath = Path.Combine(x64pfPath, relationPath);
					if(FileUtility.IsExists(x64TargetPath)) {
						return x64TargetPath;
					}
				}
				
				return path;
			}
			set { this._shortcut.TargetPath = value; }
		}
		
		
		public string Arguments
		{
			get { return this._shortcut.Arguments; }
			set { this._shortcut.Arguments = value; }
		}
		
		public string Description
		{
			get { return this._shortcut.Description; }
			set { this._shortcut.Description = value; }
		}
		
		public string Hotkey
		{
			get { return this._shortcut.Hotkey; }
			set { this._shortcut.Hotkey = value; }
		}
		
		public string IconLocation {
			get { return this._shortcut.Hotkey; }
			set { this._shortcut.Hotkey = value; }
		}
		
		public string RelativePath {
			set { this._shortcut.RelativePath = value; }
		}
		
		public int WindowStyle {
			get { return this._shortcut.WindowStyle; }
			set { this._shortcut.WindowStyle = value; }
		}
		
		public string WorkingDirectory {
			get { return this._shortcut.WorkingDirectory; }
			set { this._shortcut.WorkingDirectory = value; }
		}
		
		public string IconPath { get; private set; }
		public int IconIndex { get; private set; }
		
		public void Load(string path)
		{
			IWshRuntimeLibrary.WshShell wshShell;
			if(Environment.Is64BitOperatingSystem) {
				wshShell = new IWshRuntimeLibrary.WshShell();
			} else {
				wshShell = new IWshRuntimeLibrary.WshShellClass();
			}
			this._shortcut = (IWshShortcut)wshShell.CreateShortcut(path);
			
			var iconPath = this._shortcut.IconLocation;
			var index = iconPath.LastIndexOf(',');
			if(index == -1) {
				IconPath = iconPath;
				IconIndex = 0;
			} else {
				IconPath = iconPath.Substring(0, index);
				IconIndex = int.Parse(iconPath.Substring(index + 1));
			}

		}
		
		public void Save()
		{
			throw new NotImplementedException();
		}
	}
}
