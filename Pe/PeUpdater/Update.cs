/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/25
 * 時刻: 23:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using PeUtility;

namespace PeUpdater
{
	class Value<T>
	{
		public bool HasValue { get; set; }
		public T Data { get; set; }
		public void Import(string s)
		{
			Data = (T)Convert.ChangeType(s, typeof(T));
		}
	}
	/// <summary>
	/// 更新処理もろもろ
	/// </summary>
	public class Update
	{
		private CommandLine _commandLine;
		
		Value<int> _pid = new Value<int>();
		
		Value<Tuple<ushort, ushort, ushort, ushort>> _version = new Value<Tuple<ushort, ushort, ushort, ushort>>();
		
		Value<string> _uri = new Value<string>();
		Value<string> _downloadDir = new Value<string>();
		Value<string> _expandDir = new Value<string>();
		Value<string> _platform = new Value<string>();
		Value<bool> _getRC = new Value<bool>();
		Value<bool> _checkOnly = new Value<bool>();
		
		public bool CheckOnly { get { return this._checkOnly.Data; } }
		
		public bool IsVersionUp { get; private set; }
		public bool IsRCVersion { get; private set; }
		public string VersionText { get; private set; }
		public string DownloadFileUrl { get; private set; }
		
		public Update(CommandLine commandLine)
		{
			Debug.Assert(commandLine.Length > 0);
			
			this._commandLine = commandLine;
			
			Set("pid", this._pid);
			Set("version", this._version, (value, s) => {
			    	var v = s.Split('.').Select(n => ushort.Parse(n)).ToArray();
			    	value.Data = new Tuple<ushort, ushort, ushort, ushort>(v[0], v[1], v[2], v[3]);
			    });
			Set("uri", this._uri);
			Set("download", this._downloadDir);
			Set("expand", this._expandDir);
			Set("platform", this._platform);
			Set("rc", this._getRC);
			Set("checkonly", this._checkOnly);
			
		}
		
		void Set<T>(string key, Value<T> value)
		{
			if(this._commandLine.HasOption(key)) {
				try {
					var s = this._commandLine.GetValue(key);
					if(s.Length > 0) {
						value.Import(s);
						value.HasValue = true;
					}
				} catch(Exception ex) {
					Console.WriteLine(ex);
				}
			}
		}
		
		void Set<T>(string key, Value<T> value, Action<Value<T>, string> custom)
		{
			if(this._commandLine.HasOption(key)) {
				try {
					var s = this._commandLine.GetValue(key);
					if(s.Length > 0) {
						custom(value, s);
						value.HasValue = true;
					}
				} catch(Exception ex) {
					Console.WriteLine(ex);
				}
			}
		}
		
		public void Check()
		{
			var xml = new XmlDocument();
			using(var web = new WebClient()) {
				xml.LoadXml(web.DownloadString(this._uri.Data));
			}
			var items = xml.DocumentElement.GetElementsByTagName("item");
			var downloadFileUrl = string.Empty;
			foreach(XmlElement item in items) {
				var version = item.GetAttribute("version");
				var type    = item.GetAttribute("type");
				if(type == "rc" && !this._getRC.Data) {
					continue;
				}
				var ver = version.Split('.').Select(n => ushort.Parse(n)).ToArray();
				var versionUp
					=  ver[0] > this._version.Data.Item1
					|| ver[1] > this._version.Data.Item2
					|| ver[2] > this._version.Data.Item3
					;
				foreach(XmlElement archive in item.GetElementsByTagName("archive")) {
					if(archive.GetAttribute("platform") == this._platform.Data) {
						IsRCVersion = type == "rc";
						IsVersionUp = true;
						VersionText = version;
						DownloadFileUrl = archive.GetAttribute("uri");
						break;
					}
				}
				if(IsVersionUp) {
					break;
				}
			}
		}
		
		public void Execute()
		{
			var downloadPath = Path.Combine(this._downloadDir.Data, DownloadFileUrl.Split('/').Last());
			using(var web = new WebClient()) {
				web.DownloadFile(DownloadFileUrl, downloadPath);
			}
			
			// 展開前
			// プロセスIDが渡されていた場合は閉じる
			var isRestart = false;
			var restartExe = string.Empty;
			var restartArg = string.Empty;
			if(this._pid.HasValue) {
				try {
					var process = Process.GetProcessById(this._pid.Data);
					restartExe = process.MainModule.FileName;
					restartArg = process.StartInfo.Arguments;
					process.Kill();
					isRestart = process.WaitForExit((int)(new TimeSpan(0, 0, 15).TotalMilliseconds));
				} catch(Exception) {
					// 握り潰し
				}
			}
			// 自身の名前を切り替え
			var myPath = Assembly.GetEntryAssembly().Location;
			var renamePath = Path.ChangeExtension(myPath, VersionText + ".update-old");
			if(File.Exists(renamePath)) {
				File.Delete(renamePath);
			}
			try {
				File.Move(myPath, renamePath);
				// 置き換え開始
				using(var archive = ZipFile.OpenRead(downloadPath)) {
					foreach(var entry in archive.Entries.Where(e => e.Name.Length > 0)) {
						var expandPath = Path.Combine(this._expandDir.Data, entry.FullName);
						var dirPath = Path.GetDirectoryName(expandPath);
						if(!Directory.Exists(dirPath)) {
							Directory.CreateDirectory(dirPath);
						}
						entry.ExtractToFile(expandPath, true);
					}
				}
				if(isRestart) {
					Process.Start(restartExe, restartArg);
				}
			} catch(Exception) {
				File.Move(renamePath, myPath);
				throw;
			}
		}
	}
}
