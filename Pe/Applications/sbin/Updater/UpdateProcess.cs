/*
This file is part of Updater.

Updater is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Updater is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Updater.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using Microsoft.CSharp;

//#define NO_DOWNLOAD
//#define NO_EXPAND

namespace ContentTypeTextNet.Pe.SystemApplications.Updater
{
    /// <summary>
    /// 更新処理もろもろ。
    /// </summary>
    public class UpdateProcess
    {
        const string scriptFileName = "UpdaterScript.cs";

        private CommandLine _commandLine;

        ArgumentValue<int> _pid = new ArgumentValue<int>();

        ArgumentValue<Tuple<ushort, ushort, ushort>> _version = new ArgumentValue<Tuple<ushort, ushort, ushort>>();

        ArgumentValue<string> _uri = new ArgumentValue<string>();
        ArgumentValue<string> _downloadDir = new ArgumentValue<string>();
        ArgumentValue<string> _expandDir = new ArgumentValue<string>();
        ArgumentValue<string> _platform = new ArgumentValue<string>();
        ArgumentValue<bool> _getRC = new ArgumentValue<bool>();
        ArgumentValue<bool> _checkOnly = new ArgumentValue<bool>();
        ArgumentValue<bool> _wait = new ArgumentValue<bool>();
        ArgumentValue<bool> _noWaitUpdate = new ArgumentValue<bool>();
        ArgumentValue<string> _eventName = new ArgumentValue<string>();
        ArgumentValue<string> _scriptPath = new ArgumentValue<string>();

        /// <summary>
        /// アップデートチェックのみを行うか。
        /// </summary>
        public bool CheckOnly { get { return this._checkOnly.Data; } }
        public bool Wait { get { return this._wait.Data; } }
        public bool WaitSkip { get; private set; }

        public bool IsVersionUp { get; private set; }
        public bool IsRCVersion { get; private set; }
        public string VersionText { get; private set; }
        public string DownloadFileUrl { get; private set; }

        public UpdateProcess(CommandLine commandLine)
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
            Set("script", this._scriptPath);
        }

        void OutputErrorMessage(string s)
        {
            ChangeTemporaryColor(s, ConsoleColor.Black, ConsoleColor.Red);
        }

        void OutputErrorMessage(Exception ex)
        {
            ChangeTemporaryColor(ex.Message, ConsoleColor.Black, ConsoleColor.Red);
            ChangeTemporaryColor(ex.StackTrace, ConsoleColor.Black, ConsoleColor.DarkRed);
        }

        void Set<T>(string key, ArgumentValue<T> value)
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

        void Set<T>(string key, ArgumentValue<T> value, Action<ArgumentValue<T>, string> custom)
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
                        IsRC = x.Attribute("type").Value == "rc",
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

        void ChangeTemporaryColor(string s, ConsoleColor fore, ConsoleColor back)
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
                ChangeTemporaryColor(string.Format("PID = {0}, Event = {1}, kill wait...", this._pid.Data, this._eventName.Data), ConsoleColor.Black, ConsoleColor.Green);
            } else {
                ChangeTemporaryColor(string.Format("PID = {0}, kill wait...", this._pid.Data), ConsoleColor.Black, ConsoleColor.Yellow);
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

            var downloadPath = Path.Combine(this._downloadDir.Data, DownloadFileUrl.Split('/').Last());
#if !NO_DOWNLOAD
            using(var web = new WebClient()) {
                Console.WriteLine("Download = {0} -> {1}", DownloadFileUrl, downloadPath);
                var downloadSw = new Stopwatch();
                downloadSw.Start();
                web.DownloadFile(DownloadFileUrl, downloadPath);
                downloadSw.Stop();
                Console.WriteLine("Download -> Size: {0} byte, Time = {1}", (new FileInfo(downloadPath)).Length, downloadSw.Elapsed);
            }
#else
#if BUILD
#error Deinfed BUILD!
#endif
#endif
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
#if !NO_EXPAND
            var renamePath = Path.ChangeExtension(myPath, "update-old");
            if(File.Exists(renamePath)) {
                File.Delete(renamePath);
            }
#else
#if BUILD
#error Deinfed BUILD!
#endif
#endif
            try {
#if !NO_EXPAND
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
#else
#if BUILD
#error Deinfed BUILD!
#endif
#endif
                // スクリプト実行
                var scriptPath = string.Empty;
                if(this._scriptPath.HasValue && File.Exists(this._scriptPath.Data)) {
                    Console.WriteLine("script: arg value");
                    scriptPath = this._scriptPath.Data;
                }
                if(string.IsNullOrEmpty(scriptPath)) {
                    Console.WriteLine("script: define value");
                    scriptPath = Path.Combine(myDir, scriptFileName);
                }
                ExecuteScript(scriptPath, this._expandDir.Data, this._platform.Data);

                if(isRestart) {
                    Console.WriteLine("Exe -> {0}, Arg -> {1}", restartExe, restartArg);
                    Process.Start(restartExe, restartArg);
                }
                if(_noWaitUpdate.Data) {
                    WaitSkip = true;
                }
            } catch(Exception) {
#if !NO_EXPAND
                File.Move(renamePath, myPath);
#else
#if BUILD
#error Deinfed BUILD!
#endif
#endif
                throw;
            }
        }

        void AppendAssembly(CompilerParameters parameters, string dllName)
        {
            if(!parameters.ReferencedAssemblies.Contains(dllName)) {
                parameters.ReferencedAssemblies.Add(dllName);
            } else {
                Console.WriteLine("Overlap: {0}", dllName);
            }
        }

        void ExecuteScript(string scriptFilePath, string baseDirectoryPath, string platform)
        {
            if(!File.Exists(scriptFilePath)) {
                Console.WriteLine("not found script file: " + scriptFilePath);
                return;
            }

            ChangeTemporaryColor(string.Format("Execute Script: {0}", scriptFilePath), ConsoleColor.Cyan, ConsoleColor.Black);

            using(var compiler = new CSharpCodeProvider(new Dictionary<string, string>() {
                {"CompilerVersion", "v4.0" }
            })) {
                //var scriptText = File.ReadAllText(scriptFilePath, Encoding.UTF8);
                var scriptText = File.ReadAllText(scriptFilePath);

                var parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.IncludeDebugInformation = true;
                parameters.TreatWarningsAsErrors = true;
                parameters.WarningLevel = 4;
                parameters.CompilerOptions = string.Format("/platform:{0}", platform);

                // 最低限のアセンブリは読み込ませる
                var asmList = new[] {
                    "mscorlib.dll",
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll"
                };
                foreach(var dllName in asmList) {
                    AppendAssembly(parameters, dllName);
                }

                // //+DLL:*.dll読み込み
                var regTargetDll = new Regex(@"^//\+DLL\s*:\s*(?<DLL>.*\.dll)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach(Match match in regTargetDll.Matches(scriptText)) {
                    var dllName = match.Groups["DLL"].Value;
                    AppendAssembly(parameters, dllName);
                }

                // /*-*/using xxx は読み込み無視
                var regUsingDll = new Regex(@"[^(/*-*/)]\s*using\s+(?<NAME>.+)\s*;", RegexOptions.Multiline);
                foreach(Match match in regUsingDll.Matches(scriptText)) {
                    var name = match.Groups["NAME"].Value;
                    if(name.Any(c => c == '=')) {
                        name = name.Split('=').Last().Trim();
                    }
                    var dllName = name + ".dll";
                    AppendAssembly(parameters, dllName);
                }
                foreach(var asm in parameters.ReferencedAssemblies) {
                    Console.WriteLine("Assembly = {0}", asm);
                }

                var cr = compiler.CompileAssemblyFromSource(parameters, scriptText);

                var indent = "    ";
