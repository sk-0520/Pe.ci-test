using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    /// <summary>
    /// 本体特殊起動処理。
    /// </summary>
    /// <remarks>
    /// <para>設定・ディレクトリ・DI構築がなされていない状態で使用される。</para>
    /// </remarks>
    public class ApplicationSpecialExecutor
    {
        #region define

        private enum Mode
        {
            None,
            /// <summary>
            /// ドライラン。
            /// </summary>
            DryRun,
        }

        private sealed class ConsoleLifetime: DisposerBase
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

            private bool _attached = true;

            #endregion

            #region DisposerBase

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

        private IDisposable GetConsoleLifetimeIfConsoleMode(Mode mode)
        {
            switch(mode) {
                case Mode.DryRun:
                    return new ConsoleLifetime();
            }

            return ActionDisposerHelper.CreateEmpty();
        }

        private int RunDryRun(IEnumerable<string> arguments)
        {
            return 0;
        }

        private int RunCore(Mode mode, IEnumerable<string> arguments)
        {
            Debug.Assert(mode != Mode.None);

            using var consoleLifetime = GetConsoleLifetimeIfConsoleMode(mode);

            switch(mode) {
                case Mode.DryRun:
                    return RunDryRun(arguments);

                default:
                    throw new NotImplementedException();
            }
        }

        public int Run(string appSpecialMode, IEnumerable<string> arguments)
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
                return -1;
            }

            return RunCore(mode, arguments);
        }

        #endregion
    }
}
