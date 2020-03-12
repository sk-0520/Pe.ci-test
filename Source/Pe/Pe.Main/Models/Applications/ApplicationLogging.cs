using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NLog;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationLogging
    {
        #region property

        #endregion

        #region function

        public void ReceiveLog(LogEventInfo logEventInfo, object[] parameters)
        {
            Debug.WriteLine(logEventInfo.Level);
        }

        #endregion
    }
}
