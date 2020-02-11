using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
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
            var scriptFilePath = Path.Combine(scriptDirectory.FullName, scriptSourceFIle.Name);

            scriptSourceFIle.CopyTo(scriptFilePath, true);


            var ps = "powershell";
            var psCommands = new[] {
                "-NoProfile",
                "-ExecutionPolicy", "Unrestricted",
                "-File", CommandLine.Escape(scriptFilePath),
                "-ProcessId", Process.GetCurrentProcess().Id.ToString(),
                "-WaitSeconds", TimeSpan.FromSeconds(5).TotalMilliseconds.ToString(),
                "-SourceDirectory", CommandLine.Escape(sourceDirectory.FullName),
                "-DestinationDirectory", CommandLine.Escape(destinationDirectory.FullName),
                "-Platform", Environment.Is64BitProcess ? "x64": "x32",
                "-UpdateScript", CommandLine.Escape(Path.Combine(destinationDirectory.FullName, "etc", "updated.ps1")),
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
