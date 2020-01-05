using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class AcceptResult
    {
        public AcceptResult(bool accepted, bool checkUpdate, bool sendUsageStatistics)
        {
            Accepted = accepted;
            CheckUpdate = checkUpdate;
            SendUsageStatistics = sendUsageStatistics;
        }

        #region property

        public bool Accepted { get; }
        public bool CheckUpdate { get; }
        public bool SendUsageStatistics { get; }

        #endregion
    }
}
