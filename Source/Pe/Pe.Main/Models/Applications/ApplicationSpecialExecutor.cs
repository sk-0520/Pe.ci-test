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
    public class ApplicationSpecialExecutor
    {
        #region define

        enum Mode
        {
            None,
            DryRun,
        }

        class ConsoleLifetime: DisposerBase
        {
            public ConsoleLifetime()
            {
                if(NativeMethods.AttachConsole(-1)) {
                    this._attached = true;
                } else {
                    NativeMethods.AllocConsole();
                }
            }

            #region property

            bool _attached = true;

            #endregion

            #region MyRegion

            protected override void Dispose(bool disposing)
            {
                if(!IsDisposed) {
                    if(!this._attached) {
                        NativeMethods.FreeConsole();
                    }
                }
                base.Dispose(disposing);
            }

            #endregion
        }

        #endregion

        #region property
        #endregion

        #region function

        private IDisposable GetConsolelifetimeIfConsoleMode(Mode mode)
        {
            switch(mode) {
                case Mode.DryRun:
                    return new ConsoleLifetime();
            }

            return ActionDisposerHelper.CreateEmpty();
        }

        private void RunDryRun(IEnumerable<string> arguments)
        {
            var commandLine = new CommandLine(arguments, false);
            var arg = string.Join(" ", commandLine.Arguments.Select(i => CommandLine.Escape(i)));
            Console.WriteLine(arg);
        }

        private void RunCore(Mode mode, IEnumerable<string> arguments)
        {
            Debug.Assert(mode != Mode.None);

            using var consoleLifetime = GetConsolelifetimeIfConsoleMode(mode);

            switch(mode) {
                case Mode.DryRun:
                    RunDryRun(arguments);
                    break;

                default:
                    throw new NotImplementedException();
            }

        }


        public bool Run(string appSpecialMode, IEnumerable<string> arguments)
        {
            Mode mode;
            switch(appSpecialMode) {
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
