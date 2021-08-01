using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationConsoleExecutor
    {
        #region define

        enum Mode
        {
            None,
            DryRun,
        }

        #endregion

        #region property
        #endregion

        #region function

        private void RunDryRun(IEnumerable<string> arguments)
        {
            var commandLine = new CommandLine(arguments, false);
            var arg = string.Join(" ", commandLine.Arguments.Select(i => CommandLine.Escape(i)));
            Console.WriteLine(arg);
        }

        private void RunCore(Mode mode, IEnumerable<string> arguments)
        {
            Debug.Assert(mode != Mode.None);

            try {
                NativeMethods.AllocConsole();

                switch(mode) {
                    case Mode.DryRun:
                        RunDryRun(arguments);
                        break;

                    default:
                        throw new NotImplementedException();
                }

            } finally {
                NativeMethods.FreeConsole();
            }
        }

        public bool Run(string appConsoleMode, IEnumerable<string> arguments)
        {
            Mode mode;
            switch(appConsoleMode) {
                case "DRY-RUN":
                    mode = Mode.DryRun;
                    break;

                default:
                    mode = Mode.None;
                    break;
            }

            if(mode == Mode.None) {
                return false;
            }

            RunCore(mode, arguments);

            return true;
        }

        #endregion
    }
}
