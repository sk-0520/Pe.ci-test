using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationManager
    {
        public ApplicationManager()
        { }

        #region property



        #endregion

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

        public bool Startup(App app, StartupEventArgs e)
        {
            InitializeEnvironment(e.Args);

            return true;
        }

        #endregion
    }
}
