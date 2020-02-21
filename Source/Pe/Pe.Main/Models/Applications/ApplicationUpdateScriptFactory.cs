using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationUpdateScriptFactory
    {
        public ApplicationUpdateScriptFactory(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        public ILauncherExecutePathParameter CreateUpdateExecutePathParameter(FileInfo scriptSourceFIle, DirectoryInfo scriptDirectory, DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
            var executeFiles = environmentExecuteFile.GetPathExecuteFiles();
            var pwsh = environmentExecuteFile.Get("pwsh", executeFiles);
            var powershell = environmentExecuteFile.Get("powershell", executeFiles);

            if(pwsh == null && powershell == null) {
                Logger.LogError("[pwsh] と [powershell] が見つかんないのでもぅﾏﾁﾞ無理");
                throw new Exception("[pwsh] and [powershell] is null");
            }

            var scriptDirPath = Path.Combine(destinationDirectory.FullName, "etc", "script", "update");

            var ps = pwsh?.File.FullName ?? powershell!.File.FullName;
            var psCommands = new[] {
                "-NoProfile",
                "-ExecutionPolicy", "Unrestricted",
                "-File", CommandLine.Escape(scriptSourceFIle.FullName),
                "-ProcessId", Process.GetCurrentProcess().Id.ToString(),
                "-WaitSeconds", TimeSpan.FromSeconds(5).TotalMilliseconds.ToString(),
                "-SourceDirectory", CommandLine.Escape(sourceDirectory.FullName),
                "-DestinationDirectory", CommandLine.Escape(destinationDirectory.FullName),
                "-CurrentVersion", BuildStatus.Version.ToString(),
                "-Platform", ProcessArchitecture.ApplicationArchitecture,
                "-UpdateBeforeScript", CommandLine.Escape(Path.Combine(scriptDirPath, "update-new-before.ps1")),
                "-UpdateAfterScript", CommandLine.Escape(Path.Combine(scriptDirPath, "update-new-after.ps1")),
                "-ExecuteCommand", CommandLine.Escape(EnvironmentParameters.RootApplication.FullName),
                "-ExecuteArgument", CommandLine.Escape(string.Join(" ", Environment.GetCommandLineArgs().Skip(1).Select(i => CommandLine.Escape(i)))),
            };
            var psCommand = string.Join(" ", psCommands);

            var executePathParameter = new LauncherExecutePathParameter(ps, psCommand, scriptDirectory.FullName);
            return executePathParameter;
        }

        #endregion
    }
}
