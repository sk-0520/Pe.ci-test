using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationUpdateScriptFactory
    {
        public ApplicationUpdateScriptFactory(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        public ILauncherExecutePathParameter CreateUpdateExecutePathParameter(FileInfo scriptSourceFIle, DirectoryInfo scriptDirectory, DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            var ps = "powershell";
            var psCommands = new[] {
                "-NoProfile",
                "-ExecutionPolicy", "Unrestricted",
                "-File", CommandLine.Escape(scriptSourceFIle.FullName),
                "-ProcessId", Process.GetCurrentProcess().Id.ToString(),
                "-WaitSeconds", TimeSpan.FromSeconds(5).TotalMilliseconds.ToString(),
                "-SourceDirectory", CommandLine.Escape(sourceDirectory.FullName),
                "-DestinationDirectory", CommandLine.Escape(destinationDirectory.FullName),
                "-CurrentVersion", BuildStatus.Version.ToString(),
                "-Platform", Environment.Is64BitProcess ? "x64": "x32",
                "-UpdateBeforeScript", CommandLine.Escape(Path.Combine(destinationDirectory.FullName, "etc", "script", "update", "update-new-before.ps1")),
                "-UpdateAfterScript", CommandLine.Escape(Path.Combine(destinationDirectory.FullName, "etc", "script", "update", "update-new-after.ps1")),
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
