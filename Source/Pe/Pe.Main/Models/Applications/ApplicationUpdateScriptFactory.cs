using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal class ApplicationUpdateScriptFactory
    {
        public ApplicationUpdateScriptFactory(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private EnvironmentParameters EnvironmentParameters { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        public ILauncherExecutePathParameter CreateUpdateExecutePathParameter(FileInfo scriptSourceFIle, DirectoryInfo scriptDirectory, DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);

            var powerShellArguments = new PowerShellArguments();
            var psResult = powerShellArguments.GetPowerShellFromCommandName(environmentExecuteFile);
            if(!psResult.Success) {
                Logger.LogError("PowerShell が見つかんないのでもぅﾏﾁﾞ無理");
                throw new Exception("PowerShell が見つからない");
            }
            var ps = psResult.SuccessValue!;

            var scriptDirPath = Path.Combine(sourceDirectory.FullName, "etc", "script", "update");

            var psCommands = powerShellArguments.CreateParameters(true, new[] {
               KeyValuePair.Create( "-File", scriptSourceFIle.FullName),
               KeyValuePair.Create( "-LogPath", EnvironmentParameters.TemporaryUpdateLogFile.FullName),
               KeyValuePair.Create( "-ProcessId", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture)),
               KeyValuePair.Create( "-WaitSeconds", TimeSpan.FromSeconds(5).TotalMilliseconds.ToString(CultureInfo.InvariantCulture)),
               KeyValuePair.Create( "-SourceDirectory", sourceDirectory.FullName),
               KeyValuePair.Create( "-DestinationDirectory", destinationDirectory.FullName),
               KeyValuePair.Create( "-CurrentVersion", BuildStatus.Version.ToString()),
               KeyValuePair.Create( "-Platform", ProcessArchitecture.ApplicationArchitecture),
               KeyValuePair.Create( "-UpdateBeforeScript", Path.Combine(scriptDirPath, "update-new-before.ps1")),
               KeyValuePair.Create( "-UpdateAfterScript", Path.Combine(scriptDirPath, "update-new-after.ps1")),
               KeyValuePair.Create( "-ExecuteCommand", EnvironmentParameters.RootApplication.FullName),
            });
            psCommands.AddRange(powerShellArguments.ConvertOptions());

            var psCommand = string.Join(" ", psCommands);

            var executePathParameter = new LauncherExecutePathParameter(ps, psCommand, scriptDirectory.FullName);
            return executePathParameter;
        }

        #endregion
    }
}
