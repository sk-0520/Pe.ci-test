#define NOT_DOWNLOAD
#define NOT_EXPAND

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Library.Utility;
using Microsoft.CSharp;

namespace ContentTypeTextNet.Pe.Applications.Updater
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
		const string scriptFileName = "UpdaterScript.cs";

		private CommandLine _commandLine;
		
		Value<int> _pid = new Value<int>();
		
		Value<Tuple<ushort, ushort, ushort>> _version = new Value<Tuple<ushort, ushort, ushort>>();
		
		Value<string> _uri = new Value<string>();
		Value<string> _downloadDir = new Value<string>();
		Value<string> _expandDir = new Value<string>();
		Value<string> _platform = new Value<string>();
		Value<bool> _getRC = new Value<bool>();
		Value<bool> _checkOnly = new Value<bool>();
		Value<bool> _wait = new Value<bool>();
		Value<bool> _noWaitUpdate = new Value<bool>();
		Value<string> _eventName = new Value<string>();
		
		public bool CheckOnly { get { return this._checkOnly.Data; } }
		public bool Wait { get { return this._wait.Data; } }
		public bool WaitSkip { get; private set; }
		
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
				//var v = s.Split('.').Select(n => ushort.Parse(n)).ToArray();
				//value.Data = new Tuple<ushort, ushort, ushort>(v[0], v[1], v[2]);
				value.Data = Functions.ConvertVersionTuple(s);
			});
			Set("uri", this._uri);
			Set("download", this._downloadDir);
			Set("expand", this._expandDir);
			Set("platform", this._platform);
			Set("rc", this._getRC);
			Set("checkonly", this._checkOnly);
			Set("wait", this._wait);
			Set("no-wait-update", this._noWaitUpdate);
			Set("event", this._eventName);
		}

		void OutputErrorMessage(string s)
		{
			ChangeTempColor(s, ConsoleColor.Black, ConsoleColor.Red);
		}

		void OutputErrorMessage(Exception ex)
		{
			ChangeTempColor(ex.Message, ConsoleColor.Black, ConsoleColor.Red);
			ChangeTempColor(ex.StackTrace, ConsoleColor.Black, ConsoleColor.DarkRed);
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
					OutputErrorMessage(ex);
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
					OutputErrorMessage(ex);
				}
			}
		}
		
		public void Check()
		{
			XElement xml = null;
			using(var web = new WebClient()) {
				xml = XElement.Parse(web.DownloadString(this._uri.Data));
			}
			Console.WriteLine(xml.ToString());
			
			var items = xml
				.Elements()
				.Select(
					x => new {
						Version = Functions.ConvertVersionTuple(x.Attribute("version").Value),
						IsRC    = x.Attribute("type").Value == "rc",
						ArchiveElements = x.Elements(),
					}
				)
				.OrderByDescending(x => x.Version.Item1)
				.ThenByDescending(x => x.Version.Item2)
				.ThenByDescending(x => x.Version.Item3)
				.Where(x => Functions.VersionCheck(x.Version, this._version.Data) > 0)
				;
			
			foreach(var item in items) {
				if(item.IsRC && !this._getRC.Data) {
					continue;
				}
				foreach(var archive in item.ArchiveElements) {
					if(archive.Attribute("platform").Value == this._platform.Data) {
						IsRCVersion = item.IsRC;
						IsVersionUp = true;
						VersionText = string.Format("{0}.{1}.{2}", item.Version.Item1, item.Version.Item2, item.Version.Item3);
						DownloadFileUrl = archive.Attribute("uri").Value;
						break;
					}
				}
				if(IsVersionUp) {
					break;
				}
			}
		}

		void ChangeTempColor(string s, ConsoleColor fore, ConsoleColor back)
		{
			var tempFore = Console.ForegroundColor;
			var tempBack = Console.BackgroundColor;

			Console.ForegroundColor = fore;
			Console.BackgroundColor = back;
			Console.WriteLine(s);

			Console.ForegroundColor = tempFore;
			Console.BackgroundColor = tempBack;
		}
		
		void KillProcess(Process process)
		{
			EventWaitHandle eventHandle = null;
			if(this._eventName.HasValue) {
				eventHandle = EventWaitHandle.OpenExisting(this._eventName.Data);
				ChangeTempColor(string.Format("PID = {0}, Event = {1}, kill wait...", this._pid.Data, this._eventName.Data), ConsoleColor.Black, ConsoleColor.Green);
			} else {
				ChangeTempColor(string.Format("PID = {0}, kill wait...", this._pid.Data), ConsoleColor.Black, ConsoleColor.Yellow);
			}
			if(eventHandle != null) {
				Console.WriteLine("event set");
				eventHandle.Set();
			} else {
				Console.WriteLine("process kill");
				process.Kill();
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
				process = Process.GetProcessById(this._pid.Data);
				restartExe = "\"" + process.MainModule.FileName + "\"";
				restartArg = process.StartInfo.Arguments;
				process.Exited += (object sender, EventArgs e) => {
					processSw.Stop();
				};
				processSw.Start();
				KillProcess(process);
			}

			var downloadPath = Path.Combine(this._downloadDir.Data, DownloadFileUrl.Split('/').Last());
#if !NOT_DOWNLOAD
			using(var web = new WebClient()) {
				Console.WriteLine("Download = {0} -> {1}", DownloadFileUrl, downloadPath);
				var downloadSw = new Stopwatch();
				downloadSw.Start();
				web.DownloadFile(DownloadFileUrl, downloadPath);
				downloadSw.Stop();
				Console.WriteLine("Download -> Size: {0} byte, Time = {1}", (new FileInfo(downloadPath)).Length, downloadSw.Elapsed);
			}
#endif

			if(process != null) {
				isRestart = process.WaitForExit((int)(TimeSpan.FromMinutes(1).TotalMilliseconds));
				if(isRestart && !process.HasExited) {
					KillProcess(process);
				}
				Console.WriteLine("Kill -> {0}, Time = {1}", isRestart, processSw.Elapsed);
			}
			
			// 自身の名前を切り替え
			var myPath = Assembly.GetEntryAssembly().Location;
			var myDir = Path.GetDirectoryName(myPath);
#if !NOT_EXPAND
			var renamePath = Path.ChangeExtension(myPath, "update-old");
			if(File.Exists(renamePath)) {
				File.Delete(renamePath);
			}
#endif
			try {
#if !NOT_EXPAND
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
#endif
				// スクリプト実行
				ExecuteScript(Path.Combine(myDir, scriptFileName), this._expandDir.Data, this._platform.Data);

				if(isRestart) {
					Console.WriteLine("Exe -> {0}, Arg -> {1}", restartExe, restartArg);
					Process.Start(restartExe, restartArg);
				}
				if(_noWaitUpdate.Data) {
					WaitSkip = true;
				}
			} catch(Exception) {
#if !NOT_EXPAND
				File.Move(renamePath, myPath);
#endif
				throw;
			}
		}

		void ExecuteScript(string scriptFilePath, string baseDirectoryPath, string platform)
		{
			if(!File.Exists(scriptFilePath)) {
				Console.WriteLine("not found script file");
				return;
			}

			ChangeTempColor(string.Format("Execute Script: {0}", scriptFilePath), ConsoleColor.Cyan, ConsoleColor.Black);

			using(var compiler= new CSharpCodeProvider(new Dictionary<string, string>() { 
				{"CompilerVersion", "v4.0" } 
			})) {
				var parameters = new CompilerParameters();
				parameters.GenerateExecutable = false;
				parameters.GenerateInMemory = true;
				parameters.IncludeDebugInformation = true;
				parameters.TreatWarningsAsErrors = true;
				parameters.WarningLevel = 4;

				var cr = compiler.CompileAssemblyFromFile(parameters, scriptFilePath);
				var indent = "    ";
#if DEBUG
				if(cr.Output.Count > 0) {
					ChangeTempColor("Output:", ConsoleColor.DarkGreen, ConsoleColor.Black);
					foreach(var output in cr.Output) {
						ChangeTempColor(indent + output.ToString(), ConsoleColor.DarkGreen, ConsoleColor.Black);
					}
				}
#endif
				if(cr.Errors.Count > 0) {
					ChangeTempColor("Error:", ConsoleColor.DarkRed, ConsoleColor.Black);
					foreach(var error in cr.Errors) {
						ChangeTempColor(indent + error.ToString(), ConsoleColor.DarkRed, ConsoleColor.Black);
					}
					throw new UpdaterException(UpdaterCode.ScriptCompile);
				}

				var us = cr.CompiledAssembly.CreateInstance("UpdaterScript");
				us.GetType().GetMethod("Main").Invoke(us, new object [] { new [] { 
					scriptFilePath,
					baseDirectoryPath,
					platform
				}});
			}
		}
	}
}
