using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationInitializer
    {
        #region function

        void SetEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("PE_DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        CommandLine CreateCommandLine(IEnumerable<string> arguments)
        {
            var commandLine = new CommandLine(arguments, false);

            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyUserDirectory, hasValue: true);
            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyMachineDirectory, hasValue: true);

            commandLine.Execute();

            return commandLine;
        }

        void InitializeEnvironment(IEnumerable<string> arguments)
        {
            SetEnvironmentVariable();

            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var commandLine = CreateCommandLine(arguments);

            Debug.Assert(commandLine.IsParsed);
            EnvironmentParameters.Initialize(new DirectoryInfo(applicationDirectory), commandLine);
        }

        bool IsFirstStartup()
        {
            var file = EnvironmentParameters.Current.SettingFile;
            file.Refresh();
            return file.Exists;
        }

        void FirstSetup()
        {

        }

        void ExecuteSetup()
        {

        }

        public bool Initialize(IEnumerable<string> arguments)
        {
            InitializeEnvironment(arguments);

            if(IsFirstStartup()) {
                FirstSetup();
            } else {
                ExecuteSetup();
            }

            return false;
        }

        #endregion
    }
}
