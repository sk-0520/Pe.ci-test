using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public class LauncherExecuteResult
    {

    }

    public class LauncherExecutor
    {
        public LauncherExecutor(IOrderManager orderManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
            OrderManager = orderManager;
        }

        #region property

        IOrderManager OrderManager { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        public LauncherExecuteResult Execute(ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IEnumerable<LauncherEnvironmentVariableItem> environmentVariableItems, Screen screen)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }
            if(customParameter == null) {
                throw new ArgumentNullException(nameof(customParameter));
            }
            if(environmentVariableItems == null) {
                throw new ArgumentNullException(nameof(environmentVariableItems));
            }

            throw new NotImplementedException();
        }

        #endregion

        #region property
        #endregion
    }
}
