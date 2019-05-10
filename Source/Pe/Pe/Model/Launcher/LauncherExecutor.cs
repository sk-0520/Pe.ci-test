using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public class LauncherExecuteResult
    {

    }

    public class LauncherExecutor
    {
        public LauncherExecutor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        public LauncherExecuteResult Execute(LauncherFileData data, IEnumerable<LauncherEnvironmentVariableItem> environmentVariableItems)
        {
            if(data == null) {
                throw new ArgumentNullException(nameof(data));
            }


            throw new NotImplementedException();
        }

        #endregion

        #region property
        #endregion
    }
}
