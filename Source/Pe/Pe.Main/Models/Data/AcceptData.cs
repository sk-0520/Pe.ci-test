using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class AcceptResult
    {
        public AcceptResult(bool accepted, UpdateKind updateKind, bool isEnabledTelemetry)
        {
            Accepted = accepted;
            UpdateKind = updateKind;
            IsEnabledTelemetry = isEnabledTelemetry;
        }

        #region property

        public bool Accepted { get; }
        public UpdateKind UpdateKind { get; }
        public bool IsEnabledTelemetry { get; }

        #endregion
    }
}
