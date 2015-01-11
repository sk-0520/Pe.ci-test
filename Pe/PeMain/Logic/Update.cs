using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	public class UpdateInfo
	{
		IEnumerable<string> _log;
		
		public UpdateInfo(IEnumerable<string> log)
		{
			this._log = log;
		}
		
		public string Version { get; set; }
		public bool IsUpdate { get; set; }
		public bool IsRcVersion { get; set; }
		public bool IsError { get; set; }
		public int ErrorCode { get; set; }
		
		public string Log
		{
			get
			{
				return string.Join(Environment.NewLine, this._log);
			}
		}
	}
	
	public class UpdateData
	{
		readonly string _downloadPath;
		readonly bool _donwloadRc;
		readonly CommonData _commonData;
		
		public UpdateInfo Info { get; private set; }
		
		public static string UpdaterExe
		{
			get { return Path.Combine(Literal.ApplicationSBinDirPath, Literal.updateProgramDir, Literal.updateProgramName); }
		}
		
		public UpdateData(string downloadPath, bool donwloadRc, CommonData commonData)
		{
			this._downloadPath = downloadPath;
			this._donwloadRc = donwloadRc;
			this._commonData = commonData;
		}
		
		Process CreateProcess(Dictionary<string,string> map)
		{
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = UpdaterExe;
			
			var defaultMap = new Dictionary<string,string>() {
				{ "pid",      string.Format("{0}", Process.GetCurrentProcess().Id) },
				{ "version",  Literal.Version.FileVersion },
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
			this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/check"], process.StartInfo.Arguments);
			
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
			
			var info = new UpdateInfo(lines);
			
			if(lines.Count > 0) {
				var s = lines.SingleOrDefault(line => !string.IsNullOrEmpty(line) && line.StartsWith(">> ", StringComparison.OrdinalIgnoreCase));
				
				var v = new string(s.SkipWhile(c => c != ':').Skip(1).ToArray());
				if(s.StartsWith(">> UPDATE", StringComparison.OrdinalIgnoreCase)) {
					var version = new string(v.TakeWhile(c => c != ' ').ToArray());
					var isRc = v.Substring(version.Length + 1) == "RC";
					info.IsUpdate = true;
					info.Version = version;
					info.IsRcVersion = isRc;
				} else if(string.IsNullOrEmpty(s) || !s.StartsWith(">> NONE", StringComparison.OrdinalIgnoreCase)) {
					int r;
					if(int.TryParse(v, out r)) {
						info.ErrorCode = r;
					} else {
						info.ErrorCode = -2;
					}
					info.IsError = true;
				}
			} else {
				info.ErrorCode = -1;
				info.IsError = true;
			}

			return Info = info;
		}
		
		public bool Execute()
		{
			var eventName = "pe-event";

			var lines = new List<string>();
			var map = new Dictionary<string,string>() {
				{ "download",       this._downloadPath },
				{ "expand",         Literal.ApplicationRootDirPath },
				{ "wait",           "true" },
				{ "no-wait-update", "true" },
				{ "event",           eventName },
			};
			FileUtility.MakeFileParentDirectory(this._downloadPath);
			if(!Directory.Exists(this._downloadPath)) {
				Directory.CreateDirectory(this._downloadPath);
			}

			//var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In);
			var waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);
			
			var process = CreateProcess(map);
			this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/exec"], process.StartInfo.Arguments);

			process.Start();
			//pipe.WaitForConnection();
			if(waitEvent.WaitOne(TimeSpan.FromMinutes(3))) {
				// 終了
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/exit"], process.StartInfo.Arguments);

				return true;
			}

			return false;
		}
	}
}
