using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class AcceptResult
    {
        public AcceptResult(bool accepted, UpdateKind updateKind, bool sendUsageStatistics)
        {
            Accepted = accepted;
            UpdateKind = updateKind;
            SendUsageStatistics = sendUsageStatistics;
        }

        #region property

        public bool Accepted { get; }
        public UpdateKind UpdateKind { get; }
        public bool SendUsageStatistics { get; }

        #endregion
    }
}
