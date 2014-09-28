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
		Value<bool> _wait = new Value<bool>();
		
		public bool CheckOnly { get { return this._checkOnly.Data; } }
		public bool Wait { get { return this._wait.Data; } }
		
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
			Set("wait", this._wait);
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
				var format = "{0:000}{1:000}{2:000}";
				var checkVer = int.Parse(string.Format(format, ver[0], ver[1], ver[2]));
				var nowVer = int.Parse(string.Format(format, this._version.Data.Item1, this._version.Data.Item2, this._version.Data.Item3));
				if(checkVer > nowVer) {
					foreach(XmlElement archive in item.GetElementsByTagName("archive")) {
						if(archive.GetAttribute("platform") == this._platform.Data) {
							IsRCVersion = type == "rc";
							IsVersionUp = true;
							VersionText = version;
							DownloadFileUrl = archive.GetAttribute("uri");
							break;
						}
					}
				}
				if(IsVersionUp) {
					break;
				}
			}
		}
		
		public void Execute()
		{
			// プロセスIDが渡されていた場合は閉じる
			var isRestart = false;
			var restartExe = string.Empty;
			var restartArg = string.Empty;
			
			Process process = null;
			var processSw = new Stopwatch();
			if(this._pid.HasValue) {
				try {
					process = Process.GetProcessById(this._pid.Data);
					restartExe = "\"" + process.MainModule.FileName + "\"";
					restartArg = process.StartInfo.Arguments;
					Console.WriteLine("PID = {0}, kill wait...", this._pid.Data);
					process.Exited += (object sender, EventArgs e) => {
						processSw.Stop();
					};
					processSw.Start();
					process.Kill();
				} catch(Exception) {
					// 握り潰し
				}
			}

			var downloadPath = Path.Combine(this._downloadDir.Data, DownloadFileUrl.Split('/').Last());
			using(var web = new WebClient()) {
				Console.WriteLine("Download = {0} -> {1}", DownloadFileUrl, downloadPath);
				var downloadSw = new Stopwatch();
				downloadSw.Start();
				web.DownloadFile(DownloadFileUrl, downloadPath);
				downloadSw.Stop();
				Console.WriteLine("Download -> Size: {0} byte, Time = {1}", (new FileInfo(downloadPath)).Length, downloadSw.Elapsed);
			}
			
			if(process != null) {
				isRestart = process.WaitForExit((int)(new TimeSpan(0, 1, 0).TotalMilliseconds));
				Console.WriteLine("Kill -> {0}, Time = {1}", isRestart, processSw.Elapsed);
			}
			
			// 自身の名前を切り替え
			var myPath = Assembly.GetEntryAssembly().Location;
			var renamePath = Path.ChangeExtension(myPath, "update-old");
			if(File.Exists(renamePath)) {
				File.Delete(renamePath);
			}
			try {
				Console.WriteLine("Rename -> {0} => {1}", myPath, renamePath);
				File.Move(myPath, renamePath);
				// 置き換え開始
				using(var archive = ZipFile.OpenRead(downloadPath)) {
					foreach(var entry in archive.Entries.Where(e => e.Name.Length > 0)) {
						var expandPath = Path.Combine(this._expandDir.Data, entry.FullName);
						var dirPath = Path.GetDirectoryName(expandPath);
						if(!Directory.Exists(dirPath)) {
							Directory.CreateDirectory(dirPath);
						}
						Console.WriteLine("Expand -> {0}", expandPath);
						entry.ExtractToFile(expandPath, true);
					}
				}
				if(isRestart) {
					Console.WriteLine("Exe -> {0}, Arg -> {1}", restartExe, restartArg);
					Process.Start(restartExe, restartArg);
				}
			} catch(Exception) {
				File.Move(renamePath, myPath);
				throw;
			}
		}
	}
}