#if DEBUG
                if(cr.Output.Count > 0) {
                    ChangeTemporaryColor("Output:", ConsoleColor.DarkGreen, ConsoleColor.Black);
                    foreach(var output in cr.Output) {
                        ChangeTemporaryColor(indent + output.ToString(), ConsoleColor.DarkGreen, ConsoleColor.Black);
                    }
                }
#endif
                if(cr.Errors.Count > 0) {
                    ChangeTemporaryColor("Error:", ConsoleColor.DarkRed, ConsoleColor.Black);
                    foreach(var error in cr.Errors) {
                        ChangeTemporaryColor(indent + error.ToString(), ConsoleColor.DarkRed, ConsoleColor.Black);
                    }
                    throw new UpdaterException(UpdaterCode.ScriptCompile);
                }

                var assembly = cr.CompiledAssembly;

                var us = assembly.CreateInstance("UpdaterScript");
                us.GetType().GetMethod("Main").Invoke(us, new object[] { new [] {
                    scriptFilePath,
                    baseDirectoryPath,
                    platform
                }});
            }
        }
    }

    /// <summary>
    /// 互換性用。
    /// <para>この処理もう消したいんだけど動いてるからさわりたくない。</para>
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// バージョン文字列をタプルに変換。
        /// 
        /// *.*.* までが対象となる。
        /// 
        /// <para>Versionでいいじゃん(いいじゃん)</para>
        /// </summary>
        /// <param name="versionText"></param>
        /// <returns></returns>
        public static Tuple<ushort, ushort, ushort> ConvertVersionTuple(string versionText)
        {
            var v = versionText
                .Split('.')
                .Take(3)
                .Select(n => ushort.Parse(n))
                .ToArray()
            ;
            if(v.Length < 3) {
                throw new ArgumentException(string.Format("src = {0}, split = {1}", versionText, v));
            }
            return new Tuple<ushort, ushort, ushort>(v[0], v[1], v[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// +: a &gt; b
        /// 0: a = b
        /// -: a &lt; b
        /// </returns>
        public static int VersionCheck(Tuple<ushort, ushort, ushort> a, Tuple<ushort, ushort, ushort> b)
        {
            const string format = "{0:D2}{1:D3}{2:D5}";
            var sa = string.Format(format, a.Item1, a.Item2, a.Item3);
            var sb = string.Format(format, b.Item1, b.Item2, b.Item3);
            var na = int.Parse(sa);
            var nb = int.Parse(sb);
            //Debug.WriteLine("{0}:{1}",sa, sb);
            return na - nb;
        }

    }
}
