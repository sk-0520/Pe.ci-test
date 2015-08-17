using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	public class UpdateData
	{
		readonly string _downloadPath;
		readonly bool _donwloadRc;
		readonly CommonData _commonData;

		public UpdateInfo Info { get; private set; }

		public static string UpdaterExe
		{
			get { return Path.Combine(Constants.sbinDirectoryName, Constants.updateProgramDirectoryName, Constants.updateProgramName); }
		}

		public UpdateData(string downloadPath, bool donwloadRc, CommonData commonData)
		{
			this._downloadPath = downloadPath;
			this._donwloadRc = donwloadRc;
			this._commonData = commonData;
		}

		#region property

		public VariableConstants VariableConstants { get { return _commonData.VariableConstants; } }

		#endregion

		Process CreateProcess(Dictionary<string, string> map)
		{
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = UpdaterExe;

			var defaultMap = new Dictionary<string, string>() {
				{ "pid",      string.Format("{0}", Process.GetCurrentProcess().Id) },
				{ "version",  Constants.assemblyVersion.ToString() },
				{ "uri",      Constants.UriUpdate },
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
			var map = new Dictionary<string, string>() {
				{ "checkonly", "true" }
			};
			using(var process = CreateProcess(map)) {

				this._commonData.Logger.Information(this._commonData.Language["log/update/check"], process.StartInfo.Arguments);

				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;

				process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
					lock(lines) {
						if(e.Data != null) {
							lines.Add(e.Data);
						}
					}
				};
				process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
					lock(lines) {
						if(e.Data != null) {
							lines.Add(e.Data);
						}
					}
				};

				process.Start();

				process.BeginOutputReadLine();
				process.BeginErrorReadLine();

				process.WaitForExit();
			}

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

		/// <summary>
		/// 更新処理実行。
		/// </summary>
		/// <returns></returns>
		public bool Execute()
		{
			var eventName = "pe-event";

			var lines = new List<string>();
			var map = new Dictionary<string, string>() {
				{ "download",       this._downloadPath },
				{ "expand",         Constants.applicationRootDirectoryPath },
				{ "wait",           "true" },
				{ "no-wait-update", "true" },
				{ "event",           eventName },
				{ "script",          Path.Combine(Constants.applicationRootDirectoryPath, Constants.scriptDirectoryName, "Updater", "UpdaterScript.cs") },
			};
			FileUtility.MakeFileParentDirectory(this._downloadPath);
			if(!Directory.Exists(this._downloadPath)) {
				Directory.CreateDirectory(this._downloadPath);
			}
			// #158
			FileUtility.RotateFiles(this._downloadPath, "*.zip", ContentTypeTextNet.Library.SharedLibrary.Define.OrderBy.Desc, Constants.updateArchiveCount, e => {
				this._commonData.Logger.Warning(e);
				return true;
			});

			//var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In);
			var waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);

			using(var process = CreateProcess(map)) {
				//this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/exec"], process.StartInfo.Arguments);
				this._commonData.Logger.Information(this._commonData.Language["log/update/exec"], process.StartInfo.Arguments);

				var result = false;

				process.Start();
				var processEvent = new EventWaitHandle(false, EventResetMode.AutoReset) {
					SafeWaitHandle = new SafeWaitHandle(process.Handle, false),
				};
				var handles = new[] { waitEvent, processEvent };
				var waitResult = WaitHandle.WaitAny(handles, TimeSpan.FromMinutes(3));
				this._commonData.Logger.Debug("WaitHandle.WaitAny", waitResult);
				if(0 <= waitResult && waitResult < handles.Length) {
					if(handles[waitResult] == waitEvent) {
						// イベントが立てられたので終了
						this._commonData.Logger.Information(this._commonData.Language["log/update/exit"], process.StartInfo.Arguments);
						result = true;
					} else if(handles[waitResult] == processEvent) {
						// Updaterがイベント立てる前に死んだ
						this._commonData.Logger.Information(this._commonData.Language["log/update/error-process"], process.ExitCode);
					}
				} else {
					// タイムアウト
					if(!process.HasExited) {
						// まだ生きてるなら強制的に殺す
						process.Kill();
					}
					this._commonData.Logger.Information(this._commonData.Language["log/update/error-timeout"], process.ExitCode);
				}

				return result;
			}
		}


	}
}
