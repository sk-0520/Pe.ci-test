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

        private void RunMode(Mode mode)
        {
            Debug.Assert(mode != Mode.None);

            var stdIn = new StreamReader(new MemoryStream());
            var stdOut = new StreamWriter(new MemoryStream());
            var stdErr = new StreamWriter(new MemoryStream());

            try {
                NativeMethods.AllocConsole();

                /*
                Console.SetIn(stdIn);
                Console.SetOut(stdOut);
                Console.SetError(stdErr);
                */

                Console.WriteLine("asd");

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

            RunMode(mode);

            return true;
        }

        #endregion
    }
}
