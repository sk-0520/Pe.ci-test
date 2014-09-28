/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 13:51
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using PeUtility;

namespace PeMain.Logic
{
	public class UpdateInfo
	{
		public string Version { get; set; }
		public bool IsUpdate { get; set; }
		public bool IsRcVersion { get; set; }
		public bool IsError { get; set; }
		public int ErrorCode { get; set; }
	}
	
	public class Update
	{
		readonly string _downloadPath;
		readonly bool _donwloadRc;
		
		const string updater = "PeUpdater.exe";
		public static string UpdaterExe
		{
			get { return Path.Combine(Literal.PeRootDirPath, updater); }
		}
		
		public Update(string downloadPath, bool donwloadRc)
		{
			this._downloadPath = downloadPath;
			this._donwloadRc = donwloadRc;
		}
		
		Process CreateProcess(Dictionary<string,string> map)
		{
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = UpdaterExe;
			
			var defaultMap = new Dictionary<string,string>() {
				{ "pid",      string.Format("{0}", Process.GetCurrentProcess().Id) },
				{ "version",  Literal.PeVersion },
				{ "uri",      Literal.UpdateURL },
				{ "platform", Environment.Is64BitProcess ? "x64": "x86" },
				{ "rc",       this._donwloadRc ? "true": "false" },
			};
			
			foreach(var pair in map) {
				defaultMap[pair.Key] = pair.Value;
			}
			startInfo.Arguments = string.Join(" ", defaultMap.Select(p => string.Format("\"/{0}={1}\"", p.Key, p.Value)));

			return process;
		}
		
		public UpdateInfo Check()
		{
			var lines = new List<string>();
			var map = new Dictionary<string,string>() {
				{ "checkonly", "true" }
			};
			var process = CreateProcess(map);
			
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;

			process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
				lines.Add(e.Data);
			};
			process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
				lines.Add(e.Data);
			};
			
			process.Start();
			
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			
			process.WaitForExit();
			
			var info = new UpdateInfo();
			
			if(lines.Count > 0) {
				var s = lines[0];
				
				var v = new string(s.SkipWhile(c => c != ':').Skip(1).ToArray());
				if(s.StartsWith(">> UPDATE")) {
					var version = new string(v.TakeWhile(c => c != ' ').ToArray());
					var isRc = v.Substring(version.Length + 1) == "RC";
					info.IsUpdate = true;
					info.Version = version;
					info.IsRcVersion = isRc;
				} else {
					info.ErrorCode = int.Parse(v);
					info.IsError = true;
				}
			} else {
				info.ErrorCode = -1;
				info.IsError = true;
			}

			return info;
		}
		
		public void Execute()
		{
			var lines = new List<string>();
			var map = new Dictionary<string,string>() {
				{ "download", this._downloadPath },
				{ "expand",   Literal.PeRootDirPath },
				{ "wait",     "true" },
			};
			FileUtility.MakeFileParentDirectory(this._downloadPath);
			if(!Directory.Exists(this._downloadPath)) {
				Directory.CreateDirectory(this._downloadPath);
			}
			
			var process = CreateProcess(map);
			
			process.Start();
		}
	}
}
